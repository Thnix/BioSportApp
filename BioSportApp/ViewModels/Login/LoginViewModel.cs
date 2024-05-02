using CommunityToolkit.Mvvm.ComponentModel;

namespace BioSportApp.ViewModels.Login
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        public string? email = null;

        [ObservableProperty]
        public string? password = null;
    }
}
