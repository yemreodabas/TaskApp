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
	public class UserRepository : BaseSqliteRepository, IUserRepository
	{
		public void Insert(User user)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("INSERT INTO User (Username, Email, Password) VALUES(@Username, @Email, @Password)", user);
				user.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
			}
		}

		public IEnumerable<UserModel> GetAll()
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<UserModel>("SELECT u.*, ug.Name as GroupName FROM User u, UserGroup ug WHERE u.GroupId = ug.Id");
			}
		}

		public UserModel GetById(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<UserModel>("SELECT u.*, ug.Name as GroupName FROM User u, UserGroup ug WHERE u.GroupId = ug.Id AND u.Id = @Id", new { Id = id });
			}
		}

		public void Delete(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("DELETE FROM User WHERE Id = @Id", new { Id = id });
			}
		}

		public void Update(User user)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				string sQuery = "UPDATE User SET " +
					"Username = @Username, " +
					"Email = @Email, " +
					"Password = @Password" +
					"WHERE Id = @Id";

				dbConnection.Query(sQuery, user);
			}
		}

		public IEnumerable<User> GetByGroupId(int userGroupId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<User>("SELECT * FROM User WHERE GroupId = @GroupId", new { GroupId = userGroupId });
			}
		}

		public int GetCountByGroupId(int userGroupId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.ExecuteScalar<int>("SELECT COUNT(*) FROM User WHERE GroupId = @GroupId", new { GroupId = userGroupId });
			}
		}

		public int GetUserIdByLogin(string username, string password)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				int userId = dbConnection.Query<int>("SELECT Id FROM User WHERE Username = @Username AND Password = @Password",
									new { Username = username, Password = password }).FirstOrDefault();

				return userId;
			}
		}
	}
}
