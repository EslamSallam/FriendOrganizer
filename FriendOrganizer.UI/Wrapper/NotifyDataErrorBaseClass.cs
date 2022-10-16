using FriendOrganizer.UI.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FriendOrganizer.UI.Wrapper
{

    public class NotifyDataErrorBaseClass : ViewModelBase, INotifyDataErrorInfo
    {
        public string UIFirstError
        {
            get
            {
                if (_errorsPropertyName != null && _errorsPropertyName.Count > 0)
                {
                    string errorMessage = "";
                    foreach(var error in _errorsPropertyName)
                    {
                        foreach(var e in error.Value)
                        {
                            errorMessage += e + Environment.NewLine;
                        }
                    }
                    return errorMessage;
                }
                else
                    return string.Empty;
            }
        }
        
        private Dictionary<string, List<string>> _errorsPropertyName = new Dictionary<string, List<string>>();
        public bool HasErrors => _errorsPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (_errorsPropertyName.ContainsKey(propertyName))
            {
                return _errorsPropertyName[propertyName];
            }
            return Enumerable.Empty<DataErrorsChangedEventArgs>();
        }

        private void onErrorChanged(string? propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }

        protected void AddError(string propertyName, string error)
        {
            if (!_errorsPropertyName.ContainsKey(propertyName))
            {
                _errorsPropertyName[propertyName] = new List<string>();
            }
            if (!_errorsPropertyName[propertyName].Contains(error))
            {
                _errorsPropertyName[propertyName].Add(error);
                onErrorChanged(propertyName);
            }


        }

        protected void ClearErrors(string propertyName)
        {
            if (_errorsPropertyName.ContainsKey(propertyName))
            {
                _errorsPropertyName.Remove(propertyName);
                onErrorChanged(propertyName);
            }
        }
    }
}
