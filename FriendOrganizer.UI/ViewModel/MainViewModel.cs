using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationViewModel _navigationViewModel;
        private IFriendDetailViewModel _friendDetailViewModel;
        private readonly IEventAggregator _eventAggregator;

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
        }

        public MainViewModel()
        {
        }
        public MainViewModel(INavigationViewModel navigationViewModel, IFriendDetailViewModel friendDetailViewModel,IEventAggregator eventAggregator)
        {
            _navigationViewModel = navigationViewModel;
            _friendDetailViewModel = friendDetailViewModel;
            _eventAggregator = eventAggregator;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }
    }
}
