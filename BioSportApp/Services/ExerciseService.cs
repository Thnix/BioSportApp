using BioSportApp.Domain;
using BioSportApp.Models.Exercise;
using Mapster;
using Microsoft.Maui.Controls;
using SQLite;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;
using System.Collections.ObjectModel;

namespace BioSportApp.Services
{
    public class ExerciseService
    {
        private readonly BioSportContext bioSportContext;
        private readonly SQLiteAsyncConnection connection;

        public ExerciseService(BioSportContext bioSportContext) 
        { 
            this.bioSportContext = bioSportContext;
            connection = bioSportContext.GetConnection();
        }

        public async Task<ExerciseAddModel> GetExerciseById(Guid id)
        {
            var exercise = await connection.GetWithChildrenAsync<Exercise>(id);

            return exercise.Adapt<ExerciseAddModel>();
        }

        public async Task<List<ExerciseAddModel>> GetExercisesByRoutineId(Guid routineId)
        {
            var routineExercises = await connection.Table<Exercise>().Where(e => e.RoutineId == routineId).ToListAsync();

            return routineExercises.Adapt<List<ExerciseAddModel>>();
        }

        public async Task SaveWorkout(ObservableCollection<ExerciseAddModel> exercises)
        {
            await connection.RunInTransactionAsync((SQLiteConnection transaction) =>
            {
                foreach (var exercise in exercises)
                {
                    foreach (var set in exercise.Sets)
                    {
                        transaction.Delete(set.Adapt<Set>());
                    }
                }

                foreach (var exercise in exercises)
                {
                    var exerciseToUpdate = exercise.Adapt<Exercise>();

                    foreach (var set in exerciseToUpdate.Sets)
                    {
                        transaction.Insert(set);
                    }

                    transaction.Update(exerciseToUpdate);
                }
            });
        }
    }
}
