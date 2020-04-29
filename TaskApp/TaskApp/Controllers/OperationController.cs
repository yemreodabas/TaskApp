using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;
using TaskApp.Services;

namespace TaskApp.Controllers
{
	public class OperationController : Controller
	{
		private readonly IServices services;

		public OperationController(IServices services)
		{
			this.services = services;
		}

		[HttpGet]
		public ActionResult ListOperation()
		{
			var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.ListOperation));
			return View(model);
		}
	}
}
