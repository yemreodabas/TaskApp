using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;
using TaskApp.Services;

namespace TaskApp.Controllers
{
	public class MissionController : Controller
	{
		private readonly IServices services;

		public MissionController(IServices services)
		{
			this.services = services;
		}

		[HttpGet]
		public ActionResult ListMission()
		{
			var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.ListMission));
			
			if(model != null)
			{
				return View(model);
			}

			return RedirectToAction("Login", "User");
		}
	}
}
