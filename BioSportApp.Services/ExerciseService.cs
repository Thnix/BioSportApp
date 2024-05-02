using BioSportApp.Common.ExtensionMethods;
using BioSportApp.Common.Messaging;
using BioSportApp.Domain.Core;
using BioSportApp.Models.Exercise;
using Domain.Core;
using Mapster;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace BioSportApp.Services
{
    public class ExerciseService(BioSportContext context)
    {
        private readonly SQLiteAsyncConnection connection = context.GetConnectionAsync();

        public async Task<Response<List<ExerciseListModel>>> GetAllExercises()
        {
            try
            {
                var exercises = await connection.Table<Exercise>().ToListAsync();

                return new Response<List<ExerciseListModel>> 
                { 
                    Data = exercises.Adapt<List<ExerciseListModel>>(), 
                    IsValid = true 
                };
            }
            catch (SQLiteException)
            {
                return new Response<List<ExerciseListModel>> 
                { 
                    Data = null, 
                    IsValid = false, 
                    Message = "No se pudieron recuperar los ejercicios, debido a un error en la base de datos." 
                };
            }
            catch (Exception)
            {
                return new Response<List<ExerciseListModel>> 
                { 
                    Data = null, 
                    IsValid = false, 
                    Message = "Ha ocurrido un error desconocido." 
                };
            }
        }

        public async Task<Response<List<ExerciseListModel>>> LoadExercisesWhileScrolling(int currentPage, int itemsAmount)
        {
            try
            {
                int startIndex = (currentPage - 1) * itemsAmount;

                var query = await connection.Table<Exercise>()
                   .Skip(startIndex)
                   .Take(itemsAmount)
                   .ToListAsync();

                return new Response<List<ExerciseListModel>>
                {
                    Data = query.Adapt<List<ExerciseListModel>>(),
                    IsValid = true
                };
            }
            catch (SQLiteException)
            {
                return new Response<List<ExerciseListModel>>
                {
                    Data = null,
                    IsValid = false,
                    Message = "No se pudieron recuperar los ejercicios, debido a un error en la base de datos."
                };
            }
            catch (Exception)
            {
                return new Response<List<ExerciseListModel>>
                {
                    Data = null,
                    IsValid = false,
                    Message = "Ha ocurrido un error desconocido."
                };
            }
        }

        public async Task<Response<List<ExerciseListModel>>> SearchExercises(string search)
        {
            try
            {
                var query = await connection.Table<Exercise>()
                 .ToListAsync();

                var exercises = query
                     .Where(e => e.Name.ToLower().RemoveDiacritics().Contains(search.ToLower().RemoveDiacritics()))
                     .ToList();

                return new Response<List<ExerciseListModel>>
                {
                    Data = exercises.Adapt<List<ExerciseListModel>>(),
                    IsValid = true
                };
            }
            catch(SQLiteException) 
            {
                return new Response<List<ExerciseListModel>>
                {
                    Data = null,
                    IsValid = false,
                    Message = "No se pudieron recuperar los ejercicios, debido a un error en la base de datos."
                };
            }
            catch (Exception)
            {
                return new Response<List<ExerciseListModel>>
                {
                    Data = null,
                    IsValid = false,
                    Message = "Ha ocurrido un error desconocido."
                };
            }
        }

        public async Task<Response<BaseResponse>> AddChestExercises()
        {
            try
            {
                var existingExercises = await connection.GetAllWithChildrenAsync<Exercise>();

                var chestExercisesExist = existingExercises
                    .Any(e => e.Category.Code == "CHEST");

                if (!chestExercisesExist)
                {
                    var category = await connection.Table<Category>().FirstOrDefaultAsync(c => c.Code == "CHEST");

                    List<Exercise> chestExercisesList = [
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de pecho en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de pecho declinado en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Pullover en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press martillo en banco inclinado con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press martillo con mancuernas en banco plano",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de banca declinado alternado con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de banca declinado con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de banca inclinado con poleas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de banca inclinado con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de banca con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press alternado inclinado con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press alternado con mancuernas en banco plano",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones inclinadas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Fondos con dos sillas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Fondos con una silla",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Fondos de pecho en máquina asistida",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones Superman",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexión de brazos contra la pared",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones con bosu ball",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones con toque de pecho",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones con banda de resistencia",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones sobre balón medicinal",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones a una mano",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones abiertas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Aperturas en banco declinado (agarre supino)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Aperturas en banco plano con polea",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Aperturas declinado en polea",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Squeeze Bench Press",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Landmine Squeeze Press de rodillas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Squeeze Press inclinado con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Vuelos a una mano con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de banca plano con poleas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de banca con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Fondos de pecho",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones de brazo con aplauso",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press declinado con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Aperturas pec deck de pecho",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de banca inclinado con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Aperturas con mancuernas en banco inclinado",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Aperturas con mancuernas en banco plano",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Pullover con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Cruces en polea baja",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Cruces en polea alta o crossover para pectorales",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Aperturas en contractor de pecho",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                    ];

                    await connection.InsertAllAsync(chestExercisesList);
                }

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
                    Message = "Ha ocurrido un error con la base de datos al agregar los ejercicios para Pecho."
                };
            }
            catch (Exception)
            {
                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "Ha ocurrido un error desconocido al agregar los ejercicios para Pecho."
                };
            }
        }

        public async Task<Response<BaseResponse>> AddAbdomenExercises()
        {
            try
            {
                var existingExercises = await connection.GetAllWithChildrenAsync<Exercise>();

                var abdomenExercisesExist = existingExercises.Any(e => e.Category.Code == "ABDOMEN");

                if (!abdomenExercisesExist)
                {
                    var category = await connection.Table<Category>().FirstOrDefaultAsync(c => c.Code == "ABDOMEN");

                    List<Exercise> abdomenExercisesList = [
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Rueda abdominal de rodillas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                      
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Rueda abdominal de pie",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Rotación del tronco en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Plancha lateral",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Plancha de rodillas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Plancha a una pierna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Inchworm",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Hollow Hold",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Groin Crunch",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Giro ruso con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Giro ruso",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Dead Bug",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Abdominales sobre pelota de yoga",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Crunch sentado en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Crunch de rana",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Crunch de pie en polea (agarre cuerda)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Crunch de oblicuos en el suelo",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Crunch (brazos rectos por encima de la cabeza)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Criss Cross Legs",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Cocoons",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Air Bike en el suelo",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Air Bike (de pie)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "8 Leg Crunch",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Toque de talones",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Twist Crunch con piernas elevadas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Twisting Crunch",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "V-Up con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                      
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Plank o Plancha",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Abdominales cruzados",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Bottoms Up",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Crunch en polea alta",
                            CategoryId = category.Id,
                            Description = "Description"
                        }
                    ];

                    await connection.InsertAllAsync(abdomenExercisesList);
                }

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
                    Message = "Ha ocurrido un error con la base de datos al agregar los ejercicios para el Abdomen."
                };
            }
            catch (Exception)
            {
                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "Ha ocurrido un error desconocido al agregar los ejercicios para el Abdomen."
                };
            }
        }

        public async Task<Response<BaseResponse>> AddShouldersExercises()
        {
            try
            {
                var existingExercises = await connection.GetAllWithChildrenAsync<Exercise>();

                var shouldersExercisesExist = existingExercises.Any(e => e.Category.Code == "SHOULDERS");

                if (!shouldersExercisesExist)
                {
                    var category = await connection.Table<Category>().FirstOrDefaultAsync(c => c.Code == "SHOULDERS");

                    List<Exercise> shouldersExercisesList = [
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Rotación interna de hombro en polea",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Rotación externa de hombro en polea",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Rotación externa de hombro con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo de deltoides posterior con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press militar de pie en máquina Smith",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press militar con barra (agarre abierto)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de hombros sentado con mancuernas (agarre neutro)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de hombros en polea",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de hombros alternado con mancuernas sentado",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de hombros alternado con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de hombros tras nuca en máquina Smith",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press Arnold con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Pike Push-Up",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Hang Power Clean con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        }, 
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Face Pull a una mano en polea (con cuerda)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Clean and Press con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Clean and jerk con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Clean and jerk a una mano con pesa rusa",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Brazos en círculos",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Aperturas posteriores en polea",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Landmine Squat and Press",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Swing con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Vuelos posteriores en banco inclinado con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo a la cara con cuerda en polea alta",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Elevaciones laterales en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de hombros en máquina Smith",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Aperturas invertidas en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de hombros sentado en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        }, 
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Elevaciones frontales con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Elevaciones laterales en polea baja",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press cubano con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press tras nuca con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Elevaciones posteriores con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Elevaciones laterales con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Elevación frontal con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo con barra en banco inclinado",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Elevaciones frontales con polea baja a una mano",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press militar con mancuernas o barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo al cuello con polea baja",
                            CategoryId = category.Id,
                            Description = "Description"
                        }
                    ];

                    await connection.InsertAllAsync(shouldersExercisesList);
                }

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
                    Message = "Ha ocurrido un error con la base de datos al agregar los ejercicios para Hombros."
                };
            }
            catch (Exception)
            {
                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "Ha ocurrido un error desconocido al agregar los ejercicios para Hombros."
                };
            }
        }

        public async Task<Response<BaseResponse>> AddLegsExercises()
        {
            try
            {
                var existingExercises = await connection.GetAllWithChildrenAsync<Exercise>();

                var legsExercisesExist = existingExercises.Any(e => e.Category.Code == "LEGS");

                if (!legsExercisesExist)
                {
                    var category = await connection.Table<Category>().FirstOrDefaultAsync(c => c.Code == "LEGS");

                    List<Exercise> legsExercisesList = [
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla sobre Bosu",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla sumo (sin equipo)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla sumo en máquina Smith",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla sumo con pesa rusa",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla sumo con barra landmine",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla split en máquina Smith",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla split con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla split con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla silla en máquina Smith",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla lateral con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla frontal con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla de reverencia",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla de pistola",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla en máquina péndulo",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla de patinador",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadillas con salto",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadillas con banda elástica",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla acostado en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de pierna horizontal sentado en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Farmer Carry con barra trampa",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión a una pierna en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Estocada frontal con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Estocada con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl de pierna de rodillas en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl de pierna de pie en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl a una pierna sentado en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl a una pierna tumbado en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Cossack Squat",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sissy Squat",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Step-Up con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Step-Up sosteniendo pesas rusas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Step-Up en silla",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Thruster con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Zancada inversa (sin equipo)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Zancadas caminando (sin equipo)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Zancadas con mancuernas caminando",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press a una pierna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de pierna posición abierta",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de piernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla Hack Squat",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadillas con peso corporal",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Peso muerto rumano con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Peso muerto rumano con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla frontal con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla sumo con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de piernas en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Ejercicio buenos días o Good Morning",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexión rodilla sentado en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexión rodilla acostado en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Patada de glúteo en polea baja",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Aductores en polea baja",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Aductores en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                     ];

                    await connection.InsertAllAsync(legsExercisesList);
                }

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
                    Message = "Ha ocurrido un error con la base de datos al agregar los ejercicios para Piernas."
                };
            }
            catch (Exception)
            {
                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "Ha ocurrido un error desconocido al agregar los ejercicios para Piernas."
                };
            }
        }

        public async Task<Response<BaseResponse>> AddButtocksExercises()
        {
            try
            {
                var existingExercises = await connection.GetAllWithChildrenAsync<Exercise>();

                var buttocksExercisesExist = existingExercises.Any(e => e.Category.Code == "BUTTOCKS");

                if (!buttocksExercisesExist)
                {
                    var category = await connection.Table<Category>().FirstOrDefaultAsync(c => c.Code == "BUTTOCKS");

                    List<Exercise> buttocksExercisesList = [
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Puente de glúteos con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Puente de glúteos a una pierna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Puente de glúteos con banda de resistencia",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Puente de glúteos a una pierna con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Puente de abducción de cadera",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla búlgara (sin equipo)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla búlgara en máquina Smith",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla búlgara con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sentadilla búlgara con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Hip Thrust con banda de resistencia",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Hip Thrust a una pierna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Hip Swirls",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Hiperextensión sosteniendo disco",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Hip thrust con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                      
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Hip thrust con máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de cadera en polea baja",
                            CategoryId = category.Id,
                            Description = "Description"
                        }                       
                     ];

                    await connection.InsertAllAsync(buttocksExercisesList);
                }

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
                    Message = "Ha ocurrido un error con la base de datos al agregar los ejercicios para Glúteos."
                };
            }
            catch (Exception)
            {
                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "Ha ocurrido un error desconocido al agregar los ejercicios para Glúteos."
                };
            }
        }

        public async Task<Response<BaseResponse>> AddBackExercises()
        {
            try
            {
                var existingExercises = await connection.GetAllWithChildrenAsync<Exercise>();

                var backExercisesExist = existingExercises.Any(e => e.Category.Code == "BACK");

                if (!backExercisesExist)
                {
                    var category = await connection.Table<Category>().FirstOrDefaultAsync(c => c.Code == "BACK");

                    List<Exercise> backExercisesList = [
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo supino con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo supino en polea baja (de pie)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo sentado en polea (agarre cuerda)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo sentado con banda elástica (agarre neutro)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo sentado en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo renegado con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo invertido agarre supino",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo inclinado en banco con mancuernas (agarre prono)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo en polea agarre supino",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo sentado en polea con agarre abierto",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo en banco inclinado con mancuernas (agarre neutro)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo de pie con polea agarre prono cerrado",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo con mancuernas (ambas manos)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo con barra Z (agarre supino)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo a una mano en polea baja",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo con barra landmine (agarre V)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo con banda elástica (de pie)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo al cuello con banda elástica",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo a una mano con barra landmine",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Jalón al pecho tras nuca",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Jalón al pecho a una mano sentado en polea",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Encogimiento de hombros en polea",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Encogimiento de hombros en máquina Smith (agarre abierto)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Encogimiento de hombros en máquina de discos",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo con mancuerna (unilateral)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Superman con giro",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo sentado en polea baja (agarre V)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Jalón al pecho agarre abierto",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Dominadas agarre neutro",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Pull-ups o dominadas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Peso muerto rumano con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Encogimiento de hombros con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Encogimiento de hombros con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo con barra en banco inclinado",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo con barra recta",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Pullover con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Jalón al pecho en polea (agarre semi-abierto)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo en barra T (agarre prono)",
                            CategoryId = category.Id,
                            Description = "Description"
                        }
                    ];

                    await connection.InsertAllAsync(backExercisesList);
                }

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
                    Message = "Ha ocurrido un error con la base de datos al agregar los ejercicios para la Espalda."
                };
            }
            catch (Exception)
            {
                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "Ha ocurrido un error desconocido al agregar los ejercicios para Espalda."
                };
            }       
        }

        public async Task<Response<BaseResponse>> AddBicepsExercises()
        {
            try
            {
                var existingExercises = await connection.GetAllWithChildrenAsync<Exercise>();

                var bicepsExercisesExist = existingExercises.Any(e => e.Category.Code == "BICEPS");

                if (!bicepsExercisesExist)
                {
                    var category = await connection.Table<Category>().FirstOrDefaultAsync(c => c.Code == "BICEPS");

                    List<Exercise> bicepsExercisesList = [
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl predicador con polea",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl predicador a una mano con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl martillo en polea (con cuerda)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl de bíceps sentado en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl concentrado con mancuerna agarre prono",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl con barra Z",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl con barra Z (agarre abierto)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl alternado con banda elastica",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl agarre prono con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl a un brazo en polea",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl de martillo cruzado con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl concentrado en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl en polea baja con barra recta",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl predicador con mancuernas (agarre supino)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl de bíceps con banda elástica",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl Zottman con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl invertido con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl bíceps inclinado con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Remo con barra en banco inclinado",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Bíceps, brazos en cruz en polea alta",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl de bíceps en banco predicador",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl de bíceps con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl martillo con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl concentrado con mancuerna (agarre supino)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Curl alternado con mancuerna",
                            CategoryId = category.Id,
                            Description = "Description"
                        }
                     ];

                    await connection.InsertAllAsync(bicepsExercisesList);
                }

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
                    Message = "Ha ocurrido un error con la base de datos al agregar los ejercicios para Bíceps."
                };
            }
            catch (Exception)
            {
                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "Ha ocurrido un error desconocido al agregar los ejercicios para Bíceps."
                };
            }
        }

        public async Task<Response<BaseResponse>> AddTricepsExercises()
        {
            try
            {
                var existingExercises = await connection.GetAllWithChildrenAsync<Exercise>();

                var tricepsExercisesExist = existingExercises.Any(e => e.Category.Code == "TRICEPS");

                if (!tricepsExercisesExist)
                {
                    var category = await connection.Table<Category>().FirstOrDefaultAsync(c => c.Code == "TRICEPS");

                    List<Exercise> tricepsExercisesList = [
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press francés con polea baja",
                            CategoryId= category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de copa con mancuerna de pie",
                            CategoryId= category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press cerrado en banco inclinado",
                            CategoryId= category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Patada de tríceps con banda elástica",
                            CategoryId= category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Patada de tríceps en polea a una mano",
                            CategoryId= category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Patada de tríceps con mancuerna",
                            CategoryId= category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones sobre los antebrazos",
                            CategoryId= category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones Diamante",
                            CategoryId= category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones de diamante de rodillas",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps sobre la cabeza con polea",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps sobre la cabeza en polea de rodillas",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps en polea (agarre supino)",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps sobre la cabeza con banda elastica",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps en máquina de apalancamiento",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps concentrado en máquina",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Rompecráneos con mancuernas",
                            CategoryId= category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensiones de tríceps con agarre en V en polea",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps acostado con polea baja",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps acostado con mancuerna",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps a una mano en polea (agarre cuerda)",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps a una mano con polea agarre supino",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps a una mano con polea (agarre prono)",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de triceps con mancuernas acostado",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones de brazo cerradas",
                            CategoryId= category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps a un brazo en polea baja",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Flexiones de brazo con aplauso",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press cerrado para triceps",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de banca con agarre cerrado",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión en polea con cuerda sobre la cabeza",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión mancuerna tras nuca con una mano (de pie)",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Fondos en banco plano",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps tras nuca con mancuerna (sentado)",
                            CategoryId= category.Id,
                            Description = "Description"
                        },                        
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Extensión de tríceps en polea alta",
                            CategoryId= category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press francés en banco plano",
                            CategoryId= category.Id,
                            Description = "Description"
                        } 
                    ];

                    await connection.InsertAllAsync(tricepsExercisesList);
                }

                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = true
                };
            }
            catch(SQLiteException)
            {
                return new Response<BaseResponse> 
                { 
                    Data = null, 
                    IsValid = false, 
                    Message = "Ha ocurrido un error con la base de datos al agregar los ejercicios para Tríceps." 
                };
            }
            catch (Exception)
            {
                return new Response<BaseResponse> 
                { 
                    Data = null, 
                    IsValid = false, 
                    Message = "Ha ocurrido un error desconocido al agregar los ejercicios para Tríceps." 
                };
            }
        }

        public async Task<Response<BaseResponse>> AddCalvesExercises()
        {
            try
            {
                var existingExercises = await connection.GetAllWithChildrenAsync<Exercise>();

                var calvesExercisesExist = existingExercises.Any(e => e.Category.Code == "CALVES");

                if (!calvesExercisesExist)
                {
                    var category = await connection.Table<Category>().FirstOrDefaultAsync(c => c.Code == "CALVES");

                    List<Exercise> calvesExerciseList = [
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Levantamiento de pantorrillas en máquina Smith",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Levantamiento de pantorrillas con barra",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Levantamiento de pantorrilla a una pierna (sin equipo)",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Elevación de pantorrilla a una pierna con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Elevación de pantorrillas sentado con mancuernas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Donkey Calf Raises",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Press de pantorrillas",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Elevación de talones en prensa",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Elevación de talones de pie en máquina",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                        new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Elevación de talones sentado",
                            CategoryId = category.Id,
                            Description = "Description"
                        },
                    ];

                    await connection.InsertAllAsync(calvesExerciseList);
                }

                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = true
                };
            }
            catch(SQLiteException)
            {
                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "Ha ocurrido un error con la base de datos al agregar los ejercicios para Pantorrillas."
                };
            }
            catch (Exception)
            {
                return new Response<BaseResponse>
                {
                    Data = null,
                    IsValid = false,
                    Message = "Ha ocurrido un error desconocido al agregar los ejercicios para Pantorrillas."
                };
            } 
        }
    }
}
