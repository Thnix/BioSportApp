using BioSportApp.Domain.Core;
using BioSportApp.Models.PopUp;
using BioSportApp.Models.User;
using BioSportApp.Services;
using BioSportApp.Services.Interfaces;
using BioSportApp.ViewModels.PopUp;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mapster;
using System.Text;
using System.Text.RegularExpressions;
using Stream = System.IO.Stream;

namespace BioSportApp.ViewModels.User
{
    public partial class UserCreatePageViewModel : ObservableObject
    {
        private readonly IDialogService dialogService;
        private readonly PopupService popupService;
        private readonly UserService userService;
        public UserCreatePageViewModel(IDialogService dialogService, PopupService popupService, UserService userService)
        {
            this.dialogService = dialogService;
            this.popupService = popupService;
            this.userService = userService;

            Image = ImageSource.FromResource("BioSportApp.Resources.Images.photo.png", typeof(UserCreatePageViewModel).Assembly);
        }

        //Observable Properties

        [ObservableProperty]
        public UserViewModel user = new();

        [ObservableProperty]
        public ImageSource? image;

        [ObservableProperty]
        public DateTime maxDate = DateTime.Now;


        //Methods

        /// <summary>
        /// CreateUser
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task CreateUser(UserViewModel user)
        {
            var messageData = ValidateFormData();

            if (messageData.IsValid)
            {
                var response = await userService.CreateUser(user.Adapt<UserAddModel>());

                messageData.IsValid = response.IsValid;
                messageData.Message = response.Message;

                if(response.IsValid && response.Data != null)
                {
                    await ShowMessage(messageData);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage = new BioSportApp.AppShell();
                    });

                    return;
                }
            }

            await ShowMessage(messageData);
        }

        /// <summary>
        /// ShowMessage
        /// </summary>
        /// <param name="messageData"></param>
        /// <returns></returns>
        private async Task ShowMessage(MessageDataModel messageData)
        {
            await popupService.ShowPopupAsync<MessagePopUpViewModel>(vm => vm.ShowMessage(messageData));
        }


        /// <summary>
        /// ValidateFormData
        /// </summary>
        /// <returns></returns>
        private MessageDataModel ValidateFormData()
        {
            var messageData = new MessageDataModel
            {
                IsValid = true
            };

            var stringBuilder = new StringBuilder();


            if (string.IsNullOrEmpty(User.Name))
            {
                stringBuilder.AppendLine("El nombre es requerido.");
                messageData.IsValid = false;
            }

            if (string.IsNullOrEmpty(User.FirstSurname))
            {
                stringBuilder.AppendLine("El apellido es requerido.");
                messageData.IsValid = false;
            }

            if (string.IsNullOrEmpty(User.SecondSurname))
            {
                stringBuilder.AppendLine("El segundo apellido es requerido.");
                messageData.IsValid = false;
            }

            if (string.IsNullOrEmpty(User.Nip))
            {
                stringBuilder.AppendLine("La cédula es requerida.");
                messageData.IsValid = false;
            }

            if (string.IsNullOrEmpty(User.Email))
            {
                stringBuilder.AppendLine("El correo es requerido.");
                messageData.IsValid = false;
            }
            else
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new(pattern, RegexOptions.IgnoreCase);

                if (!regex.IsMatch(User.Email))
                {
                    stringBuilder.AppendLine("El formato del correo no es válido.");
                    messageData.IsValid = false;
                }
            }

            if (string.IsNullOrEmpty(User.Code))
            {
                stringBuilder.AppendLine("El código es requerido.");
                messageData.IsValid = false;
            }

            if (string.IsNullOrEmpty(User.BirthdayDate.ToString()))
            {
                stringBuilder.AppendLine("La fecha de nacimiento es requerida.");
                messageData.IsValid = false;
            }

            messageData.Message = stringBuilder.ToString();

            return messageData;
        }

        /// <summary>
        /// ShowActions
        /// </summary>
        [RelayCommand]
        public async void ShowActions()
        {
            string action = await dialogService.DisplayActionSheet("Imagen de perfil", "Cancelar", null, "Tomar", "Galería");

            switch(action)
            {
                case "Tomar":
                    await TakePhoto();
                    break;

                case "Galería":
                    await GetImage();
                    break;

                default:
                    break;
            } 
        }

        /// <summary>
        /// GetImage
        /// </summary>
        /// <returns></returns>
        private async Task GetImage()
        {
            var photo = await MediaPicker.PickPhotoAsync();

            if (photo != null)
            {
                using var memoryStream = await photo.OpenReadAsync();

                User.Photo = ConvertStreamToByteArray(memoryStream);

                if (User.Photo != null)
                {
                    Image = ImageSource.FromStream(() => new MemoryStream(User.Photo));
                }
            }
        }

        /// <summary>
        /// TakePhoto
        /// </summary>
        /// <returns></returns>
        private async Task TakePhoto()
        {
            var photo = await MediaPicker.CapturePhotoAsync();

            if (photo != null)
            {
                using var memoryStream = await photo.OpenReadAsync();
                User.Photo = ConvertStreamToByteArray(memoryStream);

                if (User.Photo != null)
                {
                    Image = ImageSource.FromStream(() => new MemoryStream(User.Photo));
                }
            }
        }

        /// <summary>
        /// ConvertStreamToByteArray
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private byte[]? ConvertStreamToByteArray(Stream stream)
        {
            if (stream == null)
                return null;

            using var ms = new MemoryStream();
            stream.CopyTo(ms);

            return ms.ToArray();
        }
    }
}
