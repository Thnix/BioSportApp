using BioSportApp.Common.Messaging;
using BioSportApp.Utils.Messaging.PopUp;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using BioSportApp.Common;

namespace BioSportApp.ViewModels.CustomPages
{
    public partial class MessageAlertPopUpViewModel : ObservableObject
    {
        [ObservableProperty]
        public Color backgroundColor = new();

        [ObservableProperty]
        public string icon = "";

        [ObservableProperty]
        public string statusText = "";

        [ObservableProperty]
        public string message = "";

        public async void ClosePopUp(PopUpData response)
        {
            int delay;

            Message = response.Message;

            if (response.IsValid)
            {
                BackgroundColor = Color.FromRgb(252, 188, 92);
                Icon = Icons.CircleCheck;
                StatusText = "ÉXITO";
                delay = 1500;
            }
            else
            {
                BackgroundColor = Color.FromRgb(198, 51, 41);
                Icon = Icons.CircleXmark;
                StatusText = "ERROR";
                delay = 3000;
            }


            await Task.Delay(delay);
            WeakReferenceMessenger.Default.Send(new CloseSavedAlertPopUp(true));
        }
    }
}
