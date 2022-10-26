using FriendOrganizer.UI.Command;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.Services;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private IDetailViewModel _selectedDetailViewModel;
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

        public IDetailViewModel SelectedDetailViewModel
        {
            get
            {
                return _selectedDetailViewModel;
            }
            set
            {

                _selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<IDetailViewModel> _detailViewModels;

        public ObservableCollection<IDetailViewModel> DetailViewModels
        {
            get { return _detailViewModels; }
            set { _detailViewModels = value; }
        }


        public MainViewModel()
        {
        }
        public MainViewModel(INavigationViewModel navigationViewModel, Func<IFriendDetailViewModel> friendDetailViewModelCreator, Func<IMeetingDetailViewModel> meetingDetailViewModelCreator, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _navigationViewModel = navigationViewModel;
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            _meetingDetailViewModelCreator = meetingDetailViewModelCreator;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Subscribe(onOpenDetailView);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendExecute);
            CreateNewMeetingCommand = new DelegateCommand(OnCreateNewMeetingExecute);
            _detailViewModels = new ObservableCollection<IDetailViewModel>();
        }


        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }


        private void OnCreateNewFriendExecute(object? obj)
        {
            onOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = nameof(FriendDetailViewModel) });
        }
        private void OnCreateNewMeetingExecute(object? obj)
        {
            onOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = nameof(MeetingDetailViewModel) });
        }

        private async void onOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (SelectedDetailViewModel != null && SelectedDetailViewModel.HasChanges)
            {
                var res = _messageDialogService.ShowOkCancelDialog("Friend Has unsaved Changes. Do you want to navigate?", "Warning");
                if (res == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            var detailviewModel = DetailViewModels.SingleOrDefault(vm => vm.Id == args.Id && vm.GetType().Name == args.ViewModelName);
            if (detailviewModel == null)
            {
                switch (args.ViewModelName)
                {
                    case nameof(FriendDetailViewModel):
                        SelectedDetailViewModel = _friendDetailViewModelCreator();
                        break;
                    case nameof(MeetingDetailViewModel):
                        SelectedDetailViewModel = _meetingDetailViewModelCreator();
                        break;
                    default:
                        throw new Exception($"View Model {args.ViewModelName} not mapped");
                }
                await SelectedDetailViewModel.LoadAsync(args.Id);
                DetailViewModels.Add(SelectedDetailViewModel);
                detailviewModel = SelectedDetailViewModel;
            }
            SelectedDetailViewModel = detailviewModel;

        }
        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            var detailviewmodel = DetailViewModels.SingleOrDefault(vm => vm.Id == args.Id && vm.GetType().Name == args.ViewModelName);
            if (detailviewmodel != null)
            {
                DetailViewModels.Remove(detailviewmodel);
            }
        }
    }
}
