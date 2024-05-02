using BioSportApp.Models.PopUp;
using CommunityToolkit.Mvvm.ComponentModel;
using BioSportApp.Common;
using CommunityToolkit.Mvvm.Messaging;
using BioSportApp.Messenger.CloseMessage;
using CommunityToolkit.Mvvm.Input;

namespace BioSportApp.ViewModels.PopUp
{
    public partial class MessagePopUpViewModel : ObservableObject
    {
        [ObservableProperty]
        public Color backgroundColor = new();

        [ObservableProperty]
        public string icon = new("");

        [ObservableProperty]
        public string statusText = new("");

        [ObservableProperty]
        public string? message = null;

        [ObservableProperty]
        public bool btnVisible = new();

        public async void ShowMessage(MessageDataModel messageData)
        {
            BtnVisible = !messageData.IsValid;
            Message = messageData.Message;

            if (messageData.IsValid)
            {
                BackgroundColor = Color.FromRgb(252, 188, 92);
                Icon = Icons.CircleCheck;
                StatusText = "ÉXITO";

                await Task.Delay(1500);
                WeakReferenceMessenger.Default.Send(new CloseMessagePopUp(true));
            }
            else
            {
                BackgroundColor = Color.FromRgb(217, 29, 37);
                Icon = Icons.CircleXmark;
                StatusText = "ERROR";
            }
        }

        /// <summary>
        /// ClosePopUp
        /// </summary>
        [RelayCommand]
        public async void ClosePopUp()
        {
            WeakReferenceMessenger.Default.Send(new CloseMessagePopUp(true));
        }
    }
}
