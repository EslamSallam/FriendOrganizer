using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace FriendOrganizer.UI.Wrapper
{
	public class FriendWrapper : ModelWrapper<Friend>
	{
		public FriendWrapper(Friend model) : base(model)
		{
		}

        public int Id { get { return Model.Id; } }

		public string FirstName
		{
			get { return GetValue<string>(); }
			set { SetValue<string>(value); }
		}

		public string? LastName
		{
			get { return GetValue<string>(); }
			set { SetValue<string>(value); }
		}

		public string? Email
		{
			get { return GetValue<string>(); }
			set { SetValue<string>(value); }
		}
        public int? FavoriteLanguageId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

		public ICollection<FriendPhoneNumber> PhoneNumbers { set;get; }
        protected override IEnumerable<string> validateProperty(string propertyName)
		{
			switch (propertyName)
			{
				case nameof(FirstName):
					if (FirstName.Length <= 5)
					{
						yield return $"FirstName Should be Greater than 5 chars.";
                    }
                    break;
                case nameof(LastName):
                    if (LastName.Length <= 5)
                    {
                        yield return $"LastName Should be Greater than 5 chars.";
                    }
                    break;
                default:
                        break;
					
			}
            OnPropertyChanged("UIFirstError");
		}
    }
}
