using BioSportApp.Messenger.CloseMessage;
using BioSportApp.Messenger.SendMessage;
using BioSportApp.Models.Exercise;
using BioSportApp.Models.Messenger;
using BioSportApp.Models.PopUp;
using BioSportApp.Services;
using BioSportApp.ViewModels.Exercise;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mapster;
using System.Collections.ObjectModel;

namespace BioSportApp.ViewModels.PopUp
{
    public partial class ExercisePickerPopUpViewModel : ObservableObject
    {
        private readonly ExerciseService exerciseService;
        private readonly IPopupService popupService;

        public ExercisePickerPopUpViewModel(ExerciseService exerciseService, IPopupService popupService)
        {
            this.exerciseService = exerciseService;
            this.popupService = popupService;

            Initialize();
        }

        //Observable Properties
        [ObservableProperty]
        public ObservableCollection<ExerciseListViewModel> originalExercises = [];

        [ObservableProperty]
        public ObservableCollection<ExerciseListViewModel> filteredExercises = [];

        [ObservableProperty]
        public string filterText = new("");

        [ObservableProperty]
        public ExerciseListViewModel selectedExercise = new();

        [ObservableProperty]
        private Guid targetExerciseId = new();

        //Variables
        private bool isLoading = false;
        private int currentPage = 1;

        //Methods
        /// <summary>
        /// OnSelectedExerciseChanged
        /// </summary>
        /// <param name="value"></param>
        partial void OnSelectedExerciseChanged(ExerciseListViewModel value)
        {
            var exerciseToSend = new SelectedExerciseMessageModel
            {
                Exercise = value.Adapt<ExerciseListModel>(),
                TargetExerciseId = TargetExerciseId
            };

            WeakReferenceMessenger.Default.Send(new SendSelectedExerciseMessage(exerciseToSend));
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private async void Initialize()
        {
            await GetInitialExercises();
        }

        /// <summary>
        /// GetInitialExercises
        /// </summary>
        /// <returns></returns>
        private async Task GetInitialExercises()
        {
            var response = await exerciseService.GetAllExercises();

            if (response.IsValid && response.Data != null)
            {
                OriginalExercises = response.Data.Take(10).Adapt<ObservableCollection<ExerciseListViewModel>>();
                FilteredExercises = OriginalExercises;
            }
            else
            {
                var messageData = new MessageDataModel
                {
                    IsValid = response.IsValid,
                    Message = response.Message
                };

                await popupService.ShowPopupAsync<MessagePopUpViewModel>(vm => vm.ShowMessage(messageData));
            }
        }

        /// <summary>
        /// LoadExercisesWhileScrolling
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task LoadExercisesWhileScrolling()
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                if (isLoading) return;
                isLoading = true;

                var response = await exerciseService.LoadExercisesWhileScrolling(currentPage, 10);

                if (response.IsValid && response.Data != null)
                {
                    if (currentPage == 1)
                    {
                        FilteredExercises = response.Data.Adapt<ObservableCollection<ExerciseListViewModel>>();
                    }
                    else
                    {
                        foreach (var item in response.Data)
                        {
                            FilteredExercises.Add(item.Adapt<ExerciseListViewModel>());
                        }
                    }

                    currentPage++;
                    isLoading = false;
                }
                else
                {
                    var messageData = new MessageDataModel
                    {
                        IsValid = response.IsValid,
                        Message = response.Message
                    };

                    await popupService.ShowPopupAsync<MessagePopUpViewModel>(vm => vm.ShowMessage(messageData));
                }
            }
        }

        /// <summary>
        /// SearchExercises
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task SearchExercises()
        {
            var response = await exerciseService.SearchExercises(FilterText);

            if(response.IsValid && response.Data != null)
            {
                FilteredExercises = response.Data.Adapt<ObservableCollection<ExerciseListViewModel>>();
            }
            else
            {
                var messageData = new MessageDataModel
                {
                    IsValid = response.IsValid,
                    Message = response.Message
                };

                await popupService.ShowPopupAsync<MessagePopUpViewModel>(vm => vm.ShowMessage(messageData));
            }
        }

        /// <summary>
        /// ClosePopUp
        /// </summary>
        [RelayCommand]
        public async void ClosePopUp()
        {
            WeakReferenceMessenger.Default.Send(new CloseExercisePickerPopUp(true));
        }
    }
}
