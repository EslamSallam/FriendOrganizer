using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository : IGenericRepository<Friend>
    {
        void RemovePhoneNumber(FriendPhoneNumber model);
        Task<bool> HasMeetingAsync(int friendId);
        void UpdateProgrammingCheckList(ObservableCollection<FriendProgrammingLanguages> programmingLanguages, int friendId);
    }
}