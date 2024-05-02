using BioSportApp.ViewModels.User;
using System.Diagnostics;

namespace BioSportApp.Pages.User;

public partial class UserCreatePage : ContentPage
{
	public UserCreatePage(UserCreatePageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}