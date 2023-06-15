using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp
{
    public class UserProfileDataModel : INotifyPropertyChanged
    {
        private string _userName;
        private string _errorMessageUserName;
        private string _errorMessageDateOfBirth;
        private bool _isEnabled;
        private UserProfileValidator _validator;
        private string _errorMessage;
        public UserProfileDataModel()
        {
            _validator = new UserProfileValidator();
            IsEnabled = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public string UserName 
        { 
            get { return _userName; }
            set { 
                    _userName = value;
                    var result = _validator.Validate(this);
                    IsEnabled = result.IsValid;
                    ErrorMessageUserName = _validator.GetErrorMessage();
                } 
        }

        public string ErrorMessageUserName { get { return _errorMessageUserName; } set { _errorMessageUserName = value;OnPropertyChanged(); } }

        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; OnPropertyChanged(); } }
        public string ErrorMessage { get { return _errorMessage; } set { _errorMessage = value;OnPropertyChanged(); } }
        public bool ValidateAll()
        {
            var result = _validator.Validate(this);
            ErrorMessage = _validator.GetAllErrorMessage();
            return result.IsValid;
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
