using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        public IFriendDataService FriendDataService { get; }

        public FriendDetailViewModel(IFriendDataService friendDataService)
        {
            FriendDataService = friendDataService;
        }
        public async Task LoadAsync(int friendId)
        {
            Friend = await FriendDataService.GetByIdAsync(friendId);
        }
        private Friend friend;

        public Friend Friend
        {
            get { return friend; }
            set { friend = value; OnPropertyChanged(); }
        }

    }
}
