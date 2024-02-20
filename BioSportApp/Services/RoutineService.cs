using BioSportApp.Domain;
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

        public RoutineService(BioSportContext bioSportContext) 
        { 
            this.bioSportContext = bioSportContext;
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
            }

            await connection.InsertWithChildrenAsync(routine, recursive: true);
        }

        public async Task UpdateRoutine(RoutineAddModel routineToUpdate)
        {
            var routine = await connection.GetWithChildrenAsync<Routine>(routineToUpdate.Id, recursive: true);
           
            routine.Date = routineToUpdate.Date;
            routine.Name = routineToUpdate.Name;

            await connection.RunInTransactionAsync((SQLiteConnection transaction) =>
            {
                foreach (var exercise in routine.Exercises)
                {
                    transaction.Delete(exercise);
                }

                foreach (var exercise in routineToUpdate.Exercises)
                {
                    var exerciseToAdd = exercise.Adapt<Exercise>();
                    exerciseToAdd.RoutineId = routine.Id;
                    exerciseToAdd.Routine = routine;

                    transaction.Insert(exerciseToAdd);
                    routine.Exercises.Add(exerciseToAdd);
                };

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
