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
        public MeetingDetailViewModel(IMeetingRepository meetingRepository,IEventAggregator eventAggregator,IMessageDialogService messageDialogService)
        {

            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _meetingRepository = meetingRepository;
            SaveCommand = new DelegateCommand(onSaveExecute, onSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        private bool onSaveCanExecute(object? arg)
        {
            return meeting != null && !meeting.HasErrors && HasChanges;
        }
        private async void onSaveExecute(object? obj)
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
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
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Meeting.Id == 0)
            {
                // Trick to trigger the validation
                Meeting.Title = "";
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
