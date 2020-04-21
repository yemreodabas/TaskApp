﻿using Dapper;
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
				dbConnection.Execute("INSERT INTO User (Username, Email, Password) VALUES(@Username, @Email, @Password)", mission);
				mission.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
			}
		}

		public IEnumerable<MissionModel> GetAll()
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<MissionModel>("SELECT u.*, ug.Name as GroupName FROM User u, UserGroup ug WHERE u.GroupId = ug.Id");
			}
		}

		public MissionModel GetById(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<MissionModel>("SELECT u.*, ug.Name as GroupName FROM Mission u, UserGroup ug WHERE u.GroupId = ug.Id AND u.Id = @Id", new { Id = id });
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
