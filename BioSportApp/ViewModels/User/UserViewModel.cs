using CommunityToolkit.Mvvm.ComponentModel;

namespace BioSportApp.ViewModels.User
{
    public partial class UserViewModel : ObservableObject
    {
        [ObservableProperty]
        public string? name = null;

        [ObservableProperty]
        public string? firstSurname = null;

        [ObservableProperty]
        public string? secondSurname = null;

        [ObservableProperty]
        public string? nip = null;

        [ObservableProperty]
        public string? email = null;

        [ObservableProperty]
        public string? password = null;

        [ObservableProperty]
        public string? code = null;

        [ObservableProperty]
        public DateTime? birthdayDate = null;

        [ObservableProperty]
        public byte[]? photo = null;
    }
}
