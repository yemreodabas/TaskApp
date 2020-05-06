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
	public class UserRepository : BaseSqliteRepository, IUserRepository
	{
		public void Insert(User user)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("INSERT INTO User (Username, Email, Password, BirthYear) VALUES(@Username, @Email, @Password, @BirthYear)", user);
				user.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
			}
		}

		public IEnumerable<UserModel> GetAll()
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<UserModel>("SELECT * FROM User");
			}
		}

		public UserModel GetById(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<UserModel>("SELECT * FROM User WHERE  Id = @Id", new { Id = id });
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
					"Password = @Password, " +
					"BirthYear = @BirthYear " +
					"WHERE Id = @Id";

				dbConnection.Query(sQuery, user);
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

		public void FollowUser(int followerUserId, int targetUserId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("INSERT INTO Follow (FollowerUserId, TargetUserId) VALUES(" + followerUserId + ", " + targetUserId +")", new { FollowerUserId = followerUserId , TargetUserId = targetUserId });
			}
		}

		public void UnFollowUser(int followerUserId, int targetUserId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("DELETE FROM Follow WHERE FollowerUserId = " + followerUserId + " AND TargetUserId =  " + targetUserId + "", new { FollowerUserId = followerUserId, TargetUserId = targetUserId });
			}
		}

		public IEnumerable<UserModel> GetFollowerUsers(int onlineUserId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<UserModel>("SELECT u.* FROM User u, Follow f WHERE f.TargetUserId = @UserId AND u.Id = f.FollowerUserId", new { UserId = onlineUserId });
			}
		}

		public IEnumerable<UserModel> GetTargetUsers(int onlineUserId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<UserModel>("SELECT u.* FROM User u, Follow f WHERE f.FollowerUserId = @UserId AND u.Id = f.TargetUserId", new { UserId = onlineUserId });
			}
		}
	}
}
