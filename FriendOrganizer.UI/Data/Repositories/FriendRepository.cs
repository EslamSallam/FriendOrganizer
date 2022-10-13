using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Data.Entity;
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

        public async Task<Friend>? GetByIdAsync(int FriendId)
        {
            return await _context.Friends.SingleAsync(f => f.Id == FriendId);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
