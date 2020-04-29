using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence.Dapper
{
	public class MissionRepository : BaseSqliteRepository, IMissionRepository
	{
		public void Insert(Mission mission)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("INSERT INTO Mission (Name, UserId) VALUES(@Name, @UserId)", mission);
				mission.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
			}
		}

		public IEnumerable<MissionModel> GetAll()
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<MissionModel>("SELECT * FROM Mission");
			}
		}

		public MissionModel GetById(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<MissionModel>("SELECT m.*, u.Username as MissionUsername FROM Mission m, User u WHERE u.Id = m.UserId AND m.Id = @Id", new { Id = id });
			}
		}

		public IEnumerable<MissionModel> GetMissionsByUserId(int userId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<MissionModel>("SELECT m.*, u.Username as MissionUsername FROM Mission m, User u WHERE u.Id = m.UserId AND UserId = @UserId", new { UserId = userId });
			}
		}

		public IEnumerable<Mission> GetByUserId(int missionUserId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<Mission>("SELECT * FROM Mission WHERE  userId = @userId", new { userId = missionUserId });
			}
		}

		public void Delete(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("DELETE FROM Mission WHERE Id = @Id", new { Id = id });
			}
		}

		public void Update(Mission mission)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				string sQuery = "UPDATE Mission SET " +
					"Name = @Name, " +
					"WHERE Id = @Id";

				dbConnection.Query(sQuery, mission);
			}
		}
	}
}
