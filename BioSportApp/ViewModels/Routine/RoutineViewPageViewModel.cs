using BioSportApp.Common.Messaging;
using BioSportApp.Models.Routine;
using BioSportApp.Pages.Exercise;
using BioSportApp.Pages.Routine;
using BioSportApp.Services;
using BioSportApp.Utils.Messaging.Routine;
using BioSportApp.ViewModels.CustomPages;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mapster;


namespace BioSportApp.ViewModels.Routine
{
    [QueryProperty(nameof(RoutineId), "RoutineId")]
    public partial class RoutineViewPageViewModel : ObservableObject
    {
        private readonly RoutineService routineService;
        private readonly IPopupService popupService;

        public RoutineViewPageViewModel(RoutineService routineService, IPopupService popupService) 
        {
            this.routineService = routineService;
            this.popupService = popupService;

            WeakReferenceMessenger.Default.Register<SendRoutineMessage>(this, (r, m) =>
            {
                switch (m.Value.Message)
                {
                    case "Update":
                        Routine = m.Value.Routine;                        
                        return;
                }
            });
        }

        [ObservableProperty]
        private string routineId = "";

        [ObservableProperty]
        public RoutineAddModel routine = new();

        partial void OnRoutineIdChanged(string value)
        {
            LoadRoutine(value);
        }

        private async void LoadRoutine(string routineId)
        {
            var response = await routineService.GetRoutineById(Guid.Parse(routineId));

            if (response.IsValid && response.Data != null)
            {
                Routine = response.Data;
            }
            else
            {
                await popupService.ShowPopupAsync<MessageAlertPopUpViewModel>(vm => vm.ClosePopUp(response.Adapt<PopUpData>()));
            } 
        }

        [RelayCommand]
        public async Task GoToEditRoutinePage()
        {
            await Shell.Current.GoToAsync($"{nameof(RoutineCreatePage)}?RoutineId={RoutineId}");
        }

        [RelayCommand]
        public async Task ShowPopUpDeleteRoutine(Guid routineId)
        {
            await popupService.ShowPopupAsync<DeletePopUpViewModel>(onPresenting: vm => vm.ReceiveRoutine(Routine));
        }

        [RelayCommand]
        public async Task GoToStartExercise(Guid exerciseId)
        {
            await Shell.Current.GoToAsync($"{nameof(ExerciseStartPage)}?ExerciseId={exerciseId}");
        }
    }
}
