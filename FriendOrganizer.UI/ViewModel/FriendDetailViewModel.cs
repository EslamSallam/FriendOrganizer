using FriendOrganizer.Model;
using FriendOrganizer.UI.Command;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        public ICommand AddPhoneNumberCommand { get; }
        public ICommand RemovePhoneNumberCommand { get; set; }

        public ICommand DeleteDetailViewCommand { get; set; }

        public ObservableCollection<FriendProgrammingLanguages>? ProgrammingLanguages { get; private set; }
        public ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; }
        private FriendPhoneNumberWrapper _selectedPhoneNumber;
        private string _programmingLanguagesTxt;
        public string ProgrammingLanguagesTxt
        {
            get
            {
                return _programmingLanguagesTxt;
            }
            set
            {
                if (value != typeof(FriendOrganizer.Model.FriendProgrammingLanguages).ToString())
                {

                    _programmingLanguagesTxt = value;
                    OnPropertyChanged();
                    CheckProgrammingList();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private async void CheckProgrammingList()
        {
            HasChanges = await _programmingLanguageLookupDataService.GetProgrammingListForFriend(Friend.Id, ProgrammingLanguagesTxt);
            return;
        }

        public FriendPhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }

        private FriendWrapper friend;

        private readonly IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;

        public FriendDetailViewModel(IFriendRepository friendDataService, IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
        {
            FriendDataService = friendDataService;
            _programmingLanguageLookupDataService = programmingLanguageLookupDataService;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            SaveCommand = new DelegateCommand(onSaveExecute, onSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);
            DeleteDetailViewCommand = new DelegateCommand(OnDeleteDetailView);

            ProgrammingLanguages = new ObservableCollection<FriendProgrammingLanguages>();
            PhoneNumbers = new ObservableCollection<FriendPhoneNumberWrapper>();
        }

        private void OnDeleteDetailView(object? obj)
        {
            _eventAggregator.GetEvent<AfterDetailClosedEvent>().Publish(new AfterDetailClosedEventArgs { Id=this.Id, ViewModelName = nameof(FriendDetailViewModel)});
        }

        private bool OnRemovePhoneNumberCanExecute(object? arg)
        {
            return SelectedPhoneNumber != null;
        }

        private void OnRemovePhoneNumberExecute(object? obj)
        {
            SelectedPhoneNumber.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            FriendDataService.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = FriendDataService.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddPhoneNumberExecute(object? obj)
        {
            var newNumber = new FriendPhoneNumberWrapper(new FriendPhoneNumber());
            newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Friend.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = "";
        }

        private async void OnDeleteExecute(object? obj)
        {
            if (await FriendDataService.HasMeetingAsync(Friend.Id))
            {
                _messageDialogService.ShowInfoDialog($"{Friend.FirstName} {Friend.LastName} can't be deleted, as this friend is part of a meeting");
                return;
            }
            var res = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete {Friend.FirstName} {Friend.LastName}", "Question");
            if (res == MessageDialogService.MessageDialogResult.Cancel)
            {
                return;
            }
            FriendDataService.Remove(Friend.Model);
            await FriendDataService.SaveAsync();
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(
                new AfterDetailDeletedEventArgs
                { Id = Friend.Id ,ViewModelName = nameof(FriendDetailViewModel) }
                );
        }

        private bool onSaveCanExecute(object? arg)
        {
            return Friend != null && !Friend.HasErrors && HasChanges && PhoneNumbers.All(pn => !pn.HasErrors);
        }

        private async void onSaveExecute(object? obj)
        {
            await FriendDataService.SaveAsync();
            HasChanges = FriendDataService.HasChanges();
            Id = friend.Id;
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(
            new AfterDetailSavedEventArgs
            {
                Id = friend.Id,
                DisplayMember = $"{friend.FirstName} {friend.LastName}",
                ViewModelName = nameof(FriendDetailViewModel)
            });
            _programmingLanguageLookupDataService.UpdateProgrammingListForFriend(Friend.Id, ProgrammingLanguages);
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

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _title;
        public string Title { get { return _title; } set { _title = value; OnPropertyChanged(); } }

        public async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue ? await FriendDataService.GetByIdAsync(friendId.Value) : CreateNewFriend();
            Friend = new FriendWrapper(friend);
            //Set the Id for navigation in Details Collection
            Id = friend.Id;
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
                if (e.PropertyName == nameof(Friend.FirstName)
                    || e.PropertyName == nameof(Friend.LastName))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id == 0)
            {
                // Trick to trigger the validation
                Friend.FirstName = "";
                Friend.LastName = "";
            }
       
            InitializePhoneNumbers(friend.PhoneNumbers);

            ProgrammingLanguages.Clear();
            //ProgrammingLanguages.Add(new NullLookupItem());
            var lookupItems = await _programmingLanguageLookupDataService.GetFriendprogrammingLanguageLookupAsync(friend.Id);
            var lookup = await _programmingLanguageLookupDataService.GetprogrammingLanguageLookupAsync();
            foreach (var pl in lookup)
            {
                ProgrammingLanguages.Add(pl);
            }

            foreach (var fpl in lookupItems)
            {
                var res = ProgrammingLanguages.FirstOrDefault(f => f.Id == fpl.ProgrammingLanguageId);
                ProgrammingLanguages.First(f => f.Id == fpl.ProgrammingLanguageId).ProgrammingLanguageId = fpl.ProgrammingLanguageId;
                if (res != null)
                {
                    ProgrammingLanguages.First(f => f.Id == fpl.ProgrammingLanguageId).IsChecked = true;
                    ProgrammingLanguages.First(f => f.Id == fpl.ProgrammingLanguageId).FriendId = fpl.FriendId;
                    
                    ProgrammingLanguages.First(f => f.Id == fpl.ProgrammingLanguageId).Id = fpl.Id;
                    if (ProgrammingLanguagesTxt != null && ProgrammingLanguagesTxt != "")
                    {
                        ProgrammingLanguagesTxt += ", ";
                    }
                    ProgrammingLanguagesTxt += fpl.ProgrammingLanguageName;

                }
            }
            SetTitle();

        }

        private void SetTitle()
        {
            Title = $"{friend.FirstName} {Friend.LastName}";
        }

        private void InitializePhoneNumbers(ICollection<FriendPhoneNumber> phoneNumbers)
        {
            foreach (var item in PhoneNumbers)
            {
                item.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();
            foreach (var item in phoneNumbers)
            {
                var wrapper = new FriendPhoneNumberWrapper(item);
                wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
                PhoneNumbers.Add(wrapper);

            }
        }
        private void FriendPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = FriendDataService.HasChanges();
            }
            if (e.PropertyName == nameof(FriendPhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
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
