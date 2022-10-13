using FriendOrganizer.Model;
using FriendOrganizer.UI.Command;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Events;
using Microsoft.VisualBasic;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        private IFriendLookupDataService _friendLookupService { get; }
        public ObservableCollection<NavigationItemViewModel> Friends { get; }
        private NavigationItemViewModel _selectedFriend;

        public NavigationItemViewModel SelectedFriend
        {
            get { return _selectedFriend; }
            set 
            {
                _selectedFriend = value; 
                OnPropertyChanged();
                if (_selectedFriend != null)
                {
                    _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Publish(_selectedFriend.Id);
                }
            }
        }


        public NavigationViewModel()
        {

        }

        public NavigationViewModel(IFriendLookupDataService friendLookupService,IEventAggregator eventAggregator)
        {
            _friendLookupService = friendLookupService;
            _eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Subscribe(onFriendSaveChanges);
        }

        private void onFriendSaveChanges(AfterFriendSavedEventArgs UpdatedFriend)
        {
            var lookupItem = Friends.Single(f => f.Id == UpdatedFriend.Id);
            lookupItem.DisplayMember = UpdatedFriend.DisplayMember;
        }

        public async Task LoadAsync()
        {
            var lookup = await _friendLookupService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var f in lookup)
            {
                Friends.Add(new NavigationItemViewModel(f.Id,f.DisplayMember));
            }
        }

    }
}
