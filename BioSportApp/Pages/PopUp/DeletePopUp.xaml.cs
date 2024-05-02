using BioSportApp.Messenger.CloseMessage;
using BioSportApp.ViewModels.PopUp;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace BioSportApp.Pages.PopUp;

public partial class DeletePopUp : Popup
{
	public DeletePopUp(DeletePopUpViewModel vm)
	{
		InitializeComponent();
        WeakReferenceMessenger.Default.Register<CloseDeleteRoutinePopUp>(this, (r, m) => ClosePopUp());

        BindingContext = vm;
        Closed += OnPopupClosed;
    }

    private void OnPopupClosed(object? sender, PopupClosedEventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<CloseDeleteRoutinePopUp>(this);
    }

    private async void ClosePopUp()
    {
        await CloseAsync();
    }
}