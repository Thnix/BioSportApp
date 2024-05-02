using BioSportApp.Messenger.CloseMessage;
using BioSportApp.ViewModels.PopUp;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace BioSportApp.Pages.PopUp;

public partial class MessagePopUp : Popup
{
	public MessagePopUp(MessagePopUpViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();

		WeakReferenceMessenger.Default.Register<CloseMessagePopUp>(this, (r, m) => CloseAsync());

		Closed += OnPopupClosed;
    }

    private void OnPopupClosed(object? sender, PopupClosedEventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<CloseMessagePopUp>(this);
    }
}