using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository : IGenericRepository<Friend>
    {
        void RemovePhoneNumber(FriendPhoneNumber model);
        void UpdateProgrammingCheckList(ObservableCollection<FriendProgrammingLanguages> programmingLanguages, int friendId);
    }
}