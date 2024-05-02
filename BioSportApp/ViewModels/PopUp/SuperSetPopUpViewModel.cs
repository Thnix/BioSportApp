using BioSportApp.Messenger.CloseMessage;
using BioSportApp.Messenger.SendMessage;
using BioSportApp.Models.RoutineExercise;
using BioSportApp.ViewModels.RoutineExercise;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mapster;
using System.Collections.ObjectModel;

namespace BioSportApp.ViewModels.PopUp
{
    public partial class SuperSetPopUpViewModel : ObservableObject
    {

        //Observable Properties
        [ObservableProperty]
        public ObservableCollection<RoutineExerciseViewModel> routineExercises = [];

        [ObservableProperty]
        public RoutineExerciseViewModel selectedExercise = new();

        //Methods

        /// <summary>
        /// OnSelectedExerciseChanged
        /// </summary>
        /// <param name="value"></param>
        partial void OnSelectedExerciseChanged(RoutineExerciseViewModel value)
        {
            var routineExercise = RoutineExercises.SingleOrDefault(re => re.ExerciseId == SelectedExercise.ExerciseId);

            if (routineExercise != null)
            {
                WeakReferenceMessenger.Default.Send(new SendSelectedRoutineExerciseMessage(routineExercise.Adapt<RoutineExerciseAddModel>()));
            }

            ClosePopUp();
        }

        /// <summary>
        /// ClosePopUp
        /// </summary>
        [RelayCommand]
        public void ClosePopUp()
        {
            WeakReferenceMessenger.Default.Send(new CloseSuperSetPopUp(true));   
        }
    }
}
