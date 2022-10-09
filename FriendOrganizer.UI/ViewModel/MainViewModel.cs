using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IFriendDataService _friendDataService;
        private Friend _selectedFriend;


        public ObservableCollection<Friend> Friends { get; set; }
        public MainViewModel(IFriendDataService friendDataService)
        {
            _friendDataService = friendDataService;
            Friends = new ObservableCollection<Friend>();

        }

        public void Load()
        {
            var friends = _friendDataService.GetAll();
            Friends.Clear();
            foreach (Friend friend in friends)
            {
                Friends.Add(friend);
            }
        }


        public Friend SelectedFriend
        {
            get { return _selectedFriend; }
            set { _selectedFriend = value; OnPropertyChanged(); }
        }
    }
}
