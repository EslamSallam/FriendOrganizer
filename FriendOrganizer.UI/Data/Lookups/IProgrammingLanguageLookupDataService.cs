using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public interface IProgrammingLanguageLookupDataService
    {
        Task<IEnumerable<FriendProgrammingLanguages>> GetprogrammingLanguageLookupAsync();

        Task<IEnumerable<FriendProgrammingLanguages>> GetFriendprogrammingLanguageLookupAsync(int friendId);
        Task<bool> GetProgrammingListForFriend(int FriendId, string LanguagesName);
        void UpdateProgrammingListForFriend(int id, IEnumerable<FriendProgrammingLanguages> ProgrammingLanguages);
    }
}