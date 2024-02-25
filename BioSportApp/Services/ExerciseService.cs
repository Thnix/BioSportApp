using BioSportApp.Common.Messaging;
using BioSportApp.Domain;
using BioSportApp.Models.Exercise;
using BioSportApp.Models.Set;
using Mapster;
using Microsoft.Maui.Controls;
using SQLite;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;
using SQLitePCL;
using System.Collections.ObjectModel;

namespace BioSportApp.Services
{
    public class ExerciseService
    {
        private readonly BioSportContext bioSportContext;
        private readonly SQLiteAsyncConnection connection;
        private readonly SetService setService;

        public ExerciseService(BioSportContext bioSportContext, SetService setService) 
        { 
            this.bioSportContext = bioSportContext;
            this.setService = setService;
            connection = bioSportContext.GetConnection();
        }

        public async Task<Response<ExerciseAddModel>> GetExerciseById(Guid id)
        {
            try
            {
                var exercise = await connection.GetWithChildrenAsync<Exercise>(id);
                return new Response<ExerciseAddModel> { IsValid = true, Data = exercise.Adapt<ExerciseAddModel>() };
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


        public void UpdateRoutineExercises(Routine dbRoutine, ObservableCollection<ExerciseAddModel> exercisesToAdd, SQLiteConnection transaction)
        {
            List<Exercise> newExercises = [];
            List<Exercise> exercisesToUpdate = [];

            foreach (var exerciseToAdd in exercisesToAdd)
            {
                var dbExercise = dbRoutine.Exercises.FirstOrDefault(e => e.Id == exerciseToAdd.Id);

                if (dbExercise == null)
                {
                    exerciseToAdd.RoutineId = dbRoutine.Id;
                    newExercises.Add(exerciseToAdd.Adapt<Exercise>());
                }
                else
                {
                    if (dbExercise.Name != exerciseToAdd.Name ||
                        dbExercise.Repetitions != exerciseToAdd.Repetitions ||
                        dbExercise.SetsNumber != exerciseToAdd.SetsNumber)
                    {
                        if (dbExercise.SetsNumber != exerciseToAdd.SetsNumber)
                        {
                            setService.UpdateExerciseSets(dbExercise, exerciseToAdd, transaction);
                        }

                        dbExercise.Name = exerciseToAdd.Name;
                        dbExercise.Repetitions = exerciseToAdd.Repetitions;
                        dbExercise.SetsNumber = exerciseToAdd.SetsNumber;

                        exercisesToUpdate.Add(dbExercise);
                    }
                }
            };

            var exercisesToDelete = dbRoutine.Exercises
                .Where(dbExercise => !exercisesToAdd
                .Any(e => e.Id == dbExercise.Id))
                .ToList();

            if (newExercises.Count > 0)
            {
                transaction.InsertAll(newExercises);
            }

            if (exercisesToUpdate.Count > 0)
            {
                transaction.UpdateAll(exercisesToUpdate);
            }

            if (exercisesToDelete.Count > 0)
            {
                transaction.DeleteAll(exercisesToDelete);
            }
        }
    }
}
