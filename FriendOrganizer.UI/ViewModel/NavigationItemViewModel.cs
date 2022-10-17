using FriendOrganizer.UI.Command;
using FriendOrganizer.UI.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        public NavigationItemViewModel(int _id,string _displayMember,IEventAggregator eventAggregator)
        {
            Id = _id;
            DisplayMember = _displayMember;
            OpenFriendDetailViewCommand = new DelegateCommand(OnOpenFriendDetailView);
            _eventAggregator = eventAggregator;
        }

     

        private string? displayMember;

        public string? DisplayMember
        {
            get { return displayMember; }
            set 
            {
                displayMember = value;
                OnPropertyChanged();    
            }
        }

        public int Id { get; set; }

        public ICommand OpenFriendDetailViewCommand { get; }

        private IEventAggregator _eventAggregator;
        private void OnOpenFriendDetailView(object? obj)
        {
            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Publish(Id);
        }
    }
}
