using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessagingApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MessagingApp
{
    public partial class UserProfileScreenViewModel : BaseViewModel
    {
        [ObservableProperty]
        private DateTime _maxDate;
        [ObservableProperty]
        private bool _isSheetVisible;
        [ObservableProperty]
        private bool _isEditEnable;
        [ObservableProperty]
        private string _selectedTextBottomSheet;
        [ObservableProperty]
        private ImageSource _imageUserProfilePhoto;
        [ObservableProperty]
        private DateTime _dOB;
        [ObservableProperty]
        private string _userMobileNumber;
        [ObservableProperty]
        private string _userBio;
        [ObservableProperty]
        private UserProfileModel _profile;

        public UserProfileScreenViewModel()
        {
            _profile = new UserProfileModel();
            MaxDate = DateTime.Today;
            DOB = DateTime.Today;
            IsEditEnable = true;

        }
        [RelayCommand]
        public void ChooseProfilePhoto()
        {
            IsEditEnable = false;
            Profile.IsImageTap = true;
            Profile.SelectedText = SelectedTextBottomSheet;
            Profile.IsEditEnabled = IsEditEnable;
            Profile.ShowBottomSheet();
            IsSheetVisible = Profile.IsBottomSheetVisible;
            IsEditEnable = Profile.IsEditEnabled;
            SelectedTextBottomSheet = string.Empty;
        }
        [RelayCommand]
        public async Task LoadNextPage()
        {

            Profile.DateOfBirth = DOB;
            Profile.UserBio = UserBio;           
            Profile.MobileNumber = UserMobileNumber;
            var result = await Profile.LoadNextPageAsync();
           if(!result.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(result.Title, result.Message, "Ok");
            }
            else
            {
                await App.Current.MainPage.Navigation.PushAsync(new UserDashboard());
            }
        }

    }
}
