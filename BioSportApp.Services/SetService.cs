using BioSportApp.Domain.Core;
using BioSportApp.Models.RoutineExercise;
using Domain.Core;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace BioSportApp.Services
{
    public class SetService(BioSportContext context)
    {
        private readonly SQLiteAsyncConnection connectionAsync = context.GetConnectionAsync();

        /// <summary>
        /// AddSets
        /// </summary>
        /// <param name="routineExercises"></param>
        /// <param name="transaction"></param>
        public void CreateSets(List<RoutineExercise> routineExercises, SQLiteConnection transaction)
        {
            List<Set> setsToAdd = [];

            foreach (var routineExercise in routineExercises)
            {
                for (var i = 0; i < routineExercise.SetsNumber; i++)
                {
                    setsToAdd.Add(new Set
                    {
                        Id = Guid.NewGuid(),
                        SetName = $"Serie {i + 1}",
                        Weight = 0,
                        RoutineExerciseId = routineExercise.Id
                    });
                }
            }

            transaction.InsertAll(setsToAdd);
        }

        /// <summary>
        /// UpdateExerciseSets
        /// </summary>
        /// <param name="dbRoutineExercise"></param>
        /// <param name="routineExerciseToAdd"></param>
        /// <param name="transaction"></param>
        public void UpdateExerciseSets(RoutineExercise dbRoutineExercise, RoutineExerciseAddModel routineExerciseToAdd, SQLiteConnection transaction)
        {
            List<Set> newSets = [];

            var difference = routineExerciseToAdd.SetsNumber - dbRoutineExercise.SetsNumber;

            if (difference > 0)
            {
                var existingSetsCount = dbRoutineExercise.SetsNumber;
                var targetSetsCount = routineExerciseToAdd.SetsNumber;

                for (var i = existingSetsCount; i < targetSetsCount; i++)
                {
                    newSets.Add(new Set
                    {
                        Id = Guid.NewGuid(),
                        RoutineExercise = dbRoutineExercise,
                        RoutineExerciseId = dbRoutineExercise.Id,
                        Weight = 0,
                        CurrentNumber = i,
                        SetName = $"Serie {i + 1}"
                    });
                }
            }
            else if (difference < 0)
            {
                var setsToDelete = dbRoutineExercise.Sets.OrderByDescending(set => set.CurrentNumber).Take((int)-difference).ToList();
                transaction.DeleteAll(setsToDelete);
            }

            if (newSets.Count > 0)
            {
                transaction.InsertAll(newSets);
            }
        }
    }
}
