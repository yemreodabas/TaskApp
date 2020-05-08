using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Persistence;

namespace TaskApp.Services
{
	public class ForumPostService : IForumPostService
	{
		private readonly ILogRepository _logRepository;
		private readonly IForumPostRepository _forumPostRepository;

		public ForumPostService(ILogRepository logRepository, IForumPostRepository forumPostRepository)
		{
			this._logRepository = logRepository;
			this._forumPostRepository = forumPostRepository;
		}

		public void AddNewForumPost(ForumPost forumPost)
		{
			this._forumPostRepository.Insert(forumPost);
			this._logRepository.Log(Enums.LogType.Info, $"Inserted New ForumPost : {forumPost.Post}");
			// this._emailService.SendEmail("fsdf", "dsfsdfsdfdsf");
		}
		public void Delete(int id)
		{
			var forumPost = this._forumPostRepository.GetById(id);
			this._forumPostRepository.Delete(id);
			this._logRepository.Log(Enums.LogType.Info, $"Deleted ForumPost : {forumPost.Post}");
		}

		public ForumPostModel GetById(int id)
		{
			return this._forumPostRepository.GetById(id);
		}

		public List<ForumPostModel> GetAllForumPost()
		{
			var posts = this._forumPostRepository.GetAll().ToList();

			return posts;
		}
	}
}
