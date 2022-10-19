using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
    public class Friend
    {
        public Friend()
        {
            PhoneNumbers = new Collection<FriendPhoneNumber>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        public int? FavoriteLanguageId { get; set; }
        public ProgrammingLanguage FavoriteLanguage { get; set; }
        public ICollection<FriendPhoneNumber> PhoneNumbers { get; set; }
    }
}