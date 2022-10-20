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
        private IDetailViewModel _detailViewModel;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        public ICommand CreateNewFriendCommand { get; }


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
        public MainViewModel(INavigationViewModel navigationViewModel, Func<IFriendDetailViewModel> friendDetailViewModelCreator, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _navigationViewModel = navigationViewModel;
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Subscribe(onOpenFriendDetailView);
            _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Subscribe(AfterFriendDeleted);
            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);
        }
        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }


        private void OnCreateNewFriendExecute(object? obj)
        {
            onOpenFriendDetailView(null);
        }

        private async void onOpenFriendDetailView(int? friendId)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var res = _messageDialogService.ShowOkCancelDialog("Friend Has unsaved Changes. Do you want to navigate?", "Warning");
                if (res == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            DetailViewModel = _friendDetailViewModelCreator();
            await DetailViewModel.LoadAsync(friendId);
        }
        private void AfterFriendDeleted(int obj)
        {
            DetailViewModel = null;
        }
    }
}
