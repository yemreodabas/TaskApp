using Microsoft.AspNetCore.Mvc;
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
	[Route("api/Home")]
	public class HomeApiControllar : Controller
	{
		private readonly IUserService _userService;
		private readonly IMissionService _missionService;

		public HomeApiControllar(IUserService userService, IMissionService missionService)
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
				var onlineUser = this._userService.GetAllUsers();

				var missions = this._missionService.GetAllMission();

				for(int i = 0; i < missions.Count;i++)
				{
					for(int j = 0; j < onlineUser.Count; j++)
					{
						if(missions[i].UserId == onlineUser[j].Id)
						{
							missions[i].MissionUsername = onlineUser[j].Username;
						}
					}
				}

				/*foreach(MissionModel mission in missions)
				{
					if()
					mission.MissionUsername = user.Username;
				}*/

				var response = ApiResponse<List<MissionModel>>.WithSuccess(missions);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<MissionModel>>.WithError(exp.ToString()));
			}
		}
	}
}
