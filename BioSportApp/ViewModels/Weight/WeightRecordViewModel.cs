using BioSportApp.Models.Weight;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;


namespace BioSportApp.ViewModels.Weight
{
    public partial class WeightRecordViewModel : ObservableObject
    {
        public WeightRecordViewModel()
        {
            
        }

        [ObservableProperty]
        public ObservableCollection<WeightRecordAddModel> weights = [];

        [ObservableProperty]
        private WeightRecordAddModel weight = new();


        [RelayCommand]
        public void NewWeight()
        {
            var newWeight = new WeightRecordAddModel
            {
                Id = Guid.NewGuid(),
                Weight = 0,
                Date = DateTime.Now,
            };

            Weights.Add(newWeight);
        }
    }
}
