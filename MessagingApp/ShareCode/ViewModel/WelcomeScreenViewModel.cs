using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MessagingApp.View;
using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace MessagingApp
{
    public partial class WelcomeScreenViewModel : BaseViewModel
    {

        [ObservableProperty]
        private string _welcomeText;
        [ObservableProperty]
        private string _welcomeTitle;
        [ObservableProperty] 
        private bool _isWelcomeTitleVisible;
        [ObservableProperty]
        private bool _isVerifyOTPScreen;
        [ObservableProperty]
        private string _buttonText;
        private string _mobileNumber;
        [ObservableProperty]
        private bool _isRegisterScreen;
        [ObservableProperty]
        private string _sentToText; 
        [ObservableProperty]
        private string _OTPDigit1;
        [ObservableProperty]
        private string _OTPDigit2;
        [ObservableProperty]
        private string _OTPDigit3;
        [ObservableProperty]
        private string _OTPDigit4;
        [ObservableProperty]
        private string _OTPDigit5;
        [ObservableProperty]
        private string _OTPDigit6;
        [ObservableProperty]
        private TimeSpan _timerText;
        [ObservableProperty]
        private Color _resendTextColor;
        [ObservableProperty]
        private bool _isResendTextEnabled;
        private Timer _timer;
        private UserModel _userModel;
        private FirebaseDatabaseService _firebaseDatabaseService;
        private int seconds;
        [ObservableProperty]
        private WelcomeDataModel _welcomeDataModel;

        [ObservableProperty]
        private IAuthenticationServices _authenticationServices;

        public WelcomeScreenViewModel()
        {
            _welcomeDataModel = new WelcomeDataModel();
            _userModel = new UserModel();
            _firebaseDatabaseService = new FirebaseDatabaseService();           
            SetUpRegisterScreen();
           
        }

        private void SetUpRegisterScreen()
        {

            IsRegisterScreen = true;
            WelcomeText = "Welcome";
            WelcomeTitle = "Use your personal information to get register";
            IsWelcomeTitleVisible = true;
            ButtonText = "Continue";
            IsVerifyOTPScreen = false;           
        }

        [RelayCommand]
        public async Task GetOTP()
        {
           
            if (!IsVerifyOTPScreen)
            {
                var IsValid = WelcomeDataModel.ValidateAll();
                _mobileNumber = WelcomeDataModel.MobileNumber;
               
                if (IsValid)
                {
                  
                    var responseIsValid = await AuthenticationServices.AuthenticateMobile(_mobileNumber);
                    if (responseIsValid)
                    {
                        SetUpOTPScreen();
                        _userModel.MobileNumber = _mobileNumber;
                        await _firebaseDatabaseService.AddUser<UserModel>(_userModel);
                        await Console.Out.WriteLineAsync("Data Inserted");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Oops", "Something went wrong try after few times.", "OK");
                    }

                }
                else
                {
                    
                    await Application.Current.MainPage.DisplayAlert("Oops",WelcomeDataModel.ErrorMessageMobileNumber, "OK");
                }
            }
            else
            {
                _=GoToUserProfileScreen();                
            }
        }

       

        [RelayCommand]
        public async Task ChangeNumber()
        {
            var userDetails= await _firebaseDatabaseService.GetUser(_mobileNumber);
          
            
            string result = await Application.Current.MainPage.DisplayPromptAsync("Change Number", "Do you want to change your mobile number?","Confirm","Cancel",placeholder:"Enter your mobile number",maxLength: 10,keyboard: Keyboard.Numeric);
            if(result != null)
            {
                _mobileNumber = "+91"+result;                
                var responseIsValid = await AuthenticationServices.AuthenticateMobile(_mobileNumber);
                if (responseIsValid)
                {
                    SetUpOTPScreen();
                    _userModel.Id = userDetails.Id;
                    _userModel.MobileNumber = _mobileNumber;
                    await _firebaseDatabaseService.Update<UserModel>(_userModel.Id, _userModel);
                    await Console.Out.WriteLineAsync("Updated Successfully");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Oops", "Something went wrong try after few times.", "OK");
                    ResendTextColor = Colors.DarkGray;
                    IsResendTextEnabled = false;
                    SetTimer();
                }
            }
        }
        [RelayCommand]
        public async Task ResendCode()
        {
            _mobileNumber = WelcomeDataModel.MobileNumber;
            
                var responseIsValid = await AuthenticationServices.AuthenticateMobile(_mobileNumber);
                if (responseIsValid)
                {
                   await Application.Current.MainPage.DisplayAlert("OTP Send", "OTP has been sent to your mobile number", "OK");

                }
                else
                {
                  await Application.Current.MainPage.DisplayAlert("Oops", "Something went wrong try after few times.", "OK");
                }
                ResendTextColor = Colors.DarkGray;
                IsResendTextEnabled = false;
                SetTimer();
        }

        private async Task GoToUserProfileScreen()
        {
            var smsCode = GetOTPDigits();
            if (smsCode != null)
            {
                var responseIsValid = await AuthenticationServices.VerifyOTP(smsCode);
                if (responseIsValid)
                {
                   
                    await Application.Current.MainPage.Navigation.PushAsync(new UserProfileScreen(_mobileNumber));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Oops", "Enter valid OTP", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Oops", "Enter OTP", "OK");
            }
           
        }

        private string GetOTPDigits()
        {
            if (!(string.IsNullOrEmpty(OTPDigit1))&&!(string.IsNullOrEmpty(OTPDigit2))&&!(string.IsNullOrEmpty(OTPDigit3))
                && !(string.IsNullOrEmpty(OTPDigit4)) && !(string.IsNullOrEmpty(OTPDigit5)) && !(string.IsNullOrEmpty(OTPDigit6)))
                return OTPDigit1 + OTPDigit2 + OTPDigit3 + OTPDigit4 + OTPDigit5 + OTPDigit6;
            else 
                return null;
        }

        private void SetUpOTPScreen()
        {
            IsVerifyOTPScreen = true;
            IsRegisterScreen = false;
            WelcomeText = "What's the Code?";
            SentToText = "Enter OTP sent to " + _mobileNumber;
            WelcomeTitle = string.Empty;
            IsWelcomeTitleVisible = false;
            ButtonText = "Verify";
            ResendTextColor = Colors.DarkGray;
            IsResendTextEnabled = false;
            SetTimer();
        }

        private void SetTimer()
        {
           _timer = new Timer();
            _timer.Interval = 1000;
            seconds = 0;
           _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            seconds++;
            TimerText = TimeSpan.FromSeconds(seconds);
            if (seconds >= 60)
            {
                IsResendTextEnabled = true;
                ResendTextColor = Colors.DarkSlateBlue;
                _timer.Stop(); 
                seconds = 0; 
                TimerText = TimeSpan.FromSeconds(seconds);
            }
            else
            {
                TimerText = TimeSpan.FromSeconds(seconds);
            }
         }
    }
}
