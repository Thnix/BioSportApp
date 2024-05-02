using BioSportApp.Messenger.CloseMessage;
using BioSportApp.ViewModels.PopUp;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace BioSportApp.Pages.PopUp;

public partial class ExercisePickerPopUp : Popup
{
	public ExercisePickerPopUp(ExercisePickerPopUpViewModel vm)
	{
		BindingContext = vm;
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<CloseExercisePickerPopUp>(this, (r, m) => CloseAsync());

        Closed += OnPopupClosed;
    }

    private void OnPopupClosed(object? sender, PopupClosedEventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<CloseExercisePickerPopUp>(this);
    }
}