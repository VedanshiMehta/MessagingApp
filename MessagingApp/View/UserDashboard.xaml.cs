namespace MessagingApp.View;

public partial class UserDashboard : ContentPage
{
	public UserDashboard()
	{
		InitializeComponent();
	}
    protected override bool OnBackButtonPressed()
    {
        NavigatedToRoot();
        return base.OnBackButtonPressed();

    }

    private async void NavigatedToRoot()
    {
        await Navigation.PopToRootAsync();
    }
}