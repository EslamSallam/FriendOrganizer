﻿using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : INavigationViewModel
    {
        private IFriendLookupDataService _friendLookupService { get; }
        public ObservableCollection<LookupItem> Friends { get; }

        public NavigationViewModel()
        {

        }

        public NavigationViewModel(IFriendLookupDataService friendLookupService)
        {
            _friendLookupService = friendLookupService;
            Friends = new ObservableCollection<LookupItem>();
        }

        public async Task LoadAsync()
        {
            var lookup = await _friendLookupService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var f in lookup)
            {
                Friends.Add(f);
            }
        }

    }
}
