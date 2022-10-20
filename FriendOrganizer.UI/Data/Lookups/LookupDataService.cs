using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Lookups;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class LookupDataService : IFriendLookupDataService, IProgrammingLanguageLookupDataService
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

        public async Task<IEnumerable<FriendProgrammingLanguages>> GetprogrammingLanguageLookupAsync()
        {
            using (var ctx = _contextCreator())
            {

                return await ctx.ProgrammingLanguages.AsNoTracking().Select(p => new FriendProgrammingLanguages {Id = p.Id, DisplayMember = p.Name, IsChecked = false }).ToListAsync();

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
                    FriendProgrammingLanguages fpl = new FriendProgrammingLanguages { Id = item.Id, ProgrammingLanguageName = item.ProgrammingLangName, ProgrammingLanguageId = item.PrgrammingLangId, FriendId = item.FriendId };
                    FriendProgrammingLanguages.Add(fpl);
                }
                return FriendProgrammingLanguages;
            }
        }
    }
}
