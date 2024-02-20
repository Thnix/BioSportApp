using BioSportApp.Models.Routine;
using BioSportApp.Models.Routine.Messages;
using BioSportApp.Services;
using BioSportApp.Utils.Messaging.Routine;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BioSportApp.ViewModels.CustomPages
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

        public void ReceiveRoutine(RoutineAddModel routine)
        {
            RoutineToDelete = routine;
        }

        [RelayCommand]
        public async Task DeleteRoutine()
        {
            await routineService.DeleteRoutineById(RoutineToDelete.Id);

            WeakReferenceMessenger.Default.Send(new SendRoutineMessage(new SendRoutineMessageModel { Routine = RoutineToDelete, Message = "Delete" }));

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
