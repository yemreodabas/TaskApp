using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Persistence;

namespace TaskApp.Services
{
	public class OperationService : IOperationService
	{
		private readonly ILogRepository _logRepository;
		private readonly IOperationRepository _operationRepository;

		public OperationService(ILogRepository logRepository, IOperationRepository operationRepository)
		{
			this._logRepository = logRepository;
			this._operationRepository = operationRepository;
		}

		public void AddNewOperation(Operation operation)
		{
			this._operationRepository.Insert(operation);
			this._logRepository.Log(Enums.LogType.Info, $"Inserted New Operation : {operation.Name}");
			// this._emailService.SendEmail("fsdf", "dsfsdfsdfdsf");
		}
		public void Delete(int id)
		{
			var operation = this._operationRepository.GetByOpId(id);
			this._operationRepository.Delete(id);
			this._logRepository.Log(Enums.LogType.Info, $"Deleted Operation : {operation.Name}");
		}

		public void Update(int id)
		{
			var operation = this._operationRepository.GetByCurrentId(id);
			operation.OperationStatus = 1;

			this._operationRepository.UpdateOpStatus(operation);
			this._logRepository.Log(Enums.LogType.Info, $"Deleted Operation : {operation.Name}");
		}

		public OperationModel GetById(int id)
		{
			return this._operationRepository.GetById(id);
		}

		public OperationModel GetByOperationId(int id)
		{
			return this._operationRepository.GetByOpId(id);
		}

		public List<OperationModel> GetAllOperation()
		{
			var operation = this._operationRepository.GetAll().ToList();

			return operation;
		}

		public List<OperationModel> GetOperationByMissionId(int missionId)
		{
			var missions = this._operationRepository.GetByMissionId(missionId).Select(operation => new OperationModel(operation)).ToList();

			return missions;
		}

		public List<OperationModel> GetAllOperationsByMissionId(int missionId)
		{
			var missions = this._operationRepository.GetOperationsByMissionId(missionId).ToList();

			return missions;
		}
	}
}
