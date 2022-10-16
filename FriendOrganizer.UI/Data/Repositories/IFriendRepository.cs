﻿using FriendOrganizer.Model;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface IFriendRepository
    {
        Task<Friend>? GetByIdAsync(int FriendId);

        Task SaveAsync();
        bool HasChanges();
    }
}