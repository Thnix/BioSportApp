using BioSportApp.Models.Routine;
using BioSportApp.Pages.Routine;
using BioSportApp.Services;
using BioSportApp.Utils.Messaging.Routine;
using BioSportApp.ViewModels.CustomPages;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BioSportApp.ViewModels.Routine
{
    public partial class RoutineListPageViewModel : ObservableObject
    {
        private readonly RoutineService routineService;
        private readonly IPopupService popupService;

        

        public RoutineListPageViewModel(RoutineService routineService, IPopupService popupService)
        {
            this.routineService = routineService;
            this.popupService = popupService;

            WeakReferenceMessenger.Default.Register<SendRoutineMessage>(this, (r, m) =>
            {
                switch(m.Value.Message) 
                {
                    case "Add":
                        AddToList(m.Value.Routine);
                        break;

                    case "Update":
                        UpdateRoutine(m.Value.Routine);
                        break;

                    case "Delete":
                        DeleteRoutine(m.Value.Routine);
                        break;

                    default: 
                        return;
                }
            });


            WeakReferenceMessenger.Default.Register<IsRoutineLoading>(this, (r, m) =>
            {
                IsRoutineLoading(m.Value);
            });
        }


        [ObservableProperty]
        public bool isInProgress = false;

        [ObservableProperty]
        private RoutineAddModel? selectedRoutine;


        async partial void OnSelectedRoutineChanged(RoutineAddModel? value)
        {
            if(value != null)
            {
                await Shell.Current.GoToAsync($"{nameof(RoutineViewPage)}?RoutineId={value.Id}");
                SelectedRoutine = null;
            } 
        }


        [ObservableProperty]
        private ObservableCollection<RoutineAddModel> routines = [];

        [RelayCommand]
        public async Task GoToEditRoutinePage(Guid routineId)
        {
            IsInProgress = true;
            await Shell.Current.GoToAsync($"{nameof(RoutineCreatePage)}?RoutineId={routineId}");
        }

        [RelayCommand]
        public async Task ShowPopUpDeleteRoutine(Guid routineId)
        {
            var routineToRemove = Routines.SingleOrDefault(r => r.Id == routineId);
            await popupService.ShowPopupAsync<DeletePopUpViewModel>(onPresenting: vm => vm.ReceiveRoutine(routineToRemove));
        }

        [RelayCommand]
        public async Task NewRoutine()
        {
            await Shell.Current.GoToAsync(nameof(RoutineCreatePage));
        }

        public async Task LoadAllRoutines()
        {
            if(Routines.Count == 0)
            {
                Routines = new ObservableCollection<RoutineAddModel>(await routineService.GetRoutines());

            }
        }

        public void DeleteRoutine(RoutineAddModel routine)
        {
            var routineToRemove = Routines.SingleOrDefault(x => x.Id == routine.Id);

            if(routineToRemove != null)
            {
                Routines.Remove(routineToRemove);
            }
        }

        public void UpdateRoutine(RoutineAddModel routine)
        {
            var routineToUpdate = Routines.SingleOrDefault(rt => rt.Id == routine.Id);

            if (routineToUpdate != null)
            {
                var index = Routines.IndexOf(routineToUpdate);
                Routines[index] = routine;
            }
        }

        public void AddToList(RoutineAddModel routine)
        {
            Routines.Add(routine);
        }

        public void IsRoutineLoading(bool loading)
        {
            IsInProgress = loading;
        }
    }
}
