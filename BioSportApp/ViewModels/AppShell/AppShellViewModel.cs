using BioSportApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BioSportApp.ViewModels.AppShell
{
    public partial class AppShellViewModel : ObservableObject
    {
        private readonly SessionService sessionService;
        
        public AppShellViewModel(SessionService sessionService)
        {
            this.sessionService = sessionService;

        }

        [RelayCommand]
        public void Logout()
        {
            sessionService.Logout();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = new AutenticationShell();
            });
        }
    }
}
