using BioSportApp.Common.Messaging;
using BioSportApp.Messenger.SendMessage;
using BioSportApp.Models.Exercise;
using BioSportApp.Models.PopUp;
using BioSportApp.Models.RoutineExercise;
using BioSportApp.Services;
using BioSportApp.ViewModels.PopUp;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mapster;
using System.Collections.ObjectModel;

namespace BioSportApp.ViewModels.RoutineExercise
{
    [QueryProperty(nameof(RoutineExerciseId), "RoutineExerciseId")]
    public partial class RoutineExerciseStartPageViewModel : ObservableObject
    {
        private readonly RoutineExerciseService exerciseService;
        private readonly IPopupService popupService;

        public RoutineExerciseStartPageViewModel(RoutineExerciseService exerciseService, IPopupService popupService)
        {
            this.exerciseService = exerciseService;
            this.popupService = popupService;

            WeakReferenceMessenger.Default.Register<SendSelectedRoutineExerciseMessage>(this, (r, m) =>
            {
                RoutineExerciseId = m.Value.Id.ToString();
            });
        }

        //Observable Properties
        [ObservableProperty]
        private string routineExerciseId = "";

        [ObservableProperty]
        public RoutineExerciseViewModel routineExercise = new();

        [ObservableProperty]
        public ObservableCollection<RoutineExerciseViewModel> routineExercises = [];

        [ObservableProperty]
        public ObservableCollection<RoutineExerciseViewModel> workoutExercises = [];

        //Methods

        /// <summary>
        /// OnRoutineExerciseIdChanged
        /// </summary>
        /// <param name="value"></param>
        partial void OnRoutineExerciseIdChanged(string value)
        {
            LoadExercise();
        }

        /// <summary>
        /// LoadExercise
        /// </summary>
        private async void LoadExercise()
        {
            var response = await exerciseService.GetRoutineExerciseById(Guid.Parse(RoutineExerciseId));

            if (!response.IsValid)
            {
                var messageData = new MessageDataModel
                {
                    IsValid = response.IsValid,
                    Message = response.Message
                };

                await Task.Delay(200);
                await popupService.ShowPopupAsync<MessagePopUpViewModel>(onPresenting: vm => vm.ShowMessage(messageData));
                await Shell.Current.Navigation.PopAsync();

                return;
            }

            RoutineExercise = response.Data.Adapt<RoutineExerciseViewModel>();
            WorkoutExercises.Add(RoutineExercise);
        }

        /// <summary>
        /// RemoveRoutineExercise
        /// </summary>
        /// <param name="RoutineExerciseId"></param>
        [RelayCommand]
        public void RemoveRoutineExercise(Guid RoutineExerciseId)
        {
            var routineExerciseToRemove = WorkoutExercises.SingleOrDefault(re => re.Id == RoutineExerciseId);

            if (routineExerciseToRemove != null)
            {
                WorkoutExercises.Remove(routineExerciseToRemove);
            }
        }

        /// <summary>
        /// GetExercisesByRoutineId
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task GetExercisesByRoutineId()
        {
            var response = await exerciseService.GetRoutineExercisesByRoutineId(RoutineExercise.RoutineId);

            if (!response.IsValid)
            {
                var messageData = new MessageDataModel
                {
                    IsValid = response.IsValid,
                    Message = response.Message
                };

                await ShowMessage(messageData);
                return;
            }

            RoutineExercises = response.Data.Adapt<ObservableCollection<RoutineExerciseViewModel>>();

            var exercisesToShow = new ObservableCollection<RoutineExerciseViewModel>(
                RoutineExercises.Where(exercise => !WorkoutExercises.Any(workoutExercise => workoutExercise.Id == exercise.Id))
            );

            await popupService.ShowPopupAsync<SuperSetPopUpViewModel>(onPresenting: vm => vm.RoutineExercises = exercisesToShow);
        }

        /// <summary>
        /// SaveWorkout
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task SaveWorkout()
        {
            var messageData = ValidateWorkout();

            if (!messageData.IsValid)
            {
                await ShowMessage(messageData);
                return;
            }

            var response = await exerciseService.SaveWorkout(WorkoutExercises.Adapt<List<RoutineExerciseAddModel>>());

            messageData.IsValid = response.IsValid;
            messageData.Message = response.Message;

            await ShowMessage(messageData);
        }


        /// <summary>
        /// ValidateWorkout
        /// </summary>
        private MessageDataModel ValidateWorkout()
        {
            var messageData = new MessageDataModel();

            if (WorkoutExercises.All(exercise => exercise.Sets.Any(set => set.Weight == 0)))
            {
                messageData.IsValid = false;
                messageData.Message = "Complete las series.";
            }
            else
            {
                messageData.IsValid = true;
            }

            return messageData;
        }


        /// <summary>
        /// ShowMessage
        /// </summary>
        /// <param name="messageData"></param>
        /// <returns></returns>
        private async Task ShowMessage(MessageDataModel messageData)
        {
            await popupService.ShowPopupAsync<MessagePopUpViewModel>(vm => vm.ShowMessage(messageData));
        }

        /// <summary>
        /// ShowStopWatchPopUp
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task ShowStopWatchPopUp()
        {
            await popupService.ShowPopupAsync<StopWatchPopUpViewModel>();
        }
    }
}
