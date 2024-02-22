using BioSportApp.Utils.Messaging.PopUp;
using BioSportApp.ViewModels.CustomPages;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace BioSportApp.Pages.CustomPages;

public partial class MessageAlertPopUp : Popup
{
	public MessageAlertPopUp(MessageAlertPopUpViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

        WeakReferenceMessenger.Default.Register<CloseSavedAlertPopUp>(this, (r, m) => ClosePopUp());

        Closed += OnPopupClosed;
    }

    private void OnPopupClosed(object? sender, PopupClosedEventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<CloseSavedAlertPopUp>(this);
    }

    private async void ClosePopUp()
    {
        await CloseAsync();
    }
}