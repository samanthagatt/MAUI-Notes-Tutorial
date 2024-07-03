namespace MauiNotesTutorial.Views;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
	}

	private async void LearnMore_Clicked(object sender, EventArgs e)
	{
		if (BindingContext is Models.AboutAppInfo about)
		{
			// Navigate to the specified URL in the system browser.
			await Launcher.Default.OpenAsync(about.MoreInfoUrl);
		}
	}
}