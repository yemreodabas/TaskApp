using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Persistence.Dapper
{
	public abstract class BaseSqliteRepository
	{
		// protected string _connectionString = @"Server=localhost;Database=DapperDemo.sqlite;Trusted_Connection=true;";

		private static string _dbFilePath = "C:\\Workspace\\github\\TaskApp\\TaskApp.sqlite";

		private static bool IsInitialized = false;

		protected SQLiteConnection OpenConnection()
		{
			if (!IsInitialized)
			{
				if (!File.Exists(_dbFilePath))
				{
					SQLiteConnection.CreateFile(_dbFilePath);
				}
			}

			var conn = new SQLiteConnection($"Data Source={_dbFilePath};Version=3;Read Only=False;");
			conn.Open();

			if (!IsInitialized)
			{
				this.EnsureDatabaseIsInitialized(conn);
				IsInitialized = true;
			}

			return conn;
		}

		private void EnsureDatabaseIsInitialized(SQLiteConnection conn)
		{
			string sql = null;
			SQLiteCommand command = null;
			int count = 0;

			sql = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'User'";
			command = new SQLiteCommand(sql, conn);
			count = Convert.ToInt32(command.ExecuteScalar());
			if (count == 0)
			{
				sql = "create table User (" +
							"Id INTEGER PRIMARY KEY, " +
							"Username TEXT NOT NULL, " +
							"Email TEXT NOT NULL, " +
							"Password TEXT NOT NULL, " +
							"BirthYear INTEGER NOT NULL " +
						")";

				command = new SQLiteCommand(sql, conn);
				command.ExecuteNonQuery();
			}

			sql = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'Mission'";
			command = new SQLiteCommand(sql, conn);
			count = Convert.ToInt32(command.ExecuteScalar());
			if (count == 0)
			{
				sql = "create table Mission (" +
							"Id INTEGER PRIMARY KEY, " +
							"Name TEXT NOT NULL, " +
							"UserId INTEGER NOT NULL " +
						")";

				command = new SQLiteCommand(sql, conn);
				command.ExecuteNonQuery();
			}

			sql = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'DirectMessage'";
			command = new SQLiteCommand(sql, conn);
			count = Convert.ToInt32(command.ExecuteScalar());
			if (count == 0)
			{
				sql = "create table DirectMessage (" +
							"Id INTEGER PRIMARY KEY, " +
							"Message TEXT NOT NULL, " +
							"SenderId INTEGER NOT NULL, " +
							"ReceiverId INTEGER NOT NULL " +
						")";

				command = new SQLiteCommand(sql, conn);
				command.ExecuteNonQuery();
			}

			sql = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'Operation'";
			command = new SQLiteCommand(sql, conn);
			count = Convert.ToInt32(command.ExecuteScalar());
			if (count == 0)
			{
				sql = "create table Operation (" +
							"Id INTEGER PRIMARY KEY, " +
							"Name TEXT NOT NULL, " +
							"MissionId INTEGER NOT NULL, " +
							"OperationStatus INTEGER NOT NULL" +
						")";

				command = new SQLiteCommand(sql, conn);
				command.ExecuteNonQuery();
			}

			sql = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'Follow'";
			command = new SQLiteCommand(sql, conn);
			count = Convert.ToInt32(command.ExecuteScalar());
			if (count == 0)
			{
				sql = "create table Follow (" +
							"FollowerUserId INTEGER NOT NULL, " +
							"TargetUserId INTEGER NOT NULL" +
						")";

				command = new SQLiteCommand(sql, conn);
				command.ExecuteNonQuery();
			}

			sql = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'Forum'";
			command = new SQLiteCommand(sql, conn);
			count = Convert.ToInt32(command.ExecuteScalar());
			if (count == 0)
			{
				sql = "create table Forum (" +
							"Id INTEGER PRIMARY KEY, " +
							"Post TEXT NOT NULL, " +
							"UserId INTEGER NOT NULL " +
						")";

				command = new SQLiteCommand(sql, conn);
				command.ExecuteNonQuery();
			}

			sql = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'Log'";
			command = new SQLiteCommand(sql, conn);
			count = Convert.ToInt32(command.ExecuteScalar());
			if (count == 0)
			{
				sql = "create table Log (" +
							"Id INTEGER PRIMARY KEY, " +
							"Type INTEGER NOT NULL, " +
							"Message TEXT, " +
							"Timestamp INTEGER" +
						")";

				command = new SQLiteCommand(sql, conn);
				command.ExecuteNonQuery();
			}
		}
	}
}
