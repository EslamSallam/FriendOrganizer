using FriendOrganizer.UI.Command;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.Wrapper;
using Prism.Events;
using System.Threading.Tasks;
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
        public IFriendRepository FriendDataService { get; }
        public ICommand SaveCommand { get; }
        private FriendWrapper friend;

        private readonly IEventAggregator _eventAggregator;


        public FriendDetailViewModel(IFriendRepository friendDataService, IEventAggregator eventAggregator)
        {
            FriendDataService = friendDataService;
            _eventAggregator = eventAggregator;
            SaveCommand = new DelegateCommand(onSaveExecute, onSaveCanExecute);
        }

        private bool onSaveCanExecute(object? arg)
        {
            //TODO :: check for vaild input Friend
            return Friend != null && !Friend.HasErrors && HasChanges;
        }

        private async void onSaveExecute(object? obj)
        {
            await FriendDataService.SaveAsync();
            HasChanges = FriendDataService.HasChanges();
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(
            new AfterFriendSavedEventArgs
            {
                Id = friend.Id,
                DisplayMember = $"{friend.FirstName} {friend.LastName}"
            });
        }

        private bool _hasChanges;

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }


        public async Task LoadAsync(int friendId)
        {
            var friend = await FriendDataService.GetByIdAsync(friendId);
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = FriendDataService.HasChanges();
                }
                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }
}
