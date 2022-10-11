using FriendOrganizer.Model;
using FriendOrganizer.UI.Command;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        public IFriendDataService FriendDataService { get; }
        public ICommand SaveCommand { get; }

        public FriendDetailViewModel(IFriendDataService friendDataService,IEventAggregator eventAggregator)
        {
            FriendDataService = friendDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Subscribe(onOpenFriendDetailView);
            SaveCommand = new DelegateCommand(onSaveExecute,onSaveCanExecute);
        }

        private bool onSaveCanExecute(object? arg)
        {
            //TODO :: check for vaild input Friend
            return true;
        }

        private async void onSaveExecute(object? obj)
        {
            await FriendDataService.SaveFriendAsync(friend);
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(
            new AfterFriendSavedEventArgs 
            {   Id = friend.Id,
                DisplayMember = $"{friend.FirstName} {friend.LastName}"
            });
        }

        private async void onOpenFriendDetailView(int friendId)
        {
             await LoadAsync(friendId);
        }

        public async Task LoadAsync(int friendId)
        {
            Friend = await FriendDataService.GetByIdAsync(friendId);
        }
        private Friend friend;
        private readonly IEventAggregator _eventAggregator;

        public Friend Friend
        {
            get { return friend; }
            set { friend = value; OnPropertyChanged();}
        }

    }
}
