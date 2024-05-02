using CommunityToolkit.Mvvm.ComponentModel;
using BioSportApp.Common;
using BioSportApp.Services;
using BioSportApp.ViewModels.User;
using Mapster;

namespace BioSportApp.ViewModels.HomePage
{
    public partial class HomePageViewModel : ObservableObject
    {
        public HomePageViewModel(SessionService sessionService)
        {
            currentUser = sessionService.CurrentUser.Adapt<UserViewModel>();

            SetWelcomeMessage();
        }

        //Observable Properties

        [ObservableProperty]
        public string welcomeMessage = new("");

        [ObservableProperty]
        public string welcomeIcon = new("");

        [ObservableProperty]
        public string currentDate = new("");

        [ObservableProperty]
        public UserViewModel currentUser = new();

        //Methods

        /// <summary>
        /// SetWelcomeMessage
        /// </summary>
        private void SetWelcomeMessage()
        {
            CurrentDate = DateTime.Now.ToShortTimeString();

            var hour = DateTime.Now.Hour;

            if (hour < 12)
            {
                WelcomeMessage = $"¡Buenos días! {CurrentUser.Name}";
                WelcomeIcon = Icons.Sun;
            }
            else if (hour < 19)
            {
                WelcomeMessage = $"¡Buenas tardes! {CurrentUser.Name}";
                WelcomeIcon = Icons.MugHot;
            }
            else
            {
                WelcomeMessage = $"¡Buenas noches! {CurrentUser.Name}";
                WelcomeIcon = Icons.Moon;
            }
        }
    }
}
