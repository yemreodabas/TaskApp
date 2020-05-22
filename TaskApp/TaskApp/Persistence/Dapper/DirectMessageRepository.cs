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
	public class DirectMessageRepository : BaseSqliteRepository, IDirectMessageRepository
	{
		public void Insert(DirectMessage message)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("INSERT INTO DirectMessage (Message, SenderId, ReceiverId, IsDeleted) VALUES(@Message, @SenderId, @ReceiverId, @IsDeleted)", message);
				message.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
			}
		}

		public IEnumerable<DirectMessageModel> GetAll()
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<DirectMessageModel>("SELECT * FROM DirectMessage");
			}
		}

		public DirectMessageModel GetById(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<DirectMessageModel>("SELECT * FROM DirectMessage WHERE  Id = @Id", new { Id = id });
			}
		}
		/*
		public IEnumerable<DirectMessageModel> GetSenderMessage(int onlineUserId, int userId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<DirectMessageModel>("SELECT u.* FROM User u, DirectMessage m WHERE m.SenderId = @UserId AND u.Id = m.ReceiverId", new { UserId = onlineUserId });
			}
		}*/

		public IEnumerable<DirectMessageModel> GetMessageByUserId(int onlineUserId, int receiverId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<DirectMessageModel>("SELECT * FROM DirectMessage WHERE SenderId = @OnlineUserId AND ReceiverId = @ReceiverId OR SenderId = @ReceiverId AND ReceiverId = @OnlineUserId", new { OnlineUserId = onlineUserId, ReceiverId = receiverId });
				//return dbConnection.Query<DirectMessageModel>("SELECT * FROM DirectMessage WHERE SenderId = "+ onlineUserId + " AND ReceiverId = "+ receiverId + " OR SenderId = " + receiverId + " AND ReceiverId = " + onlineUserId + "", new { OnlineUserId = onlineUserId, UserId = receiverId });
			}
		}

		public IEnumerable<DirectMessageModel> GetLastMessage(int onlineUserId, int receiverId, int lastMessageId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<DirectMessageModel>("SELECT * FROM DirectMessage WHERE Id > @lastMessageId AND SenderId = @ReceiverId AND ReceiverId = @OnlineUserId", new { OnlineUserId = onlineUserId, ReceiverId = receiverId, LastMessageId = lastMessageId });
				//return dbConnection.Query<DirectMessageModel>("SELECT * FROM DirectMessage WHERE Id > @lastMessageId AND SenderId = " + receiverId + " AND ReceiverId = " + onlineUserId + "", new { OnlineUserId = onlineUserId, UserId = receiverId, LastMessageId = lastMessageId });
			}
		}

		public IEnumerable<DirectMessageModel> GetReceiverMessage(int onlineUserId, int receiverId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<DirectMessageModel>("SELECT * FROM DirectMessage WHERE SenderId =@ReceiverId AND ReceiverId = @OnlineUserId", new { OnlineUserId = onlineUserId, ReceiverId = receiverId });
				//return dbConnection.Query<DirectMessageModel>("SELECT * FROM DirectMessage WHERE SenderId = " + receiverId + " AND ReceiverId = " + onlineUserId + "", new { OnlineUserId = onlineUserId, ReceiverId = receiverId });
			}
		}
		/*
		public void GetBySenderId(int followerUserId, int targetUserId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("INSERT INTO DirectMessage (FollowerUserId, TargetUserId) VALUES(" + followerUserId + ", " + targetUserId + ")", new { FollowerUserId = followerUserId, TargetUserId = targetUserId });
			}
		}
		
		public IEnumerable<DirectMessage> GetByUserId(int userId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<DirectMessage>("SELECT * FROM DirectMessage WHERE  userId = @userId", new { userId = userId });
			}
		}*/

		public void Delete(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("DELETE FROM DirectMessage WHERE Id = @Id", new { Id = id });
			}
		}

		public void Update(DirectMessageModel directMessage)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				string sQuery = "UPDATE DirectMessage SET " +
					"IsDeleted = 1 " +
					"WHERE Id = @Id";

				dbConnection.Query(sQuery, directMessage);
			}
		}
	}
}
