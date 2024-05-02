using BioSportApp.Common.Messaging;
using BioSportApp.Domain.Core;
using BioSportApp.Models.Routine;
using Domain.Core;
using Mapster;
using SQLite;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;

namespace BioSportApp.Services
{
    public class RoutineService(BioSportContext context, RoutineExerciseService routineExerciseService)
    {
        private readonly SQLiteAsyncConnection connectionAsync = context.GetConnectionAsync();
        //private readonly SQLiteConnection connection = context.GetConnection();

        private readonly RoutineExerciseService routineExerciseService = routineExerciseService;

        /// <summary>
        /// GetAllRoutines
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<RoutineListModel>>> GetAllRoutines()
        {
            try
            {
                var routines = await connectionAsync.Table<Routine>().ToListAsync();

                return new Response<List<RoutineListModel>> 
                { 
                    Data = routines.Adapt<List<RoutineListModel>>(), 
                    IsValid = true 
                };
            }
            catch (SQLiteException)
            {
                return new Response<List<RoutineListModel>> 
                { 
                    Data = null, 
                    IsValid = false, 
                    Message = "No se pudieron cargar las rutinas, debido a un error con la base de datos."
                };
            }
            catch (Exception)
            {
                return new Response<List<RoutineListModel>> 
                { 
                    Data = null, 
                    IsValid = false, 
                    Message = "Ha ocurrido un error desconocido." 
                };
            }
        }

        /// <summary>
        /// AddRoutine
        /// </summary>
        /// <param name="routineToAdd"></param>
        /// <returns></returns>
        public async Task<Response<BaseAddResponse>> CreateRoutine(RoutineAddModel routineToAdd)
        {
            try
            {
                var routine = routineToAdd.Adapt<Routine>();

                await connectionAsync.RunInTransactionAsync((SQLiteConnection transaction) =>
                {
                    routine.Id = Guid.NewGuid();
                    routine.CreationDate = DateTime.Now;

                    transaction.Insert(routine);
                    routineExerciseService.AddRoutineExercises(routine, transaction);
                });

                return new Response<BaseAddResponse>
                {
                    Data = new BaseAddResponse
                    {
                        Id = routine.Id,
                    },
                    IsValid = true,
                    Message = "Rutina guardada."
                };
            }
            catch (SQLiteException)
            {
                return new Response<BaseAddResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "No se pudo guardar la rutina, debido a un error con la base de datos."
                };
            }
            catch (Exception)
            {
                return new Response<BaseAddResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "Ha ocurrido un error desconocido."
                };
            }
        }

        /// <summary>
        /// UpdateRoutine
        /// </summary>
        /// <param name="routineToUpdate"></param>
        /// <returns></returns>
        public async Task<Response<BaseResponse>> UpdateRoutine(RoutineAddModel routineToUpdate)
        {
            try
            {
                var routine = await connectionAsync.GetWithChildrenAsync<Routine>(routineToUpdate.Id, recursive : true);
                routine.Name = routineToUpdate.Name;

                await connectionAsync.RunInTransactionAsync((SQLiteConnection transaction) => 
                { 
                    transaction.Update(routine);
                    routineExerciseService.UpdateRoutineExercises(routine, routineToUpdate.RoutineExercises, transaction);
                });

                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = true,
                    Message = "Rutina actualizada."
                };
            }
            catch (SQLiteException)
            {
                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "No se pudo actualizar la rutina, debido a un error con la base de datos."
                };
            }
            catch(Exception)
            {
                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "Ha ocurrido un error desconocido."
                };
            }
        }

        /// <summary>
        /// GetRoutineById
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Response<RoutineAddModel>> GetRoutineById(Guid Id)
        {
            try
            {
                var routine = await connectionAsync.GetWithChildrenAsync<Routine>(Id, recursive : true);

                return new Response<RoutineAddModel> 
                { 
                    IsValid = true,
                    Data = routine.Adapt<RoutineAddModel>()
                };
            }
            catch (SQLiteException)
            {
                return new Response<RoutineAddModel>
                {
                    IsValid = false,
                    Data = null,
                    Message = "No se pudo cargar la rutina debido a un error con la base de datos."
                };
            }
            catch (Exception)
            {
                return new Response<RoutineAddModel>
                {
                    IsValid = false,
                    Data = null,
                    Message = "No se pudo cargar la rutina debido a un error desconocido."
                };
            }
        }

        /// <summary>
        /// GetRoutineViewById
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Response<RoutineAddModel>> GetRoutineViewById(Guid Id)
        {
            try
            {
                var routine = await connectionAsync.GetWithChildrenAsync<Routine>(Id, recursive: true);

                var routineToReturn = routine.Adapt<RoutineAddModel>();

                foreach (var routineExercise in routineToReturn.RoutineExercises)
                {
                    var exercise = await connectionAsync.GetAsync<Exercise>(e => e.Id == routineExercise.ExerciseId);
                    routineExercise.Name = exercise.Name;
                }

                return new Response<RoutineAddModel>
                {
                    IsValid = true,
                    Data = routineToReturn
                };
            }
            catch (SQLiteException)
            {
                return new Response<RoutineAddModel>
                {
                    IsValid = false,
                    Data = null,
                    Message = "No se pudo cargar la rutina debido a un error con la base de datos."
                };
            }
            catch (Exception)
            {
                return new Response<RoutineAddModel>
                {
                    IsValid = false,
                    Data = null,
                    Message = "No se pudo cargar la rutina debido a un error desconocido."
                };
            }
        }

        /// <summary>
        /// DeleteRoutineById
        /// </summary>
        /// <param name="routineId"></param>
        /// <returns></returns>
        public async Task<Response<BaseResponse>> DeleteRoutineById(Guid routineId)
        {
            try
            {
                await connectionAsync.RunInTransactionAsync(transaction =>
                {
                    var routine = transaction.GetWithChildren<Routine>(routineId);
                    transaction.Delete(routine, recursive: true);
                });

                return new Response<BaseResponse> 
                { 
                    Data = null, 
                    IsValid = true 
                };
            }
            catch (SQLiteException)
            {
                return new Response<BaseResponse> 
                { 
                    Data = null, 
                    IsValid = false, 
                    Message = "No se pudo eliminar la rutina." 
                };
            }
            catch (Exception)
            {
                return new Response<BaseResponse> 
                { 
                    Data = null, 
                    IsValid = false, 
                    Message = "Ha ocurrido un error inesperado." 
                };
            }
        }
    }
}
