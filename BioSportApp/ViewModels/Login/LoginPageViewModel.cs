using BioSportApp.Models.Login;
using BioSportApp.Models.PopUp;
using BioSportApp.Pages.User;
using BioSportApp.Services;
using BioSportApp.ViewModels.PopUp;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mapster;
using System.Text.RegularExpressions;
using System.Text;
using BioSportApp.Common;

namespace BioSportApp.ViewModels.Login
{
    public partial class LoginPageViewModel(UserService userService, IServiceProvider serviceProvider, LoginService loginService, IPopupService popupService) : ObservableObject
    {
        private readonly UserService userService = userService;
        private readonly LoginService loginService = loginService;
        private readonly IServiceProvider serviceProvider = serviceProvider;
        private readonly IPopupService popupService = popupService;

        //Observable properties
        [ObservableProperty]
        public LoginViewModel userData = new();

        [ObservableProperty]
        public bool isPassword = true;

        [ObservableProperty]
        public string passwordIcon = Icons.Eye;


        //Methods


        [RelayCommand]
        public void ChangePasswordVisibility()
        {
            IsPassword = !IsPassword;
            PasswordIcon = IsPassword ? Icons.Eye : Icons.EyeSlash;
        }

        [RelayCommand]
        public async Task AutenticateUser(LoginViewModel userToAutenticate)
        {
            MessageDataModel messageData = ValidateLoginData();

            if (!messageData.IsValid)
            {
                await ShowMessage(messageData);
                return;
            }

            var response = await loginService
                .AutenticateUser(userToAutenticate.Adapt<LoginModel>());

            messageData.IsValid = response.IsValid;
            messageData.Message = response.Message;

            if (!response.IsValid)
            {
                await ShowMessage(messageData);
                return;
            }


            MainThread.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = new BioSportApp.AppShell();
            });
        }


        /// <summary>
        /// ValidateFormData
        /// </summary>
        /// <returns></returns>
        private MessageDataModel ValidateLoginData()
        {
            var messageData = new MessageDataModel
            {
                IsValid = true
            };

            var stringBuilder = new StringBuilder();


            if (string.IsNullOrEmpty(UserData.Email))
            {
                stringBuilder.AppendLine("El correo es requerido.");
                messageData.IsValid = false;
            }
            else
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new(pattern, RegexOptions.IgnoreCase);

                if (!regex.IsMatch(UserData.Email))
                {
                    stringBuilder.AppendLine("El formato del correo no es válido.");
                    messageData.IsValid = false;
                }
            }

            if (string.IsNullOrEmpty(UserData.Password))
            {
                stringBuilder.AppendLine("La contraseña es requerida.");
                messageData.IsValid = false;
            }

            messageData.Message = stringBuilder.ToString();

            return messageData;
        }




        //Navigation
        [RelayCommand]
        public async Task GoToUserCreatePage()
        {
            await Shell.Current.GoToAsync(nameof(UserCreatePage));


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


    }
}
