using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;
using TaskApp.Services;

namespace TaskApp.Controllers
{
	public class ForumController : Controller
	{
		private readonly IServices services;

		public ForumController(IServices services)
		{
			this.services = services;
		}

		[HttpGet]
		public ActionResult ForumPost()
		{
			var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.ForumPost));

			return View(model);
		}
	}
}
