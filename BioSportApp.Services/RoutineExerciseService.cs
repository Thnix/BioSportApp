using BioSportApp.Common.Messaging;
using BioSportApp.Domain.Core;
using BioSportApp.Models.RoutineExercise;
using Domain.Core;
using Mapster;
using SQLite;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;

namespace BioSportApp.Services
{
    public class RoutineExerciseService(SetService setService, BioSportContext context)
    {
        private readonly SetService setService = setService;
        private readonly SQLiteAsyncConnection connectionAsync = context.GetConnectionAsync();

        /// <summary>
        /// PrepareRoutineExercises
        /// </summary>
        /// <param name="routine"></param>
        public void AddRoutineExercises(Routine routine, SQLiteConnection transaction)
        {
            List<RoutineExercise> routineExerciseList = [];

            foreach (var routineExercise in routine.RoutineExercises)
            {
                routineExercise.Id = Guid.NewGuid();
                routineExercise.RoutineId = routine.Id;
                routineExerciseList.Add(routineExercise);
            }

            transaction.InsertAll(routineExerciseList);
            setService.CreateSets(routine.RoutineExercises, transaction);
        }

        /// <summary>
        /// UpdateRoutineExercises
        /// </summary>
        /// <param name="dbRoutine"></param>
        /// <param name="routineExercisesToAdd"></param>
        /// <param name="transaction"></param>
        public void UpdateRoutineExercises(Routine dbRoutine, List<RoutineExerciseAddModel> routineExercisesToAdd, SQLiteConnection transaction)
        {
            List<RoutineExercise> newRoutineExercises = [];
            List<RoutineExercise> routineExercisesToUpdate = [];

            foreach (var routineExerciseToAdd in routineExercisesToAdd)
            {
                var dbRoutineExercise = dbRoutine.RoutineExercises.SingleOrDefault(re => re.Id == routineExerciseToAdd.Id);

                if (dbRoutineExercise == null)
                {
                    routineExerciseToAdd.RoutineId = dbRoutine.Id;
                    newRoutineExercises.Add(routineExerciseToAdd.Adapt<RoutineExercise>());
                }
                else
                {
                    if (
                        dbRoutineExercise.Repetitions != routineExerciseToAdd.Repetitions ||
                        dbRoutineExercise.SetsNumber != routineExerciseToAdd.SetsNumber ||
                        dbRoutineExercise.ExerciseId != routineExerciseToAdd.ExerciseId)
                    {
                        if (dbRoutineExercise.SetsNumber != routineExerciseToAdd.SetsNumber)
                        {
                            setService.UpdateExerciseSets(dbRoutineExercise, routineExerciseToAdd, transaction);
                        }

                        dbRoutineExercise.ExerciseId = routineExerciseToAdd.ExerciseId;
                        dbRoutineExercise.Repetitions = routineExerciseToAdd.Repetitions;
                        dbRoutineExercise.SetsNumber = routineExerciseToAdd.SetsNumber;

                        routineExercisesToUpdate.Add(dbRoutineExercise);
                    }
                }
            };

            var routineExercisesToDelete = dbRoutine.RoutineExercises
                .Where(dbExercise => !routineExercisesToAdd
                .Any(e => e.Id == dbExercise.Id))
                .ToList();

            if (newRoutineExercises.Count > 0)
            {
                transaction.InsertAll(newRoutineExercises);
                setService.CreateSets(newRoutineExercises, transaction);
            }

            if (routineExercisesToUpdate.Count > 0)
            {
                transaction.UpdateAll(routineExercisesToUpdate);
            }

            if (routineExercisesToDelete.Count > 0)
            {
                transaction.DeleteAll(routineExercisesToDelete);
            }
        }

        /// <summary>
        /// GetRoutineExerciseById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Response<RoutineExerciseAddModel>> GetRoutineExerciseById(Guid id)
        {
            try
            {
                var routineExercise = await connectionAsync.GetWithChildrenAsync<RoutineExercise>(id);
                var exercise = await connectionAsync.Table<Exercise>().FirstOrDefaultAsync(e => e.Id == routineExercise.ExerciseId);

                var routineExerciseToReturn = routineExercise.Adapt<RoutineExerciseAddModel>();

                if (exercise != null)
                {
                    routineExerciseToReturn.Name = exercise.Name;
                }


                return new Response<RoutineExerciseAddModel> 
                { 
                    IsValid = true, 
                    Data = routineExerciseToReturn 
                };
            }
            catch (Exception e)
            {
                var a = e;
                return new Response<RoutineExerciseAddModel> 
                { 
                    IsValid = false, 
                    Data = null, 
                    Message = "No se pudo cargar el ejercicio." 
                };
            }
        }

        /// <summary>
        /// GetRoutineExercisesByRoutineId
        /// </summary>
        /// <param name="routineId"></param>
        /// <returns></returns>
        public async Task<Response<List<RoutineExerciseAddModel>>> GetRoutineExercisesByRoutineId(Guid routineId)
        {
            try
            {
                var routineExercises = await connectionAsync.GetAllWithChildrenAsync<RoutineExercise>(re => re.RoutineId == routineId);

                var exercises = await connectionAsync.Table<Exercise>().ToListAsync();

                var routineExercisesToReturn = routineExercises.Adapt<List<RoutineExerciseAddModel>>();

                foreach (var routineExercise in routineExercisesToReturn)
                {
                    var exercise = exercises.SingleOrDefault(e => e.Id == routineExercise.ExerciseId);

                    if (exercise != null)
                    {
                        routineExercise.Name = exercise.Name;
                    }
                }

                return new Response<List<RoutineExerciseAddModel>> 
                { 
                    IsValid = true, 
                    Data = routineExercisesToReturn 
                };
            }
            catch (Exception)
            {
                return new Response<List<RoutineExerciseAddModel>> 
                { 
                    IsValid = false, 
                    Data = null, 
                    Message = "No se pudieron cargar los ejercicios." 
                };
            }
        }

        /// <summary>
        /// SaveWorkout
        /// </summary>
        /// <param name="exercises"></param>
        /// <returns></returns>
        public async Task<Response<BaseResponse>> SaveWorkout(List<RoutineExerciseAddModel> exercises)
        {
            try
            {
                await connectionAsync.RunInTransactionAsync((SQLiteConnection transaction) =>
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
                        var exerciseToUpdate = exercise.Adapt<RoutineExercise>();

                        foreach (var set in exerciseToUpdate.Sets)
                        {
                            transaction.Insert(set);
                        }

                        transaction.Update(exerciseToUpdate);
                    }
                });

                return new Response<BaseResponse> 
                { 
                    IsValid = true, 
                    Message = "Entrenamiento guardado.", 
                    Data = null 
                };
            }
            catch (SQLiteException)
            {
                return new Response<BaseResponse> 
                { 
                    IsValid = false, 
                    Message = "Ha ocurrido un error con la base de datos.", 
                    Data = null 
                };
            }
            catch (Exception)
            {
                return new Response<BaseResponse> 
                { 
                    IsValid = false, 
                    Message = "Ha ocurrido un error inesperado.", 
                    Data = null 
                };
            }
        }
    }
}
