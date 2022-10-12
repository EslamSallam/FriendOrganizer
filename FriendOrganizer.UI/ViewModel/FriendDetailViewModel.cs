using FriendOrganizer.Model;
using FriendOrganizer.UI.Command;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.Wrapper;
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
        public FriendWrapper Friend
        {
            get { return friend; }
            set { friend = value; OnPropertyChanged(); }
        }
        public IFriendDataService FriendDataService { get; }
        public ICommand SaveCommand { get; }
        private FriendWrapper friend;
        private readonly IEventAggregator _eventAggregator;

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
            await FriendDataService.SaveFriendAsync(friend.Model);
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
            var friend = await FriendDataService.GetByIdAsync(friendId);
            Friend = new FriendWrapper(friend);
                
        }



    }
}
