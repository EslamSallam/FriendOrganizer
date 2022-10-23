using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class FriendRepository : GenericRepository<Friend, FriendOrganizerDBContext>,IFriendRepository 
    {
        public FriendRepository(FriendOrganizerDBContext context)
            : base(context)
        {
        }

        public override async Task<Friend>? GetByIdAsync(int? FriendId)
        {
            return await Context.Friends.Include(f => f.FriendProgrammingLanguages).Include(f => f.PhoneNumbers).SingleAsync(f => f.Id == FriendId);
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            Context.FriendPhoneNumber.Remove(model);
        }

        public void UpdateProgrammingCheckList(ObservableCollection<FriendProgrammingLanguages> programmingLanguages, int friendId)
        {
            foreach (var item in programmingLanguages)
            {
                var FriendPl = new FriendProgrammingLanguage { Id = item.Id};
                FriendPl.FriendId = friendId;
                FriendPl.Friend = Context.Friends.Single(f => f.Id == friendId);
                FriendPl.ProgrammingLanguage = Context.ProgrammingLanguages.Single(f => f.Id == FriendPl.ProgrammingLanguageId);
                if (item.IsChecked)
                {
                    Context.FriendProgrammingLanguage.AddOrUpdate(FriendPl);
                }
                else
                {
                    var res = Context.FriendProgrammingLanguage.Find(FriendPl);
                    if (res != null)
                    {
                        Context.FriendProgrammingLanguage.Remove(FriendPl);
                    }
                }

            }
        }
    }
}
