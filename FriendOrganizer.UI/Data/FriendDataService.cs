using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        public IEnumerable<Friend>? GetAll()
        {
            using (var ctx = new FriendOrganizerDBContext())
            {
                return ctx.Friends.AsNoTracking().ToList();
            }
            
            
            
        }
    }
}
