using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? Id);
        bool HasChanges { get; }
        public int Id { get; }
        public string Title { get; set; }
    }
}