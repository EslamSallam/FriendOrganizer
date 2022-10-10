using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
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
        private readonly IFriendDetailViewModel friendDetailViewModel;

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
        public MainViewModel(INavigationViewModel navigationViewModel, IFriendDetailViewModel friendDetailViewModel)
        {
            _navigationViewModel = navigationViewModel;
            _friendDetailViewModel = friendDetailViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }
    }
}
