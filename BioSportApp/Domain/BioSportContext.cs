using SQLite;

namespace BioSportApp.Domain
{
    public class BioSportContext
    {
        private const string dbName = "bioSportLocalDb";
        private readonly SQLiteAsyncConnection connection;

        public BioSportContext()
        {
            connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, dbName));
        }

        public async Task CreateDbTables()
        {
            await connection.CreateTableAsync<Routine>();
            await connection.CreateTableAsync<Exercise>();
            await connection.CreateTableAsync<Set>();
            await connection.CreateTableAsync<WeightRecord>();
        }

        public SQLiteAsyncConnection GetConnection()
        {
            return connection;
        }
    }
}
