using BioSportApp.Common.Messaging;
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

        public async Task<Response<ExerciseAddModel>> GetExerciseById(Guid id)
        {
            try
            {
                var a = 1;
                var b = 0;
                var c = a / b;


                var exercise = await connection.GetWithChildrenAsync<Exercise>(id);
                return new Response<ExerciseAddModel> {  IsValid= true, Data = exercise.Adapt<ExerciseAddModel>() };
            }
            catch(Exception)
            {
                return new Response<ExerciseAddModel> { IsValid = false, Data = null, Message = "No se pudo cargar el ejercicio." };
            }
        }

        public async Task<Response<List<ExerciseAddModel>>> GetExercisesByRoutineId(Guid routineId)
        {
            try
            {
                var routineExercises = await connection.Table<Exercise>().Where(e => e.RoutineId == routineId).ToListAsync();
                return new Response<List<ExerciseAddModel>> { IsValid = true, Data = routineExercises.Adapt<List<ExerciseAddModel>>() };
            }
            catch (Exception)
            {
                return new Response<List<ExerciseAddModel>> { IsValid = false, Data = null, Message = "No se pudieron cargar los ejercicios." };
            }
        }

        public async Task<Response<BaseResponse>> SaveWorkout(ObservableCollection<ExerciseAddModel> exercises)
        {
            try
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

                //var a = 1;
                //var b = 0;
                //var c = a / b;

                return new Response<BaseResponse> { IsValid = true, Message = "Entrenamiento guardado.", Data = null };
            }
            catch (SQLiteException)
            {
                return new Response<BaseResponse> { IsValid = false, Message = "Ha ocurrido un error con la base de datos.", Data = null };
            }
            catch (Exception)
            {
                return new Response<BaseResponse> { IsValid = false, Message = "Ha ocurrido un error inesperado.", Data = null };
            }
        }
    }
}
