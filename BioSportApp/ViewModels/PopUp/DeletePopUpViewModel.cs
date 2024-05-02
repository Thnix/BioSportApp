using BioSportApp.Messenger.CloseMessage;
using BioSportApp.Messenger.SendMessage;
using BioSportApp.Models.Routine;
using BioSportApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BioSportApp.ViewModels.PopUp
{
    public partial class DeletePopUpViewModel : ObservableObject
    {
        private readonly RoutineService routineService;

        public DeletePopUpViewModel(RoutineService routineService)
        {
            this.routineService = routineService;
        }

        [ObservableProperty]
        private RoutineAddModel routineToDelete = new();

        [RelayCommand]
        public async Task DeleteRoutine()
        {
            await routineService.DeleteRoutineById(RoutineToDelete.Id);

            WeakReferenceMessenger.Default.Send(new SendRoutineMessage(RoutineToDelete){ Action = "Delete" });

            WeakReferenceMessenger.Default.Send(new CloseDeleteRoutinePopUp(true));
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand]
        public static void ClosePopUp()
        {
            WeakReferenceMessenger.Default.Send(new CloseDeleteRoutinePopUp(true));
        }
    }
}
