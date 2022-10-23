using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.Model
{
    public class LookupItem
    {
        public int Id { get; set; }
        public string? DisplayMember { get; set; }
    }
    public class FriendProgrammingLanguages
    {
        public int Id { get; set; }
        public int FriendId { get; set; }
        public Friend Friend { get; set; }
        public string? DisplayMember { get; set; }
        public int ProgrammingLanguageId { get; set; }
        public ProgrammingLanguage ProgrammingLanguage { get; set; }
        public string ProgrammingLanguageName { get; set; }    
        public bool IsChecked { get; set; }
    }
    public class NullLookupItem : LookupItem
    {
        public new int? Id { get; set; }
    }
}
