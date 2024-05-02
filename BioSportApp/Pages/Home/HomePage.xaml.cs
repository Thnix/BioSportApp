using BioSportApp.ViewModels.HomePage;

namespace BioSportApp.Pages.Home;

public partial class HomePage : ContentPage
{
	public HomePage(HomePageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}