using BioSportApp.Common.Messaging;
using BioSportApp.Models.Routine;
using BioSportApp.Models.Routine.Messages;
using BioSportApp.Services;
using BioSportApp.Utils.Messaging.Routine;
using BioSportApp.ViewModels.CustomPages;
using BioSportApp.ViewModels.Exercise;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mapster;

namespace BioSportApp.ViewModels.Routine
{
    [QueryProperty(nameof(RoutineId), "RoutineId")]
    public partial class RoutineCreatePageViewModel(RoutineService routineService, IPopupService popupService) : ObservableObject
    {
        public ExerciseViewModel ExerciseViewModel { get; } = new ExerciseViewModel();

        private readonly RoutineService routineService = routineService;
        private readonly IPopupService popupService = popupService;

        [ObservableProperty]
        private string routineId = "";

        [ObservableProperty]
        private string pageTitle = "Crear rutina";

        [ObservableProperty]
        private RoutineAddModel routine = new();



        [ObservableProperty]
        public bool isRoutineNameValid = new();

        [ObservableProperty]
        public bool showError = false;


        [RelayCommand]
        private async Task CreateOrUpdateRoutine()
        {

            if(IsRoutineNameValid)
            {
                if (RoutineId != "")
                {
                    
                    await UpdateRoutine();
                }
                else
                {
                    await CreateRoutine();
                }
            }
            else
            {
                ShowError = true;
            }
        }

        private async Task CreateRoutine()
        {
            Routine.Exercises = ExerciseViewModel.Exercises;
            Routine.Id = Guid.NewGuid();
            Routine.Date = DateTime.Now;

            var response = await routineService.CreateRoutine(Routine);

            if (response.IsValid)
            {
                Routine.Exercises.Clear();
                WeakReferenceMessenger.Default.Send(new SendRoutineMessage(new SendRoutineMessageModel { Routine = Routine, Message = "Add" }));
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                await popupService.ShowPopupAsync<MessageAlertPopUpViewModel>(vm => vm.ClosePopUp(response.Adapt<PopUpData>()));
            } 
        }

        private async Task UpdateRoutine()
        {
            Routine.Exercises = ExerciseViewModel.Exercises;

            var response = await routineService.UpdateRoutine(Routine);

            if (response.IsValid)
            {
                Routine.Date = DateTime.Now;
                WeakReferenceMessenger.Default.Send(new SendRoutineMessage(new SendRoutineMessageModel { Routine = Routine, Message = "Update" }));
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                await popupService.ShowPopupAsync<MessageAlertPopUpViewModel>(vm => vm.ClosePopUp(response.Adapt<PopUpData>()));
            }
        }

        private async void LoadRoutine()
        {
            var response = await routineService.GetRoutineById(Guid.Parse(RoutineId));

            if(response.IsValid && response.Data != null)
            {
                Routine = response.Data;
                ExerciseViewModel.Exercises = response.Data.Exercises;
                PageTitle = "Editar Rutina";

                WeakReferenceMessenger.Default.Send(new IsRoutineLoading(false));
            }
            else
            {
                await popupService.ShowPopupAsync<MessageAlertPopUpViewModel>(vm => vm.ClosePopUp(response.Adapt<PopUpData>()));
            }
        }

        partial void OnRoutineIdChanged(string value)
        {
            if (RoutineId != "")
            {
                LoadRoutine();
            }
        }

        partial void OnIsRoutineNameValidChanged(bool value)
        {
            ShowError = !value;
        }
    }
}
