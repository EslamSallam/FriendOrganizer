using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Wrapper
{
    public class MeetingWrapper : ModelWrapper<Meeting>
    {
        public MeetingWrapper(Meeting model) : base(model)
        {
        }
        public int Id { get { return Model.Id; } }

        public string Title
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public DateTime DateFrom
        {
            get
            { return Model.DateFrom != new DateTime() ? Model.DateFrom : new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); }
            set
            { 
                SetValue(value);
                if (DateTo < DateFrom)
                {
                    DateTo = DateFrom;
                }
            }
        }

        public DateTime DateTo
        {
            get
            { return Model.DateTo != new DateTime() ? Model.DateTo : new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); }
            set { 
                SetValue(value);
                if (DateTo < DateFrom)
                {
                    DateFrom = DateTo;
                }
            }
        }

    }
}
