using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp
{
    public class WelcomeDataModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _mobileNumber;
        private string _errorMessageMobileNumber;
        private WelcomeValidator _validator;
        private bool _isEnable;

        public WelcomeDataModel()
        {
            _validator = new WelcomeValidator();
            IsEnable = false;
        }

        public string MobileNumber
        {
            get
            {
                return _mobileNumber;
            }
            set
            {
                _mobileNumber = value;
                var result = _validator.Validate(this);
                IsEnable = result.IsValid;
                ErrorMessageMobileNumber = _validator.GetErrorMessage();
            }
        }
        public string ErrorMessageMobileNumber { get { return _errorMessageMobileNumber; } set { _errorMessageMobileNumber = value; OnPropertyChanged(); } }
        public bool IsEnable { get { return _isEnable; } set { _isEnable = value; OnPropertyChanged(); } }
        public bool ValidateAll()
        {
            var result = _validator.Validate(this);
            ErrorMessageMobileNumber = _validator.GetAllErrorMessage();
            return result.IsValid;
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
