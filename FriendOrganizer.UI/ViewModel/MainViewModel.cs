using FriendOrganizer.UI.Command;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.Services;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using static FriendOrganizer.UI.Services.MessageDialogService;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationViewModel _navigationViewModel;
        private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
        private Func<IMeetingDetailViewModel> _meetingDetailViewModelCreator;
        private IDetailViewModel _detailViewModel;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        public ICommand CreateNewFriendCommand { get; }
        public ICommand CreateNewMeetingCommand { get; }


        public INavigationViewModel NavigationViewModel
        {
            get
            {
                return _navigationViewModel;
            }
        }

        public IDetailViewModel DetailViewModel
        {
            get
            {
                return _detailViewModel;
            }
            private set
            {

                _detailViewModel = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
        }
        public MainViewModel(INavigationViewModel navigationViewModel, Func<IFriendDetailViewModel> friendDetailViewModelCreator,Func<IMeetingDetailViewModel> meetingDetailViewModelCreator, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _navigationViewModel = navigationViewModel;
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            _meetingDetailViewModelCreator = meetingDetailViewModelCreator;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Subscribe(onOpenDetailView);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            _eventAggregator.GetEvent<AfterDetailClosedEvent>().Subscribe(AfterDetailClosed);
            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);
            CreateNewMeetingCommand = new DelegateCommand(OnCreateNewMeetingExecute);
        }

        private void AfterDetailClosed(AfterDetailClosedEventArgs args)
        {
            var detailVM = DetailViewModels.SingleOrDefault(vm => vm.Id == args.Id && vm.GetType().Name == args.ViewModelName);
            if (detailVM != null)
            {
                DetailViewModels.Remove(detailVM);
            }
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }


        private void OnCreateNewFriendExecute(object? obj)
        {
            onOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = nameof(FriendDetailViewModel)});
        }
        private void OnCreateNewMeetingExecute(object? obj)
        {
            onOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = nameof(MeetingDetailViewModel) });
        }

        private async void onOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var res = _messageDialogService.ShowOkCancelDialog("Friend Has unsaved Changes. Do you want to navigate?", "Warning");
                if (res == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    DetailViewModel = _friendDetailViewModelCreator();
                    break;
                case nameof(MeetingDetailViewModel):
                    DetailViewModel = _meetingDetailViewModelCreator();
                    break;
                default:
                    throw new Exception($"View Model {args.ViewModelName} not mapped");
            }
            await DetailViewModel.LoadAsync(args.Id);
        }
        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }
    }
}
