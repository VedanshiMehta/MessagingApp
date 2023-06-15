namespace MessagingApp.View;

public partial class UserProfileScreen : ContentPage
{
    private UserProfileScreenViewModel _viewModel;
    private string _selectedText;
	public UserProfileScreen(string _mobileNumber)
	{       
        InitializeComponent(); 
        _viewModel = (UserProfileScreenViewModel)BindingContext;
        _viewModel.UserMobileNumber = _mobileNumber;
        BottomSheetContainer.BottomSheetTextTapped += BottomSheetContainer_BottomSheetTextTapped;
	}

    private void BottomSheetContainer_BottomSheetTextTapped(object sender, string e)
    {
        _selectedText = e;
        _viewModel.SelectedTextBottomSheet = _selectedText;
        _viewModel.ChooseProfilePhoto();
    }


    protected override bool OnBackButtonPressed()
    {
        return true;
    }



}