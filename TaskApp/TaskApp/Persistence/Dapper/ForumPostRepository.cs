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
	public class ForumPostRepository : BaseSqliteRepository, IForumPostRepository
	{
		public void Insert(ForumPost forumPost)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("INSERT INTO Forum (Post, UserId) VALUES(@Post, @UserId)", forumPost);
				forumPost.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
			}
		}

		public IEnumerable<ForumPostModel> GetAll()
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<ForumPostModel>("SELECT * FROM Forum");
			}
		}

		public ForumPostModel GetById(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<ForumPostModel>("SELECT * FROM Forum WHERE  Id = @Id", new { Id = id });
			}
		}

		public IEnumerable<ForumPost> GetByUserId(int userId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<ForumPost>("SELECT * FROM Forum WHERE  userId = @userId", new { userId = userId });
			}
		}

		public void Delete(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("DELETE FROM Forum WHERE Id = @Id", new { Id = id });
			}
		}

		public void Update(ForumPost forumPost)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				string sQuery = "UPDATE Forum SET " +
					"Comment = @Comment, " +
					"WHERE Id = @Id";

				dbConnection.Query(sQuery, forumPost);
			}
		}
	}
}
