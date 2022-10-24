using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Lookups;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class LookupDataService : IFriendLookupDataService, IMeetingLookupDataService, IProgrammingLanguageLookupDataService
    {
        private Func<FriendOrganizerDBContext> _contextCreator { get; }
        public LookupDataService(Func<FriendOrganizerDBContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetFriendLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Friends.AsNoTracking().Select(f => new LookupItem { Id = f.Id, DisplayMember = f.FirstName + " " + f.LastName }).ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetMeetingLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Meetings.AsNoTracking().Select(f => new LookupItem { Id = f.Id, DisplayMember = f.Title }).ToListAsync();
            }
        }


        public async Task<IEnumerable<FriendProgrammingLanguages>> GetprogrammingLanguageLookupAsync()
        {
            using (var ctx = _contextCreator())
            {

                return await ctx.ProgrammingLanguages.AsNoTracking().Select(p => new FriendProgrammingLanguages { Id = p.Id, DisplayMember = p.Name, IsChecked = false, ProgrammingLanguageId = p.Id }).ToListAsync();

            }
        }

        public async Task<IEnumerable<FriendProgrammingLanguages>> GetFriendprogrammingLanguageLookupAsync(int friendId)
        {


            using (var ctx = _contextCreator())
            {
                var fullEntries = await ctx.FriendProgrammingLanguage
               .Join(
                   ctx.Friends,
                   fpl => fpl.FriendId,
                   f => f.Id,
                   (fpl, f) => new { fpl, f }
               )
               .Join(
                   ctx.ProgrammingLanguages,
                   CombinedPl => CombinedPl.fpl.ProgrammingLanguageId,
                   pl => pl.Id,
                   (CombinedPl, pl) => new
                   {
                       Id = CombinedPl.fpl.Id,
                       FriendId = CombinedPl.fpl.FriendId,
                       PrgrammingLangId = CombinedPl.fpl.ProgrammingLanguageId,
                       ProgrammingLangName = pl.Name
                   }
               )
               .Where(fullEntry => fullEntry.FriendId == friendId).ToListAsync();
                List<FriendProgrammingLanguages> FriendProgrammingLanguages = new List<FriendProgrammingLanguages>();
                foreach (var item in fullEntries)
                {
                    FriendProgrammingLanguages fpl = new FriendProgrammingLanguages { Id = item.Id, ProgrammingLanguageName = item.ProgrammingLangName, ProgrammingLanguage = new ProgrammingLanguage { Id = item.PrgrammingLangId, Name = item.ProgrammingLangName }, ProgrammingLanguageId = item.PrgrammingLangId, FriendId = item.FriendId, Friend = new Friend { Id = friendId } };
                    FriendProgrammingLanguages.Add(fpl);
                }
                return FriendProgrammingLanguages;
            }
        }

        public async Task<bool> GetProgrammingListForFriend(int friendId, string LanguagesName)
        {
            var lookupData = await GetFriendprogrammingLanguageLookupAsync(friendId);
            string previousProgrammingLanguages = "";
            foreach (FriendProgrammingLanguages item in lookupData)
            {
                if (previousProgrammingLanguages != "")
                {
                    previousProgrammingLanguages += ", ";
                }
                previousProgrammingLanguages += item.ProgrammingLanguageName;
            }
            return previousProgrammingLanguages != LanguagesName;
        }

        public void UpdateProgrammingListForFriend(int friendId, IEnumerable<FriendProgrammingLanguages> ProgrammingLanguages)
        {
            using (var ctx = _contextCreator())
            {
                //EntityFramework Bulk Remove
                //Remove Friend Progarmming Languages
                IEnumerable<FriendProgrammingLanguage> oldPL = ctx.FriendProgrammingLanguage.Where(f => f.FriendId == friendId);
                ctx.FriendProgrammingLanguage.RemoveRange(oldPL);
                ctx.SaveChanges();
                //Add New Rows to DB
                // define INSERT query with parameters
                StringBuilder query = new StringBuilder("INSERT INTO dbo.FriendProgrammingLanguage (FriendId, ProgrammingLanguageId) ");
                bool hasvalue = false;

                foreach (var item in ProgrammingLanguages)
                {
                    if (item.IsChecked)
                    {
                        if (!hasvalue)
                        {
                            query.Append("VALUES (" + friendId.ToString() + "," + item.ProgrammingLanguageId.ToString() + ") ,");
                        }
                        else
                        {
                            query.Append(" (" + friendId.ToString() + "," + item.ProgrammingLanguageId.ToString() + ") ,");
                        }
                        hasvalue = true;
                    }
                }
                if (hasvalue)
                {
                    query.Remove(query.Length - 1, 1);
                    // create connection and command
                    using (SqlConnection cn = new SqlConnection(GetConnectionString()))
                    using (SqlCommand cmd = new SqlCommand(query.ToString(), cn))
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }


            }
        }
        private string GetConnectionString()
        {
            return "Server=.;Database=FriendOrganizerDb;Trusted_Connection=True;";
        }

       
    }

}

