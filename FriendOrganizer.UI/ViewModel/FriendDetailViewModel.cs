using FriendOrganizer.Model;
using FriendOrganizer.UI.Command;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.Services;
using FriendOrganizer.UI.Wrapper;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
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

        private IProgrammingLanguageLookupDataService _programmingLanguageLookupDataService;

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ObservableCollection<LookupItem> ProgrammingLanguages { get; private set; }

        private FriendWrapper friend;

        private readonly IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;

        public FriendDetailViewModel(IFriendRepository friendDataService, IEventAggregator eventAggregator,IMessageDialogService messageDialogService,IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
        {
            FriendDataService = friendDataService;
            _programmingLanguageLookupDataService = programmingLanguageLookupDataService;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            SaveCommand = new DelegateCommand(onSaveExecute, onSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
        }

        private async void OnDeleteExecute(object? obj)
        {
            var res = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete {Friend.FirstName} {Friend.LastName}","Question");
            if (res == MessageDialogService.MessageDialogResult.Cancel)
            {
                return;
            }
            FriendDataService.Remove(Friend.Model);
            await FriendDataService.SaveAsync();
            _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Publish(Friend.Id);
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


        public async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue ? await FriendDataService.GetByIdAsync(friendId.Value) : CreateNewFriend();
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
            if (Friend.Id == 0)
            {
                // Trick to trigger the validation
                Friend.FirstName = "";
                Friend.LastName = "";
            }

            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem());
            var lookupItems = await _programmingLanguageLookupDataService.GetprogrammingLanguageLookupAsync();
            foreach (var pl in lookupItems)
            {
                ProgrammingLanguages.Add(pl);
            }

        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            FriendDataService.Add(friend);
            return friend;
        }
    }
}
