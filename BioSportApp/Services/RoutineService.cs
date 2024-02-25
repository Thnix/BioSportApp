using BioSportApp.Common.Messaging;
using BioSportApp.Domain;
using BioSportApp.Models.Routine;
using Mapster;
using SQLite;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;


namespace BioSportApp.Services
{
    public class RoutineService(BioSportContext bioSportContext, ExerciseService exerciseService)
    {
        private readonly SQLiteAsyncConnection connection = bioSportContext.GetConnection();
        private readonly ExerciseService exerciseService = exerciseService;

        public async Task<Response<BaseResponse>> CreateRoutine(RoutineAddModel routineToCreate)
        {
            try
            {
                var routine = routineToCreate.Adapt<Routine>();

                foreach (var exercise in routine.Exercises)
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
                            SetName = $"Serie {i + 1}",
                            CurrentNumber = i + 1
                        };

                        exercise.Sets.Add(set);
                    }
                }

                await connection.InsertWithChildrenAsync(routine, recursive: true);

                return new Response<BaseResponse> { IsValid = true, Data = null };
            }
            catch (SQLiteException) 
            {
                return new Response<BaseResponse> { IsValid = false, Data = null, Message = "Ha ocurrido un error inesperado." };
            }
            catch (Exception)
            {
                return new Response<BaseResponse> { IsValid = false, Data = null, Message = "Ha ocurrido un error inesperado." };
            } 
        }

        public async Task<Response<BaseResponse>> UpdateRoutine(RoutineAddModel routineToUpdate)
        {
            try
            {
                var routine = await connection.GetWithChildrenAsync<Routine>(routineToUpdate.Id, recursive: true);

                routine.Name = routineToUpdate.Name;

                await connection.RunInTransactionAsync((SQLiteConnection transaction) =>
                {
                    exerciseService.UpdateRoutineExercises(routine, routineToUpdate.Exercises, transaction);

                    transaction.Update(routine);
                });

                return new Response<BaseResponse> { Data = null, IsValid = true };
            }
            catch (SQLiteException)
            {
                return new Response<BaseResponse> { Data = null, IsValid = false, Message = "No se pudo actualizar la rutina" };
            }
            catch (Exception)
            {
                return new Response<BaseResponse> { Data = null, IsValid = false, Message = "Ha ocurrido un error inesperado." };
            }
        }

        public async Task<Response<List<RoutineAddModel>>> GetRoutines()
        {
            try
            {
                var routineList = await connection.Table<Routine>().ToListAsync();
                return new Response<List<RoutineAddModel>> { Data = routineList.Adapt<List<RoutineAddModel>>(), IsValid = true };
            }  
            catch (SQLiteException)
            {
                return new Response<List<RoutineAddModel>> { Data = null, IsValid = false, Message = "No se pudieron cargar las rutinas." };
            }
            catch (Exception)
            {
                return new Response<List<RoutineAddModel>> { Data = null, IsValid = false, Message = "Ha ocurrido un error inesperado." };
            }
        }

        public async Task<Response<RoutineAddModel>> GetRoutineById(Guid routineId)
        {
            try
            {
                var routine = await connection.GetWithChildrenAsync<Routine>(routineId);
                return new Response<RoutineAddModel> { Data = routine.Adapt<RoutineAddModel>(), IsValid = true };
            }
            catch (SQLiteException)
            {
                return new Response<RoutineAddModel> { Data = null, IsValid = false, Message = "No se pudo cargar la rutina." };
            }
            catch (Exception)
            {
                return new Response<RoutineAddModel> { Data = null, IsValid = false, Message = "Ha ocurrido un error inesperado." };
            }
        }

        public async Task<Response<BaseResponse>> DeleteRoutineById(Guid routineId)
        {
            try
            {
                await connection.RunInTransactionAsync(transaction =>
                {
                    var routine = transaction.GetWithChildren<Routine>(routineId);
                    transaction.Delete(routine, recursive: true);
                });

                return new Response<BaseResponse> { Data = null, IsValid = true };
            }
            catch (SQLiteException)
            {
                return new Response<BaseResponse> { Data = null, IsValid = false, Message = "No se pudo eliminar la rutina." };
            }
            catch(Exception)
            {
                return new Response<BaseResponse> { Data = null, IsValid = false, Message = "Ha ocurrido un error inesperado." };
            }
        }
    }
}
