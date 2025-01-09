using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Runtime.CompilerServices;

namespace BudgetExpenseSystem.Repository
{
    public class DatabaseBackup
    {
        private readonly DbContext _context;
        private readonly string _databaseName;

        public DatabaseBackup(DbContext context)
        {
            _context = context;
            _databaseName = GetDatabaseName();
            LogConnectionString();
        }

        private void LogConnectionString()
        {
            var connectionString = _context.Database.GetDbConnection().ConnectionString;
            Console.WriteLine($"Using connection string: {connectionString}");
        }

        private string GetDatabaseName()
        {
            var connectionString = _context.Database.GetDbConnection().ConnectionString;
            var builder = new MySqlConnector.MySqlConnectionStringBuilder(connectionString);
            return builder.Database;
        }

        public async Task BackupDatabaseToFileAsync()
        {
            var backupFolder = "../DbBackup";
            string backupFile = Path.Combine(backupFolder, $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql");
            var sb = new StringBuilder();
            var databaseName = _databaseName;

            var tables = await GetTableNamesAsync();

            if (!Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }

            foreach (var table in tables)
            {
                sb.AppendLine($"-- Table: {databaseName}.{table}");

                var columnNames = await GetColumnNamesAsync(table);
                sb.AppendLine($"CREATE TABLE IF NOT EXISTS {databaseName}.{table} ({string.Join(", ", columnNames)});");

                var data = await GetTableDataAsync(table);

                foreach (var row in data)
                {
                    var values = string.Join(", ", row.Select(value => value is string ? $"\"{value}\"" : value.ToString()));
                    sb.AppendLine($"INSERT INTO {databaseName}.{table} ({string.Join(", ", columnNames)}) VALUES ({values});");
                }
            }

            await File.WriteAllTextAsync(backupFile, sb.ToString());
            File.SetAttributes(backupFile, FileAttributes.Normal);
            Console.WriteLine($"Backup completed: {backupFile}");
        }

        private async Task<List<string>> GetTableNamesAsync()
        {
            var query = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{_databaseName}' AND TABLE_TYPE = 'BASE TABLE'";
            var formattableQuery = FormattableStringFactory.Create(query);

            var tableNames = await _context.Database
                .SqlQuery<string>(formattableQuery)
                .ToListAsync();

            return tableNames;
        }

        private async Task<List<string>> GetColumnNamesAsync(string tableName)
        {
            var query = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
            var formattableQuery = FormattableStringFactory.Create(query);

            var columnNames = await _context.Database
                .SqlQuery<string>(formattableQuery)
                .ToListAsync();

            return columnNames;
        }

        private async Task<List<List<object>>> GetTableDataAsync(string tableName)
        {
            var query = $"SELECT * FROM `{tableName}`";
            var rows = new List<List<object>>();

            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    _context.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new List<object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader.GetValue(i));
                            }
                            rows.Add(row);
                        }
                    }

                    _context.Database.CloseConnection();
                }
            }
            catch (MySqlConnector.MySqlException ex) when (ex.Message.Contains("doesn't exist"))
            {
                Console.WriteLine($"Table '{tableName}' does not exist. Skipping...");
            }

            return rows;
        }
    }
}
