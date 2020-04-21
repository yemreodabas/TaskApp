using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Persistence;

namespace TaskApp.Services
{
	public class MissionService : IMissionService
	{
		private readonly ILogRepository _logRepository;
		private readonly IMissionRepository _missionRepository;

		public MissionService(ILogRepository logRepository, IMissionRepository missionRepository)
		{
			this._logRepository = logRepository;
			this._missionRepository = missionRepository;
		}

		public void AddNewMission(Mission mission)
		{
			this._missionRepository.Insert(mission);
			this._logRepository.Log(Enums.LogType.Info, $"Inserted New Mission : {mission.Name}");
			// this._emailService.SendEmail("fsdf", "dsfsdfsdfdsf");
		}
		public void Delete(int id)
		{
			var mission = this._missionRepository.GetById(id);
			this._missionRepository.Delete(id);
			this._logRepository.Log(Enums.LogType.Info, $"Deleted Mission : {mission.Name}");
		}

		public MissionModel GetById(int id)
		{
			return this._missionRepository.GetById(id);
		}

		public List<MissionModel> GetAllMission()
		{
			var missions = this._missionRepository.GetAll().ToList();

			return missions;
		}
	}
}
