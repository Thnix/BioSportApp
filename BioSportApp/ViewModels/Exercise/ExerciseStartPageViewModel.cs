using BioSportApp.Models.Exercise;
using BioSportApp.Models.Set;
using BioSportApp.Services;
using BioSportApp.Utils.Messaging.Exercise;
using BioSportApp.Utils.Messaging.PopUp;
using BioSportApp.ViewModels.CustomPages;
using BioSportApp.ViewModels.Weight;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BioSportApp.ViewModels.Exercise
{
    [QueryProperty(nameof(ExerciseId), "ExerciseId")]
    public partial class ExerciseStartPageViewModel : ObservableObject, IDisposable
    {
        private readonly ExerciseService exerciseService;
        private readonly IPopupService popupService;

        public WeightRecordViewModel WeightRecord { get; }

        public ExerciseStartPageViewModel(ExerciseService exerciseService, IPopupService popupService)
        {
            this.exerciseService = exerciseService;
            this.popupService = popupService;

            WeightRecord = new WeightRecordViewModel();

            WeakReferenceMessenger.Default.Register<SendExerciseMessage>(this, (r, m) =>
            {
                ExerciseId = m.Value.Id.ToString();
         
                    //AddWorkoutExercises(m.Value);
            
                
            });
        }

        private ExerciseAddModel LastExerciseReceived = new();

        [ObservableProperty]
        private string exerciseId = "";

        [ObservableProperty]
        public ExerciseAddModel exercise = new();

        [ObservableProperty]
        public ObservableCollection<ExerciseAddModel> routineExercises = [];

        [ObservableProperty]
        public ObservableCollection<ExerciseAddModel> workoutExercises = [];

        


        partial void OnExerciseIdChanged(string value)
        {
            LoadExercise();
        }

        private async void LoadExercise()
        {
            Exercise = await exerciseService.GetExerciseById(Guid.Parse(ExerciseId));

            if (Exercise.Sets.Any())
            {
                WorkoutExercises.Add(Exercise);
            }
            else
            {
                AddWorkoutExercises(Exercise);
            }
        }

        public void AddWorkoutExercises(ExerciseAddModel exercise)
        {
            var e = WorkoutExercises.SingleOrDefault(x => x.Id == exercise.Id);

            if (e == null)
            {
                for (int i = 0; i < exercise.SetsNumber; i++)
                {
                    exercise.Sets.Add(new SetAddModel
                    {
                        Id = Guid.NewGuid(),
                        Number = $"Serie {i + 1}",
                        Weight = null,
                        ExerciseId = exercise.Id,
                        //Exercise = exercise,
                    });
                }

                WorkoutExercises.Add(exercise);
            }
        }

        [RelayCommand]
        public async Task GetExercisesByRoutineId()
        {
            RoutineExercises = new ObservableCollection<ExerciseAddModel>(await exerciseService.GetExercisesByRoutineId(Exercise.RoutineId));

            var exercisesToShow = new ObservableCollection<ExerciseAddModel>(
                RoutineExercises.Where(exercise => !WorkoutExercises.Any(workoutExercise => workoutExercise.Id == exercise.Id))
            );

            await popupService.ShowPopupAsync<SuperSetPopUpViewModel>(onPresenting: vm => vm.RoutineExercises = exercisesToShow);
        }

        //public async Task GetExerciseByRoutineId()
        //{
        //    Exercise = await exerciseService.GetExerciceById(Guid.Parse(ExerciseId));
        //}


        [RelayCommand]
        public async Task ShowPopUp()
        {
            await popupService.ShowPopupAsync<StopWatchPopUpViewModel>();

        }

        [RelayCommand]
        public async Task SaveWorkout()
        {
            await exerciseService.SaveWorkout(WorkoutExercises);
            ShowToast();
        }

        public void Dispose()
        {
            //WeakReferenceMessenger.Default.UnregisterAll(this);
            WeakReferenceMessenger.Default.Unregister<CloseSuperSetPopUp>(this);

        }

        public async void ShowToast()
        {
            //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            //string text = "Guardado";
            //ToastDuration duration = ToastDuration.Short;
            //double fontSize = 14;

            //var toast = Toast.Make(text, duration, fontSize);

            //await toast.Show();

            //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            //var snackbarOptions = new SnackbarOptions
            //{
            //    BackgroundColor = Colors.White,
            //    TextColor = Colors.Black,
            //    ActionButtonTextColor = Colors.Red,
            //    CornerRadius = new CornerRadius(10),
            //    Font = Microsoft.Maui.Font.SystemFontOfSize(14),
            //    ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14),
            //    CharacterSpacing = 0.5
            //};

            //string text = "This is a Snackbar";
            //string actionButtonText = "Eliminar";
            //Action action = async () => await xd();


            //var snackbar = Snackbar.Make(text, action, actionButtonText, null, snackbarOptions);

            //await snackbar.Show(cancellationTokenSource.Token);
        }

        public async Task xd()
        {

        }
    }
}
