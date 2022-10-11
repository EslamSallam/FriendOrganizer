using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        public NavigationItemViewModel(int _id,string _displayMember)
        {
            Id = _id;
            DisplayMember = _displayMember;
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

    }
}
