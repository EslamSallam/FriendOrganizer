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
        public NavigationItemViewModel(int _id,string _displayMember,string _detailViewModelName,IEventAggregator eventAggregator)
        {
            Id = _id;
          this._detailViewModelName = _detailViewModelName;
            DisplayMember = _displayMember;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
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

        private string _detailViewModelName;

        public ICommand OpenDetailViewCommand { get; }

        private IEventAggregator _eventAggregator;
        private void OnOpenDetailViewExecute(object? obj)
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Publish(new OpenDetailViewEventArgs() {  Id = Id, ViewModelName = _detailViewModelName});
        }
    }
}
