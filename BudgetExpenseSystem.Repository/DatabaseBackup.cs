using System.Diagnostics;

namespace BudgetExpenseSystem.Repository
{
	public class DatabaseBackup
	{
		public static void BackupMySqlDatabase()
		{
			var backupFolder = "backup_folder";
			var databaseName = "db_name";
			var user = "user";
			var password = "password";
			var backupFile = Path.Combine(backupFolder, $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql");

			if (!Directory.Exists(backupFolder)) Directory.CreateDirectory(backupFolder);

			var command = $"mysqldump -u {user} -p{password} {databaseName} > \"{backupFile}\"";

			var processStartInfo = new ProcessStartInfo
			{
				FileName = "/bin/bash",
				Arguments = $"-c \"{command}\"",
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				CreateNoWindow = true,
				UseShellExecute = false
			};

			try
			{
				var process = new Process { StartInfo = processStartInfo };
				process.Start();
				process.WaitForExit();

				Console.WriteLine($"Backup completed: {backupFile}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error during backup: {ex.Message}");
			}
		}
	}
}
