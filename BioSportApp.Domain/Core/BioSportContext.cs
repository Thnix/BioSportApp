using BioSportApp.Domain.Core;
using SQLite;

namespace Domain.Core
{
    public class BioSportContext(string dbPath)
    {
        private readonly SQLiteAsyncConnection connectionAsync = new(Path.Combine(dbPath));
        private readonly SQLiteConnection connection = new(Path.Combine(dbPath));


        public async Task CreateDbTables()
        {
            await connectionAsync.CreateTableAsync<User>();
            await connectionAsync.CreateTableAsync<Category>();
            await connectionAsync.CreateTableAsync<Exercise>();
            await connectionAsync.CreateTableAsync<Routine>();
            await connectionAsync.CreateTableAsync<RoutineExercise>();
            await connectionAsync.CreateTableAsync<Set>();
        }

        public SQLiteConnection GetConnection()
        {
            return connection;
        }

        public SQLiteAsyncConnection GetConnectionAsync()
        {
            return connectionAsync;
        }
    }
}
