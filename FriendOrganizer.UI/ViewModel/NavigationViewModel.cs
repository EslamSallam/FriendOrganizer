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

        public NavigationViewModel()
        {

        }

        public NavigationViewModel(IFriendLookupDataService friendLookupService,IEventAggregator eventAggregator)
        {
            _friendLookupService = friendLookupService;
            _eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Subscribe(onFriendSaveChanges);
            _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Subscribe(onFriendDeleted);
        }

        private void onFriendDeleted(int friendId)
        {
            var FriendModel = Friends.SingleOrDefault(f => f.Id == friendId);
            if (FriendModel != null)
            {
                Friends.Remove(FriendModel);
            }
        }

        private void onFriendSaveChanges(AfterFriendSavedEventArgs UpdatedFriend)
        {
            var lookupItem = Friends.SingleOrDefault(f => f.Id == UpdatedFriend.Id);
            if (lookupItem == null)
            {
                Friends.Add(new NavigationItemViewModel(UpdatedFriend.Id, UpdatedFriend.DisplayMember, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = UpdatedFriend.DisplayMember;
            }
        }

        public async Task LoadAsync()
        {
            var lookup = await _friendLookupService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var f in lookup)
            {
                Friends.Add(new NavigationItemViewModel(f.Id,f.DisplayMember, _eventAggregator));
            }
        }

    }
}
