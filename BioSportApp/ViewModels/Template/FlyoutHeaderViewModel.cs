using BioSportApp.Services;
using BioSportApp.ViewModels.User;
using CommunityToolkit.Mvvm.ComponentModel;
using Mapster;

namespace BioSportApp.ViewModels.Template
{
    public partial class FlyoutHeaderViewModel : ObservableObject
    {
        public FlyoutHeaderViewModel(SessionService sessionService)
        {
            currentUser = sessionService.CurrentUser.Adapt<UserViewModel>();

            if (currentUser.Photo != null)
            {
                userImage = currentUser.Photo;
            }
        }

        [ObservableProperty]
        public UserViewModel currentUser = new();

        [ObservableProperty]
        public byte[] userImage = [];
    }
}
