using FriendOrganizer.Model;
using FriendOrganizer.UI.Command;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class MeetingDetailViewModel : ViewModelBase, IMeetingDetailViewModel
    {

        public IMeetingRepository _meetingRepository { get; }
        public IEventAggregator _eventAggregator;
        public IMessageDialogService _messageDialogService;
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddFriendCommand { get; }
        public ICommand RemoveFriendCommand { get; }
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

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _title;
        public string Title { get { return _title; } set { _title = value; OnPropertyChanged(); } }

        private MeetingWrapper meeting;
        public MeetingWrapper Meeting
        {
            get { return meeting; } 
            set
            {
                meeting = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Friend> _addedFriends;

        public ObservableCollection<Friend> AddedFriends
        {
            get { return _addedFriends; }
            set { _addedFriends = value; }
        }

        private ObservableCollection<Friend> _availabeFriends;

        public ObservableCollection<Friend> AvailableFriends
        {
            get { return _availabeFriends; }
            set { _availabeFriends = value; }
        }

        private Friend _selectedAddedFriend;

        public Friend SelectedAddedFriend
        {
            get { return _selectedAddedFriend; }
            set 
            {
                _selectedAddedFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveFriendCommand).RaiseCanExecuteChanged();
            }
        }

        private Friend _selectedAvailableFriend;
        private List<Friend> _allFriends;

        public Friend SelectedAvailableFriend
        {
            get { return _selectedAvailableFriend; }
            set 
            { 
                _selectedAvailableFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)AddFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public MeetingDetailViewModel(IMeetingRepository meetingRepository,IEventAggregator eventAggregator,IMessageDialogService messageDialogService)
        {

            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _meetingRepository = meetingRepository;
            AddedFriends = new ObservableCollection<Friend>();
            AvailableFriends = new ObservableCollection<Friend>();
            AddFriendCommand = new DelegateCommand(AddFriendToMeeting,AddFriendToMeetingCanExecute);
            RemoveFriendCommand = new DelegateCommand(RemoveFriendFromMeeting,RemoveFriendFromMeetingCanExecute);
            SaveCommand = new DelegateCommand(onSaveExecute, onSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

     

        private bool AddFriendToMeetingCanExecute(object? arg)
        {
            return SelectedAvailableFriend != null;
        }
        private void AddFriendToMeeting(object? obj)
        {
            var friendToAdd = SelectedAvailableFriend;
            Meeting.Model.Friends.Add(friendToAdd);
            AddedFriends.Add(friendToAdd);
            AvailableFriends.Remove(friendToAdd);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }
        private bool RemoveFriendFromMeetingCanExecute(object? arg)
        {
            return SelectedAddedFriend != null;
        }
        private void RemoveFriendFromMeeting(object? obj)
        {
           var friendToRemove = SelectedAddedFriend;
            Meeting.Model.Friends.Remove(friendToRemove);
            AddedFriends.Remove(friendToRemove);
            AvailableFriends.Add(friendToRemove);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool onSaveCanExecute(object? arg)
        {
            return meeting != null && !meeting.HasErrors && HasChanges;
        }
        private async void onSaveExecute(object? obj)
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            Id = meeting.Id;
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(
            new AfterDetailSavedEventArgs
            {
                Id = meeting.Id,
                DisplayMember = $"{meeting.Title}",
                ViewModelName = nameof(MeetingDetailViewModel)
            });
        }

        private async void OnDeleteExecute(object? obj)
        {
            var res = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete {meeting.Title}", "Question");
            if (res == MessageDialogService.MessageDialogResult.Cancel)
            {
                return;
            }
            _meetingRepository.Remove(meeting.Model);
            await _meetingRepository.SaveAsync();
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(
                new AfterDetailDeletedEventArgs
                { Id = meeting.Id, ViewModelName = nameof(MeetingDetailViewModel) }
                );
        }


        public async Task LoadAsync(int? MeetingId)
        {
            var __meeting = MeetingId.HasValue ? await _meetingRepository.GetByIdAsync(MeetingId.Value) : CreateNewMeeting();
            Meeting = new MeetingWrapper(__meeting);
            //Set the Id for Navigation for the Detail collection
            Id = Meeting.Id;
            Meeting.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _meetingRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
                if (e.PropertyName == nameof(Meeting.Title))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Meeting.Id == 0)
            {
                // Trick to trigger the validation
                Meeting.Title = "";
            }

            // Load all Friends 
            _allFriends = await _meetingRepository.getallFriendsAsync();

            SetupPickList();
            SetTitle();
        }

        private void SetTitle()
        {
            Title = Meeting.Title; 
        }

        private void SetupPickList()
        {
            var meetingFriendsIds = Meeting.Model.Friends.Select(f => f.Id).ToList();
            var addedFriends = _allFriends.Where(f => meetingFriendsIds.Contains(f.Id)).OrderBy(f => f.FirstName);
            var availableFriends = _allFriends.Except(addedFriends).OrderBy(f => f.FirstName);

            AddedFriends.Clear();
            AvailableFriends.Clear();
            foreach (var addedFriend in addedFriends)
            {
                AddedFriends.Add(addedFriend);
            }
            foreach (var availableFriend in availableFriends)
            {
                AvailableFriends.Add(availableFriend);
            }
        }

        public Meeting CreateNewMeeting()
        {
            var Meeting = new Meeting();
            _meetingRepository.Add(Meeting);
            return  Meeting;
        }
    }
}
