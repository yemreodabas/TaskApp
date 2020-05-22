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
	[Route("api/User")]
	public class UserApiController : Controller
	{
		private readonly IUserService _userService;
		private readonly IDirectMessageService _directMessageService;

		public UserApiController(IUserService userService, IDirectMessageService directMessageService)
		{
			_userService = userService;
			_directMessageService = directMessageService;
		}

		[HttpGet]
		[Route(nameof(GetActiveUsers))]
		public ActionResult<ApiResponse<List<UserModel>>> GetActiveUsers()
		{
			try
			{
				var users = this._userService.GetAllUsers();

				var response = ApiResponse<List<UserModel>>.WithSuccess(users);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<UserModel>>.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(CreateUser))]
		public ActionResult<ApiResponse<UserModel>> CreateUser([FromBody]CreateUserModel model)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineUser != null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				UserModel result = null;

				var newUser = new User();
				newUser.Username = model.Username;
				newUser.Email = model.Email;
				newUser.Password = model.Password;
				newUser.BirthYear = model.BirthYear;

				bool userCheck = this._userService.AddNewUser(newUser);

				if(userCheck == true)
				{
					result = this._userService.GetById(newUser.Id);
					return Json(ApiResponse<UserModel>.WithSuccess(result));
				}

				return Json(ApiResponse<UserModel>.WithError("User Exist"));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<UserModel>.WithError(exp.ToString()));
			}
		}

		[HttpPut]
		[Route(nameof(UpdateUser))]
		public ActionResult<ApiResponse<UserModel>> UpdateUser([FromBody]CreateUserModel model)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				if(onlineUser == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				UserModel result = null;

				var newUser = new User();
				newUser.Id = onlineUser.Id;
				newUser.Username = model.Username;
				newUser.Email = model.Email;
				newUser.Password = model.Password;
				newUser.BirthYear = model.BirthYear;

				bool userCheck = this._userService.UpdateUser(newUser);

				if (userCheck == true)
				{
					result = this._userService.GetById(newUser.Id);
					return Json(ApiResponse<UserModel>.WithSuccess(result));
				}

				return Json(ApiResponse<UserModel>.WithError("User Exist"));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<UserModel>.WithError(exp.ToString()));
			}
		}

		[HttpDelete]
		[Route(nameof(DeleteUser))]
		public ActionResult<ApiResponse> DeleteUser([FromBody] int userId)
		{
			try
			{
				this._userService.Delete(userId);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(Login))]
		public ActionResult<ApiResponse> Login([FromBody]UserLoginModel model)
		{
			try
			{
				if (!this._userService.TryLogin(model, this.HttpContext))
				{
					return Json(ApiResponse.WithError("Invalid Username or Password!"));
				}

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetOnlineUser))]
		public ActionResult<ApiResponse<UserModel>> GetOnlineUser()
		{
			try
			{
				var user = this._userService.GetOnlineUser(this.HttpContext);

				return Json(ApiResponse<UserModel>.WithSuccess(user));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<UserModel>.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(FollowUser))]
		public ActionResult<ApiResponse> FollowUser([FromBody] int targetUser)
		{
			try
			{
				var followerUser = this._userService.GetOnlineUser(this.HttpContext);

				if (followerUser == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				this._userService.FollowUser(followerUser.Id, targetUser);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpDelete]
		[Route(nameof(UnFollowUser))]
		public ActionResult<ApiResponse> UnFollowUser([FromBody] int targetUser)
		{
			try
			{
				var followerUser = this._userService.GetOnlineUser(this.HttpContext);

				if (followerUser == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				this._userService.UnFollowUser(followerUser.Id, targetUser);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetFollowUsers))]
		public ActionResult<ApiResponse> GetFollowUsers()
		{
			try
			{
				var onlineId = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineId == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				var users = this._userService.GetFollowUsers(onlineId.Id);

				var response = ApiResponse<List<UserModel>>.WithSuccess(users);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetFollowerById))]
		public ActionResult<ApiResponse> GetFollowerById(int userId)
		{
			try
			{
				var users = this._userService.GetFollowUsers(userId);

				var response = ApiResponse<List<UserModel>>.WithSuccess(users);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetTargetUsers))]
		public ActionResult<ApiResponse> GetTargetUsers()
		{
			try
			{
				var onlineId = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineId == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				var users =  this._userService.GetTargetUsers(onlineId.Id);

				var response = ApiResponse<List<UserModel>>.WithSuccess(users);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetTargetById))]
		public ActionResult<ApiResponse> GetTargetById(int userId)
		{
			try
			{
				var users = this._userService.GetTargetUsers(userId);

				var response = ApiResponse<List<UserModel>>.WithSuccess(users);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetNotTargetUsers))]
		public ActionResult<ApiResponse> GetNotTargetUsers()
		{
			try
			{
				bool contain = false;

				var onlineId = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineId == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				var targetUsers = this._userService.GetTargetUsers(onlineId.Id);

				//var users = this._userService.GetAllUsers();
				var users = this._userService.GetAllUsers();

				for (int i = 0; i < users.Count;i++)
				{
					if(users[i].Id == onlineId.Id)
					{
						users.RemoveAt(i);
						break;
					}
				}

				List<UserModel> newTargets = new List<UserModel>();

				for (int i = 0; i < users.Count; i++)
				{
					for (int j = 0; j < targetUsers.Count; j++)
					{
						if (users[i].Id == targetUsers[j].Id)
						{
							contain = true;
							continue;
						}
					}
					if (contain == false)
					{
						newTargets.Add(users[i]);
					}
					contain = false;
				}

				var response = ApiResponse<List<UserModel>>.WithSuccess(newTargets);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(NewMessageAdd))]
		public ActionResult<ApiResponse<DirectMessageModel>> NewMessageAdd([FromBody]CreateDirectMessageModel model)
		{
			try
			{
				var onlineId = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineId == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				DirectMessageModel result = null;

				var newMessage = new DirectMessage();
				newMessage.Message = model.Message;
				newMessage.SenderId = model.SenderId;
				newMessage.ReceiverId = model.ReceiverId;
				newMessage.IsDeleted = 0;

				this._directMessageService.AddNewMessage(newMessage);

				result = this._directMessageService.GetById(newMessage.Id);
				return Json(ApiResponse<DirectMessageModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<DirectMessageModel>.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetMessageById))]
		public ActionResult<ApiResponse> GetMessageById(int receiverId)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineUser == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				var messages = this._directMessageService.GetMessageById(onlineUser.Id, receiverId);

				var response = ApiResponse<List<DirectMessageModel>>.WithSuccess(messages);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetLastMessage))]
		public ActionResult<ApiResponse> GetLastMessage(int lastMessageId)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineUser == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				var lastMessage = this._directMessageService.GetById(lastMessageId);

				int receiverId = lastMessage.ReceiverId;

				var messages = this._directMessageService.GetLastMessage(onlineUser.Id, receiverId, lastMessageId);

				var response = ApiResponse<List<DirectMessageModel>>.WithSuccess(messages);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpDelete]
		[Route(nameof(DeleteMessage))]
		public ActionResult<ApiResponse> DeleteMessage([FromBody] int messageId)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineUser == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				var deletedMessage = this._directMessageService.GetById(messageId);
				this._directMessageService.UpdateMessageStatus(deletedMessage);
				
				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}
	}
}
