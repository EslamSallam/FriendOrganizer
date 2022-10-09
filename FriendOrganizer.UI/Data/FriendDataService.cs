using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        public IEnumerable<Friend> GetAll()
        {
            yield return new Friend { FirstName = "Eslam", LastName = "Hussien" };
            yield return new Friend { FirstName = "Ahmed", LastName = "Hamed" };
            yield return new Friend { FirstName = "Maged", LastName = "Ahmed" };
            yield return new Friend { FirstName = "Hazem", LastName = "Khaled" };
        }
    }
}
