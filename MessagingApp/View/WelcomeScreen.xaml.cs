namespace MessagingApp;

public partial class WelcomeScreen : ContentPage
{
    private readonly IAuthenticationServices _authenticationServices;
    private WelcomeScreenViewModel _welcomeViewModel;
    private List<Entry> _entryList;
    private Entry _currentEntry;
    private Entry _nextEntry;
    private Entry _previousEntry;
    private Frame _frame;
    public WelcomeScreen(IAuthenticationServices authenticationServices)
	{      
      InitializeComponent();
      
        _welcomeViewModel = (WelcomeScreenViewModel)BindingContext;
        _authenticationServices = authenticationServices;
        _welcomeViewModel.AuthenticationServices = _authenticationServices;
    }
    protected override bool OnBackButtonPressed()
    {
        if (_welcomeViewModel.IsVerifyOTPScreen)
        {
            return true;
        }
        else
            return false;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _welcomeViewModel.IsRegisterScreen = true;
        _welcomeViewModel.IsVerifyOTPScreen = false;
    }

    private void OtpDigit_TextChanged(object sender, TextChangedEventArgs e)
    {
        _entryList = new List<Entry> { OtpDigit1, OtpDigit2, OtpDigit3, OtpDigit4, OtpDigit5, OtpDigit6 };
        _currentEntry = (Entry)sender;
        _nextEntry = GetNextEntry(_currentEntry);
        _previousEntry = GetPreviousEntry(_currentEntry);

        _frame = GetParentEntryFrame(_currentEntry);

        if (string.IsNullOrEmpty(_currentEntry.Text))
        {
            VisualStateManager.GoToState(_frame, "Empty");
        }
        else
        {
            VisualStateManager.GoToState(_frame, "Filled");
        }

        if (_currentEntry.Text.Length == 1 && _currentEntry != null)
        {
            _nextEntry.Focus();
        }
        else
        {
            _previousEntry.Focus();
        }


    }
    private Frame GetParentEntryFrame(Entry currentEntry)
    {
        if (currentEntry == OtpDigit1)
        {
            return frame1;
        }
        else if (currentEntry == OtpDigit2)
        {
            return frame2;
        }
        else if (currentEntry == OtpDigit3)
        {
            return frame3;
        }
        else if (currentEntry == OtpDigit4)
        {
            return frame4;
        }
        else if (currentEntry == OtpDigit5)
        {
            return frame5;
        }
        else if (currentEntry == OtpDigit6)
        {
            return frame6;
        }
        return null;
    }

    private Entry GetPreviousEntry(Entry currentEntry)
    {

        var currentIndex = _entryList.IndexOf(currentEntry);
        if (currentIndex == 0)
        {
            return currentEntry;
        }
        return _entryList[currentIndex - 1];
    }

    private Entry GetNextEntry(Entry currentEntry)
    {

        var currentIndex = _entryList.IndexOf(currentEntry);
        var nextIndex = currentIndex + 1;

        if (nextIndex < _entryList.Count)
        {
            return _entryList[nextIndex];
        }

        return _entryList[_entryList.Count - 1];
    }

}


