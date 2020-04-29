using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;
using TaskApp.Services;

namespace TaskApp.Controllers
{
	public class UserController : Controller
	{
		private readonly IServices services;

		public UserController(IServices services)
		{
			this.services = services;
		}

		[HttpGet]
		public ActionResult List()
		{
			var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.List));
			return View(model);
		}

		public IActionResult Login()
		{
			var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.Login));
			return View(model);
		}
		public IActionResult Register()
		{
			var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.Register));
			return View(model);
		}

		public ActionResult MissionDetail(int id)
		{
			var mission = id;

			var model = this.services.ViewService.CreateViewModel<OperationViewModel>(this.HttpContext, nameof(this.MissionDetail));
			model.MissionId = mission.ToString();

			return View(model);
		}

		public IActionResult Logout()
		{
			this.services.UserService.Logout(this.HttpContext);

			return RedirectToAction("Index", "Home");
		}
	}
}
