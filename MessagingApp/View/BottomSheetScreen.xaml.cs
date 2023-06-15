namespace MessagingApp.View;

public partial class BottomSheetScreen : ContentPage
{
    public event EventHandler<string> BottomSheetTextTapped;
	public BottomSheetScreen()
	{
		InitializeComponent();
	}
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		var label = (sender) as Label;
        if (label.Text=="Camera")
        {
            BottomSheetTextTapped?.Invoke(this, "camera");
        }
        else if(label.Text=="Gallery")
        {
            BottomSheetTextTapped?.Invoke(this,"gallery" );
        }
        else if (label.Text == "Remove")
        {
            BottomSheetTextTapped?.Invoke(this, "delete");
        }
        else if(label.Text=="Cancel")
        {
            BottomSheetTextTapped?.Invoke(this,"cancel" );
        }
    }
   
}