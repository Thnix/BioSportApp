using BioSportApp.Models.Routine;
using BioSportApp.Models.Routine.Messages;
using BioSportApp.Services;
using BioSportApp.Utils.Messaging.Routine;
using BioSportApp.ViewModels.Exercise;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BioSportApp.ViewModels.Routine
{
    [QueryProperty(nameof(RoutineId), "RoutineId")]
    public partial class RoutineCreatePageViewModel : ObservableObject
    {
        public ExerciseViewModel ExerciseViewModel { get; }

        private readonly RoutineService routineService;

        public RoutineCreatePageViewModel(RoutineService routineService)
        {
            this.routineService = routineService;
            ExerciseViewModel = new ExerciseViewModel();
        }

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

            await routineService.CreateRoutine(Routine);

            Routine.Exercises.Clear();

            WeakReferenceMessenger.Default.Send(new SendRoutineMessage(new SendRoutineMessageModel { Routine = Routine, Message = "Add" }));

            await Shell.Current.Navigation.PopAsync();
        }

        private async Task UpdateRoutine()
        {
            Routine.Exercises = ExerciseViewModel.Exercises;
            await routineService.UpdateRoutine(Routine);
            Routine.Date = DateTime.Now;

            WeakReferenceMessenger.Default.Send(new SendRoutineMessage( new SendRoutineMessageModel { Routine = Routine, Message = "Update" }));

            await Shell.Current.Navigation.PopAsync();
        }

        private async void LoadRoutine()
        {
            var routine = await routineService.GetRoutineById(Guid.Parse(RoutineId));
            Routine = routine;
            ExerciseViewModel.Exercises = routine.Exercises;
            PageTitle = "Editar Rutina";

            WeakReferenceMessenger.Default.Send(new IsRoutineLoading(false));
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
