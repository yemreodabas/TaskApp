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
			var operation = this._operationRepository.GetById(id);
			this._operationRepository.Delete(id);
			this._logRepository.Log(Enums.LogType.Info, $"Deleted Operation : {operation.Name}");
		}

		public OperationModel GetById(int id)
		{
			return this._operationRepository.GetById(id);
		}

		public List<OperationModel> GetAllOperation()
		{
			var operation = this._operationRepository.GetAll().ToList();

			return operation;
		}
	}
}
