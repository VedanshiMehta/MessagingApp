using CommunityToolkit.Mvvm.ComponentModel;
using MessagingApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MessagingApp
{
    public partial class UserProfileModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isBottomSheetVisible;
        [ObservableProperty]
        private bool _isImageTap;
        [ObservableProperty]
        private string _selectedText;
        [ObservableProperty]
        private bool _isEditEnabled;
        [ObservableProperty]
        private ImageSource _imageUserProfile;
        [ObservableProperty]
        private UserProfileDataModel _userProfileDataModel;
        [ObservableProperty]
        private DateTime _dateOfBirth;
        [ObservableProperty]
        private string _mobileNumber;
        [ObservableProperty]
        private string _userBio;
        private string _userName;
        private string _base64profile;
        private double _maxLengthFile = 3.0;
        private FirebaseDatabaseService _firebaseDatabaseService;
        private UserModel _userModel;
        private bool _isProfileRemoved;

        public UserProfileModel()
        {
            SetDefaultImage();
            _userProfileDataModel = new UserProfileDataModel();
            _userName = _userProfileDataModel.UserName;
            _firebaseDatabaseService = new FirebaseDatabaseService();
            _userModel = new UserModel();
        }

        

        private void SetDefaultImage()
        {
            _base64profile = Common.Base64DefaultImage;
            ImageUserProfile = Converter.ConvertBase64ToImage(_base64profile);
        }
        public  async Task<Result> LoadNextPageAsync()
        { 
           
             var isValid = UserProfileDataModel.ValidateAll();
            if (isValid)
            {
                var userDetails = await _firebaseDatabaseService.GetUser(MobileNumber);
                if (userDetails != null)
                {
                    _userModel.Id = userDetails.Id;
                    _userModel.Name = _userName;
                    _userModel.ProfileImage = _base64profile;
                    _userModel.MobileNumber = MobileNumber;
                    _userModel.DateOfBirth = DateOfBirth;
                    _userModel.IsProfileImageRemoved = _isProfileRemoved;
                    if(UserBio==null)
                    {
                        UserBio = "Hey there!!! What's Up";
                        _userModel.Bio = UserBio;
                    }
                    else
                    {
                        _userModel.Bio = UserBio;
                    }
                    await _firebaseDatabaseService.Update<UserModel>(_userModel.Id, _userModel);
                    await Console.Out.WriteLineAsync("Updated Successfully");
                    return new Result()
                    { IsSuccess = true };
                }
                else
                {
                    return new Result()
                    {
                        IsSuccess = false,
                        Message = "User not found",
                        Title = "Oops"
                    };
                }
               
            }
            else
            {
                return new Result()
                {
                    IsSuccess = false,
                    Message = UserProfileDataModel.ErrorMessage,
                    Title = "Oops"
                };
            }
        }
        public void ShowBottomSheet()
        {
            if (IsImageTap)
            {

                IsBottomSheetVisible = true;
                SelectImages();

            }
            else
            {
                IsBottomSheetVisible = false;
                IsEditEnabled = true;
            }
        }

        private void SelectImages()
        {
            if (SelectedText == "camera")
            {
                TakePhoto();
                IsBottomSheetVisible = false;
                IsEditEnabled = true;
            }
            else if (SelectedText == "gallery")
            {
                SelectPhoto();
                IsBottomSheetVisible = false;
                IsEditEnabled = true;
            }
            else if (SelectedText == "delete")
            {
                RemovePhoto();
                IsBottomSheetVisible = false;
                IsEditEnabled = true;
            }
            else if (SelectedText == "cancel")
            {
                IsBottomSheetVisible = false;
                IsEditEnabled = true;
            }

        }

        private void RemovePhoto()
        {
            SetDefaultImage();
            _isProfileRemoved = true;
        }

        private async void SelectPhoto()
        {
           
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null)
                {
                    if (photo.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) || photo.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        photo.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase))
                    {

                        using Stream sourceStream = await photo.OpenReadAsync();

                        var bytes = new byte[sourceStream.Length];
                         await sourceStream.ReadAsync(bytes, 0, (int)sourceStream.Length);
                        _base64profile = System.Convert.ToBase64String(bytes);

                        var length = bytes.Length;
                        var mb = length / 1048576.0;

                        if (mb <= _maxLengthFile)
                        {
                            ImageUserProfile = Converter.ConvertBase64ToImage(_base64profile);
                        _isProfileRemoved = false;

                    }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Oops", "Selected file is larger than 3MB.", "OK");
                        }

                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Oops", "Selected file is invalid.", "OK");
                    }

                }
            
        }

        private async void TakePhoto()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    if (photo.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) || photo.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        photo.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase))
                    {

                        using Stream sourceStream = await photo.OpenReadAsync();
                        var bytes = new byte[sourceStream.Length];
                        await sourceStream.ReadAsync(bytes, 0, (int)sourceStream.Length);
                        _base64profile = System.Convert.ToBase64String(bytes);
                        var length = bytes.Length;
                        var mb = length / 1048576.0;

                        if (mb <= _maxLengthFile)
                        {
                            ImageUserProfile = Converter.ConvertBase64ToImage(_base64profile);

                            _isProfileRemoved = true;
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Oops", "Selected file is larger than 3MB.", "OK");
                        }

                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Oops", "Selected file is invalid.", "OK");
                    }

                }
            }
        }
    }
}
