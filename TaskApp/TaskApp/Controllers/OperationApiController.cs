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
	[Route("api/Operation")]
	public class OperationApiControllar : Controller
	{
		private readonly IUserService _userService;
		private readonly IMissionService _missionService;
		private readonly IOperationService _operationService;

		public OperationApiControllar(IUserService userService, IMissionService missionService, IOperationService operationService)
		{
			_userService = userService;
			_missionService = missionService;
			_operationService = operationService;
		}

		[HttpGet]
		[Route(nameof(GetActiveOperations))]
		public ActionResult<ApiResponse<List<OperationModel>>> GetActiveOperations()
		{
			try
			{
				var operation = this._operationService.GetAllOperation();

				var response = ApiResponse<List<OperationModel>>.WithSuccess(operation);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<OperationModel>>.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(CreateOperation))]
		public ActionResult<ApiResponse<OperationModel>> CreateOperation([FromBody]CreateOperationModel model)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(model.Name))
				{
					return Json(ApiResponse<OperationModel>.WithError("Name is required"));
				}

				var user = this._userService.GetOnlineUser(this.HttpContext);

				OperationModel result = null;

				var newOperation = new Operation();
				newOperation.Name = model.Name;
				newOperation.MissionId = Convert.ToInt32(model.MissionId);
				newOperation.OperationStatus = 0;

				this._operationService.AddNewOperation(newOperation);
				result = _operationService.GetByOperationId(newOperation.Id);

				return Json(ApiResponse<OperationModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<OperationModel>.WithError(exp.ToString()));
			}
		}

		[HttpDelete]
		[Route(nameof(DeleteOperation))]
		public ActionResult<ApiResponse> DeleteOperation([FromBody] int operationId)
		{
			try
			{
				this._operationService.Delete(operationId);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpPut]
		[Route(nameof(UpdateOperation))]
		public ActionResult<ApiResponse> UpdateOperation([FromBody] int operationId)
		{
			try
			{
				var updateOperation = this._operationService.GetByOperationId(operationId);
				this._operationService.Update(operationId);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}
	}
}
