using BioSportApp.Domain;
using BioSportApp.Models.Exercise;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace BioSportApp.Services
{
    public class SetService
    {
        private readonly BioSportContext bioSportContext;
        private readonly SQLiteAsyncConnection connection;

        public SetService(BioSportContext bioSportContext)
        {
            this.bioSportContext = bioSportContext;
            connection = bioSportContext.GetConnection();
        }

        public void UpdateExerciseSets(Exercise dbExercise, ExerciseAddModel exerciseToAdd, SQLiteConnection transaction)
        {
            List<Set> newSets = [];

            var difference = exerciseToAdd.SetsNumber - dbExercise.SetsNumber;

            if (difference > 0)
            {
                var existingSetsCount = dbExercise.SetsNumber;
                var targetSetsCount = exerciseToAdd.SetsNumber;

                for (var i = existingSetsCount; i < targetSetsCount; i++)
                {
                    newSets.Add(new Set
                    {
                        Id = Guid.NewGuid(),
                        Exercise = dbExercise,
                        ExerciseId = dbExercise.Id,
                        Weight = null,
                        CurrentNumber = i,
                        SetName = $"Serie {i + 1}"
                    });
                }
            }
            else if (difference < 0)
            {
                var setsToDelete = dbExercise.Sets.OrderByDescending(set => set.CurrentNumber).Take((int)-difference).ToList();
                transaction.DeleteAll(setsToDelete);
            }

            if (newSets.Count > 0)
            {
                transaction.InsertAll(newSets);
            }
        }
    }
}
