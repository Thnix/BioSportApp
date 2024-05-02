using BioSportApp.Messenger.CloseMessage;
using BioSportApp.ViewModels.PopUp;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace BioSportApp.Pages.PopUp;

public partial class SuperSetPopUp : Popup
{
	public SuperSetPopUp(SuperSetPopUpViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

        WeakReferenceMessenger.Default.Register<CloseSuperSetPopUp>(this, (r, m) => ClosePopUp());
        Closed += OnPopupClosed;
    }

    private void OnPopupClosed(object? sender, PopupClosedEventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<CloseSuperSetPopUp>(this);
    }

    private async void ClosePopUp()
    {
        await CloseAsync(); 
    }
}