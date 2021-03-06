﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Services;

namespace TaskApp.Controllers
{
	[ApiController]
	[Route("api/Mission")]
	public class MissionApiControllar : Controller
	{
		private readonly IUserService _userService;
		private readonly IMissionService _missionService;

		public MissionApiControllar(IUserService userService, IMissionService missionService)
		{
			_userService = userService;
			_missionService = missionService;
		}

		[HttpGet]
		[Route(nameof(GetActiveMissions))]
		public ActionResult<ApiResponse<List<MissionModel>>> GetActiveMissions()
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineUser == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				var missions = this._missionService.GetAllMyMissionsByUserId(onlineUser.Id);

				var response = ApiResponse<List<MissionModel>>.WithSuccess(missions);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<MissionModel>>.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetActiveMissionsByUsers))]
		public ActionResult<ApiResponse<List<MissionModel>>> GetActiveMissionsByUsers()
		{
			try
			{
				bool contain = false;

				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineUser == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				var users = this._userService.GetAllUsers();
				var missions = this._missionService.GetAllMission();
				var targets = this._userService.GetTargetUsers(onlineUser.Id);

				for(int i =0; i< missions.Count;i++)
				{
					for(int j =0; j < users.Count;j++)
					{
						if(missions[i].UserId == users[j].Id)
						{
							missions[i].MissionUsername = users[j].Username;
						}
					}
				}

				List<MissionModel>  newMissionList = new List<MissionModel>();

				for(int i = 0; i < missions.Count;i++)
				{
					for(int j = 0; j < targets.Count;j++)
					{
						if(missions[i].UserId == targets[j].Id)
						{
							contain = true;
							continue;
						}
					}
					if (contain == true)
					{
						newMissionList.Add(missions[i]);
					}
					contain = false;
				}

				var response = ApiResponse<List<MissionModel>>.WithSuccess(newMissionList);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<MissionModel>>.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(CreateMission))]
		public ActionResult<ApiResponse<MissionModel>> CreateMission([FromBody]CreateMissionModel model)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(model.Name))
				{
					return Json(ApiResponse<MissionModel>.WithError("Name is required"));
				}

				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineUser == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				MissionModel result = null;

				var newMission = new Mission();
				newMission.Name = model.Name;
				newMission.UserId = onlineUser.Id;

				this._missionService.AddNewMission(newMission);
				result = _missionService.GetById(newMission.Id);

				return Json(ApiResponse<MissionModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<MissionModel>.WithError(exp.ToString()));
			}
		}

		[HttpDelete]
		[Route(nameof(DeleteMission))]
		public ActionResult<ApiResponse> DeleteMission([FromBody] int missionId)
		{
			try
			{
				this._missionService.Delete(missionId);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}
	}
}
