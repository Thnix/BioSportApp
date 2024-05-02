using BioSportApp.Pages.Login;
using BioSportApp.Pages.User;

namespace BioSportApp;

public partial class AutenticationShell : Shell
{
	public AutenticationShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("UserCreatePage", typeof(UserCreatePage));
        Routing.RegisterRoute("LoginPage", typeof(LoginPage));
    }
}