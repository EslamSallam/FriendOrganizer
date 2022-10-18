using FriendOrganizer.Model;
using FriendOrganizer.UI.Command;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.Services;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static FriendOrganizer.UI.Services.MessageDialogService;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationViewModel _navigationViewModel;
        private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
        private IFriendDetailViewModel _friendDetailViewModel;
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

        public IFriendDetailViewModel FriendDetailViewModel
        {
            get
            {
                return _friendDetailViewModel;
            }
           private set
            {

                _friendDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
        }
        public MainViewModel(INavigationViewModel navigationViewModel, Func<IFriendDetailViewModel> friendDetailViewModelCreator,IEventAggregator eventAggregator,IMessageDialogService messageDialogService)
        {
            _navigationViewModel = navigationViewModel;
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Subscribe(onOpenFriendDetailView);
            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);
        }

        private void OnCreateNewFriendExecute(object? obj)
        {
            onOpenFriendDetailView(null);
        }

        private async void onOpenFriendDetailView(int? friendId)
        {
            if(FriendDetailViewModel!=null && FriendDetailViewModel.HasChanges)
            {
               var res = _messageDialogService.ShowOkCancelDialog("Friend Has unsaved Changes. Do you want to navigate?", "Warning");
                if (res == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            FriendDetailViewModel = _friendDetailViewModelCreator();
            await FriendDetailViewModel.LoadAsync(friendId);
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }
    }
}
