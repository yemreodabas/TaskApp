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
	public class OperationRepository : BaseSqliteRepository, IOperationRepository
	{
		public void Insert(Operation operation)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("INSERT INTO Operation (Name, MissionId, OperationStatus) VALUES(@Name, @MissionId, @OperationStatus)", operation);
				operation.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
			}
		}

		public IEnumerable<OperationModel> GetAll()
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<OperationModel>("SELECT * FROM Operation");
			}
		}

		public OperationModel GetById(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<OperationModel>("SELECT o.*, m.Id AS MissionId FROM Operation o, Mission m, WHERE  m.Id = o.MissionId AND o.Id = @Id", new { Id = id });
			}
		}

		public OperationModel GetByOpId(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<OperationModel>("SELECT * FROM Operation WHERE Id = @Id", new { Id = id });
			}
		}

		public Operation GetByCurrentId(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<Operation>("SELECT * FROM Operation WHERE Id = @Id", new { Id = id });
			}
		}

		public IEnumerable<OperationModel> GetOperationsByMissionId(int missionId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<OperationModel>("SELECT * FROM Operation WHERE MissionId = @MissionId", new { MissionId = missionId });
			}
		}

		public IEnumerable<Operation> GetByMissionId(int missionId)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<Operation>("SELECT * FROM Operation WHERE  missionId = @missionId", new { MissionId = missionId });
			}
		}

		public void Delete(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("DELETE FROM Operation WHERE Id = @Id", new { Id = id });
			}
		}

		public void UpdateOpStatus(Operation operation)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				string sQuery = "UPDATE Operation SET " +
					"OperationStatus = 1 " +
					"WHERE Id = @Id";

				dbConnection.Query(sQuery, operation);
			}
		}
	}
}
