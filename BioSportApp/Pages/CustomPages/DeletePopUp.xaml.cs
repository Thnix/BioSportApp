using BioSportApp.Utils.Messaging.Routine;
using BioSportApp.ViewModels.CustomPages;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace BioSportApp.Common.CustomPages;

public partial class DeletePopUp : Popup
{
	public DeletePopUp(DeletePopUpViewModel viewModel)
	{
		InitializeComponent();
        WeakReferenceMessenger.Default.Register<CloseDeleteRoutinePopUp>(this, (r, m) => ClosePopUp());

        BindingContext = viewModel;
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