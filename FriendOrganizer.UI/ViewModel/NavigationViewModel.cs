using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Events;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        private IFriendLookupDataService _friendLookupService { get; }
        private IMeetingLookupDataService _meetingLookupService { get; }
        public ObservableCollection<NavigationItemViewModel> Friends { get; }
        public ObservableCollection<NavigationItemViewModel> Meetings { get; }

        public NavigationViewModel()
        {

        }

        public NavigationViewModel(IFriendLookupDataService friendLookupService,IMeetingLookupDataService meetingLookupService, IEventAggregator eventAggregator)
        {
            _friendLookupService = friendLookupService;
            _meetingLookupService = meetingLookupService;   
            _eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSavedExecute);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(onDetailDeletedExecute);
        }

        private void onDetailDeletedExecute(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    DetailDeletedExecute(Friends,args);
                    break;
                case nameof(MeetingDetailViewModel):
                    DetailDeletedExecute(Meetings, args);
                    break;
            }
        }

        private void DetailDeletedExecute(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(i => i.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        private void AfterDetailSavedExecute(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    DetailSavedExecute(Friends, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    DetailSavedExecute(Meetings, args);
                    break;
            }
        }

        private void DetailSavedExecute(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(i => i.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, args.ViewModelName, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }

        public async Task LoadAsync()
        {
            var lookup = await _friendLookupService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var f in lookup)
            {
                Friends.Add(new NavigationItemViewModel(f.Id, f.DisplayMember, nameof(FriendDetailViewModel).ToString(), _eventAggregator));
            }
            lookup = await _meetingLookupService.GetMeetingLookupAsync();
            Meetings.Clear();
            foreach (var m in lookup)
            {
                Meetings.Add(new NavigationItemViewModel(m.Id,m.DisplayMember,nameof(MeetingDetailViewModel).ToString(), _eventAggregator));
            }
            
        }

    }
}
