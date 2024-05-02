using BioSportApp.Domain.Core;
using Domain.Core;
using SQLite;

namespace BioSportApp.Services
{
    public class CategoryService(BioSportContext context)
    {
        private readonly SQLiteAsyncConnection connection = context.GetConnectionAsync();

        public async Task AddCategories()
        {
            var existingCategories = await connection.Table<Category>().CountAsync();

            if(existingCategories == 0)
            {
                List<Category> categories = 
                  [
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Pantorillas",
                        Code = "CALVES"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Piernas",
                        Code = "LEGS"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Glúteo",
                        Code = "BUTTOCKS"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Pecho",
                        Code = "CHEST"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Espalda",
                        Code = "BACK"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Bíceps",
                        Code = "BICEPS"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Tríceps",
                        Code = "TRICEPS"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Hombros",
                        Code = "SHOULDERS"
                    },
                    new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Abdomen",
                        Code = "ABDOMEN"
                    },
                ];

                await connection.InsertAllAsync(categories);
            }  
        }
    }
}
