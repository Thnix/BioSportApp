using BioSportApp.Common.Messaging;
using BioSportApp.Utils.Messaging.PopUp;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace BioSportApp.ViewModels.CustomPages
{
    public partial class MessageAlertPopUpViewModel : ObservableObject
    {
        [ObservableProperty]
        public Color backgroundColor = new();

        [ObservableProperty]
        public string text = "";

        public async void ClosePopUp(PopUpData response)
        {
            Text = response.Message;

            BackgroundColor = response.IsValid 
                ? Color.FromRgb(252, 188, 92) 
                : Color.FromRgb(198, 51, 41);

            int delay = response.IsValid 
                ? 1500 
                : 3000;


            await Task.Delay(delay);
            WeakReferenceMessenger.Default.Send(new CloseSavedAlertPopUp(true));
        }
    }
}
