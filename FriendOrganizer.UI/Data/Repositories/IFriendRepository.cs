using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository
    {
        Task<Friend>? GetByIdAsync(int? FriendId);

        Task SaveAsync();
        bool HasChanges();
        void Add(Friend friend);
        void Remove(Friend model);
        void RemovePhoneNumber(FriendPhoneNumber model);
        void UpdateProgrammingCheckList(ObservableCollection<FriendProgrammingLanguages> programmingLanguages, int friendId);
    }
}