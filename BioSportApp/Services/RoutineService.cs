using BioSportApp.Domain;
using BioSportApp.Models.Exercise;
using BioSportApp.Models.Routine;
using Mapster;
using SQLite;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;


namespace BioSportApp.Services
{
    public class RoutineService
    {
        private readonly BioSportContext bioSportContext;
        private readonly SQLiteAsyncConnection connection;
        private readonly ExerciseService exerciseService;

        public RoutineService(BioSportContext bioSportContext, ExerciseService exerciseService) 
        { 
            this.bioSportContext = bioSportContext;
            this.exerciseService = exerciseService;
            connection = bioSportContext.GetConnection(); 
        }

        public async Task CreateRoutine(RoutineAddModel routineToCreate)
        {
            var routine = routineToCreate.Adapt<Routine>();

            foreach(var exercise in routine.Exercises)
            {
                exercise.Id = Guid.NewGuid();
                exercise.RoutineId = routine.Id;
                exercise.Routine = routine;

                for (var i = 0; i < exercise.SetsNumber; i++)
                {
                    var set = new Set 
                    { 
                        Id = Guid.NewGuid(),
                        Exercise = exercise,
                        ExerciseId = exercise.Id,
                        Weight = null,
                        Number = $"Serie {i+1}"
                    };

                    exercise.Sets.Add(set);
                }
            }

            await connection.InsertWithChildrenAsync(routine, recursive: true);
        }

        public async Task UpdateRoutine(RoutineAddModel routineToUpdate)
        {
            var routine = await connection.GetWithChildrenAsync<Routine>(routineToUpdate.Id, recursive: true);
           
            routine.Name = routineToUpdate.Name;

            await connection.RunInTransactionAsync((SQLiteConnection transaction) =>
            {
                exerciseService.UpdateRoutineExercises(routine, routineToUpdate.Exercises, transaction);

                transaction.Update(routine);
            });
        }

        public async Task<List<RoutineAddModel>> GetRoutines()
        {
            var routineList = await connection.Table<Routine>().ToListAsync();
            return routineList.Adapt<List<RoutineAddModel>>();
        }

        public async Task<RoutineAddModel> GetRoutineById(Guid routineId)
        {
            var routine = await connection.GetWithChildrenAsync<Routine>(routineId);
            return routine.Adapt<RoutineAddModel>();
        }

        public async Task DeleteRoutineById(Guid routineId)
        {
            await connection.RunInTransactionAsync(transaction =>
            {
                var routine = transaction.GetWithChildren<Routine>(routineId);
                transaction.Delete(routine, recursive: true);
            });
        }
    }
}
