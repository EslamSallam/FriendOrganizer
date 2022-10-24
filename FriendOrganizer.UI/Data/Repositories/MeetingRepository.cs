using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, FriendOrganizerDBContext>, IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDBContext _context) : base(_context)
        {
        }

        public override async Task<Meeting>? GetByIdAsync(int? Id)
        {
            return await Context.Meetings.Include(m => m.Friends).SingleAsync(m => m.Id == Id);
        }
    }
}
