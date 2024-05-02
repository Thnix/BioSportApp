using BioSportApp.ViewModels.Login;

namespace BioSportApp.Pages.Login;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}