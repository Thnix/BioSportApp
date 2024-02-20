using BioSportApp.Utils.Messaging;
using BioSportApp.ViewModels.CustomPages;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace BioSportApp.Pages.CustomPages;

public partial class StopWatchPopUp : Popup
{
	public StopWatchPopUp()
	{
		InitializeComponent();

		BindingContext = new StopWatchPopUpViewModel();

        WeakReferenceMessenger.Default.Register<CloseStopWatchPopUp>(this, (r, m) => ClosePopUp());
        Closed += OnPopupClosed;

    }

    private void OnPopupClosed(object? sender, PopupClosedEventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<CloseStopWatchPopUp>(this);
    }

    private async void ClosePopUp()
    {
        await CloseAsync();
    }
}