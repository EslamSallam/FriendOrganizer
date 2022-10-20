using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private FriendOrganizerDBContext _context { get; }
        public FriendRepository(FriendOrganizerDBContext context)
        {
            _context = context;
        }

        public async Task<Friend>? GetByIdAsync(int? FriendId)
        {
            return await _context.Friends.Include(f => f.FriendProgrammingLanguages).Include(f => f.PhoneNumbers).SingleAsync(f => f.Id == FriendId);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Add(Friend friend)
        {
            _context.Friends.Add(friend);
        }

        public void Remove(Friend model)
        {
            _context.Friends.Remove(model);
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            _context.FriendPhoneNumber.Remove(model);
        }

        public void UpdateProgrammingCheckList(ObservableCollection<FriendProgrammingLanguages> programmingLanguages, int friendId)
        {
            foreach (var item in programmingLanguages)
            {
                var FriendPl = new FriendProgrammingLanguage { Id = item.Id};
                FriendPl.FriendId = friendId;
                FriendPl.Friend = _context.Friends.Single(f => f.Id == friendId);
                FriendPl.ProgrammingLanguage = _context.ProgrammingLanguages.Single(f => f.Id == FriendPl.ProgrammingLanguageId);
                if (item.IsChecked)
                {
                    _context.FriendProgrammingLanguage.AddOrUpdate(FriendPl);
                }
                else
                {
                    var res = _context.FriendProgrammingLanguage.Find(FriendPl);
                    if (res != null)
                    {
                        _context.FriendProgrammingLanguage.Remove(FriendPl);
                    }
                }

            }
        }
    }
}
