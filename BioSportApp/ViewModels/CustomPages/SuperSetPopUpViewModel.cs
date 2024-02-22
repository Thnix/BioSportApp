using BioSportApp.Models.Exercise;
using BioSportApp.Utils.Messaging.Exercise;
using BioSportApp.Utils.Messaging.PopUp;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BioSportApp.ViewModels.CustomPages
{
    public partial class SuperSetPopUpViewModel : ObservableObject, IDisposable
    {
        public SuperSetPopUpViewModel()
        {
            
        }

        [ObservableProperty]
        public ObservableCollection<ExerciseAddModel> routineExercises = [];

        [ObservableProperty]
        public ExerciseAddModel selectedExercise = new();





        partial void OnSelectedExerciseChanged(ExerciseAddModel value)
        {
            var exercise = RoutineExercises.SingleOrDefault(e => e.Id == SelectedExercise.Id);

            if (exercise != null)
            {
                WeakReferenceMessenger.Default.Send(new SendExerciseMessage(exercise));
            }

            ClosePopUp();

        }

        //[RelayCommand]
        //public void AddExercise(Guid exerciseId)
        //{

        //    var exercise = RoutineExercises.SingleOrDefault(e => e.Id == SelectedExercise.Id);

        //    if(exercise != null) 
        //    {
        //        WeakReferenceMessenger.Default.Send(new SendExerciseMessage(exercise));
        //    }

        //    ClosePopUp();
     
        //}

        [RelayCommand]
        public void ClosePopUp()
        {
            WeakReferenceMessenger.Default.Send(new CloseSuperSetPopUp(true));   
        }

        public void Dispose()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }

        
    }
}
