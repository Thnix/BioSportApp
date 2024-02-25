using BioSportApp.Common.Messaging;
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
using Mapster;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Font = Microsoft.Maui.Font;

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
            var response = await exerciseService.GetExerciseById(Guid.Parse(ExerciseId));

            if(response.IsValid && response.Data != null)
            {
                Exercise = response.Data.Adapt<ExerciseAddModel>();

                if (Exercise.Sets.Any())
                {
                    WorkoutExercises.Add(Exercise);
                }
                else
                {
                    //AddWorkoutExercises(Exercise);
                }
            }
            else
            {
                await Task.Delay(200);
                await popupService.ShowPopupAsync<MessageAlertPopUpViewModel>(onPresenting: vm => vm.ClosePopUp(response.Adapt<PopUpData>()));
                await Shell.Current.Navigation.PopAsync();

            }
        }

        //public void AddWorkoutExercises(ExerciseAddModel exercise)
        //{
        //    var e = WorkoutExercises.SingleOrDefault(x => x.Id == exercise.Id);

        //    if (e == null)
        //    {
        //        for (int i = 0; i < exercise.SetsNumber; i++)
        //        {
        //            exercise.Sets.Add(new SetAddModel
        //            {
        //                Id = Guid.NewGuid(),
        //                Number = $"Serie {i + 1}",
        //                Weight = null,
        //                ExerciseId = exercise.Id,
        //                //Exercise = exercise,
        //            });
        //        }

        //        WorkoutExercises.Add(exercise);
        //    }
        //}

        [RelayCommand]
        public async Task GetExercisesByRoutineId()
        {
            var response = await exerciseService.GetExercisesByRoutineId(Exercise.RoutineId);

            if(response.IsValid && response.Data != null)
            {
                RoutineExercises = new ObservableCollection<ExerciseAddModel>(response.Data);

                var exercisesToShow = new ObservableCollection<ExerciseAddModel>(
                    RoutineExercises.Where(exercise => !WorkoutExercises.Any(workoutExercise => workoutExercise.Id == exercise.Id))
                );

                await popupService.ShowPopupAsync<SuperSetPopUpViewModel>(onPresenting: vm => vm.RoutineExercises = exercisesToShow);
            }
            else
            {
                await popupService.ShowPopupAsync<MessageAlertPopUpViewModel>(onPresenting: vm => vm.ClosePopUp(response.Adapt<PopUpData>()));
            }
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
            var response = await exerciseService.SaveWorkout(WorkoutExercises);

            await popupService.ShowPopupAsync<MessageAlertPopUpViewModel>(onPresenting: vm => vm.ClosePopUp(response.Adapt<PopUpData>()));
        }

        public void Dispose()
        {
            WeakReferenceMessenger.Default.Unregister<CloseSuperSetPopUp>(this);
        }

        public async void ShowToast(bool isValid)
        {
            //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            //string text = "Guardado";
            //ToastDuration duration = ToastDuration.Short;
            //double fontSize = 14;

            //var toast = Toast.Make(text, duration, fontSize);

            //await toast.Show();

            //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();


            //var backgroundColor = new Color();
            //var text = "";

            //if (isValid)
            //{
            //    text = "Guardado!";
            //    backgroundColor = new Color(252, 188, 92);
            //}
            //else
            //{
            //    text = "Ha ocurrido un error!";
            //    backgroundColor = Colors.Red;
            //}

            //var snackbarOptions = new SnackbarOptions
            //{
            //    BackgroundColor = backgroundColor,
            //    TextColor = Colors.Black,
            //    CornerRadius = new CornerRadius(10),
            //    Font = Font.SystemFontOfSize(16),
            //};

            //var snackbar = Snackbar.Make(text,null,"",null, snackbarOptions);

           

            //await snackbar.Show(cancellationTokenSource.Token);
        }
    }
}
