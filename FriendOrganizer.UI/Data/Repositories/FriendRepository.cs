using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Data.Entity;
using System.Data.Entity.Migrations.Builders;
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
            return await _context.Friends.Include(f => f.PhoneNumbers).SingleAsync(f => f.Id == FriendId);
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
    }
}
