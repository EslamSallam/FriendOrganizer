using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        private Func<FriendOrganizerDBContext> _contextCreator { get; }
        public FriendDataService(Func<FriendOrganizerDBContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }


        public async Task<Friend>? GetByIdAsync(int FriendId)
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Friends.AsNoTracking().SingleAsync(f => f.Id == FriendId);
            }
        }
    }
}
