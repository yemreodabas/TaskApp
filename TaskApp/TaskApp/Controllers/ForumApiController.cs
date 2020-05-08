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
	[Route("api/Forum")]
	public class ForumApiController : Controller
	{
		private readonly IUserService _userService;
		private readonly IForumPostService _forumPostService;

		public ForumApiController(IUserService userService, IForumPostService forumPostService)
		{
			_userService = userService;
			_forumPostService = forumPostService;
		}

		[HttpGet]
		[Route(nameof(GetAllForumPost))]
		public ActionResult<ApiResponse<List<ForumPostModel>>> GetAllForumPost()
		{
			try
			{
				var users = this._userService.GetAllUsers();
				var forumPosts = this._forumPostService.GetAllForumPost();

				for(int i = 0;i < forumPosts.Count;i++)
				{
					for(int j = 0; j < users.Count;j++)
					{
						if(forumPosts[i].UserId == users[j].Id)
						{
							forumPosts[i].Username = users[j].Username;
						}
					}
				}

				var response = ApiResponse<List<ForumPostModel>>.WithSuccess(forumPosts);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<ForumPostModel>>.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(CreateForumPost))]
		public ActionResult<ApiResponse<ForumPostModel>> CreateForumPost([FromBody]CreateForumPostModel forumPost)
		{
			try
			{
				ForumPostModel result = null;

				var newPost = new ForumPost();
				newPost.Post = forumPost.Post;
				newPost.UserId = forumPost.UserId;

				this._forumPostService.AddNewForumPost(newPost);
				result = this._forumPostService.GetById(newPost.Id);

				return Json(ApiResponse<ForumPostModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<ForumPostModel>.WithError(exp.ToString()));
			}
		}
		/*
		[HttpPut]
		[Route(nameof(UpdatePost))]
		public ActionResult<ApiResponse<ForumPostModel>> UpdatePost([FromBody]CreateForumPostModel forumPost)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				ForumPostModel result = null;

				var newPost = new ForumPost();
				newPost.Id = onlineUser.Id;
				newPost.Post = forumPost.Post;
				newPost.UserId = forumPost.UserId;

				this._forumPostService.Update(newPost);

				result = this._forumPostService.GetById(newPost.Id);
				return Json(ApiResponse<ForumPostModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<UserModel>.WithError(exp.ToString()));
			}
		}*/

		[HttpDelete]
		[Route(nameof(DeletePost))]
		public ActionResult<ApiResponse> DeletePost([FromBody] int postId)
		{
			try
			{
				this._forumPostService.Delete(postId);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}
	}
}
