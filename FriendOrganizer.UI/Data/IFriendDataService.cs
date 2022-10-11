using FriendOrganizer.Model;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public interface IFriendDataService
    {
        Task<Friend>? GetByIdAsync(int FriendId);

        Task SaveFriendAsync(Friend friend);
    }
}