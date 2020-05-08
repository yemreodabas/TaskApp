using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;

namespace TaskApp.Models
{
	public class ForumPostModel
	{
		public int Id { get; set; }
		public string Post { get; set; }
		public int UserId { get; set; }
		public string Username { get; set; }

		public ForumPostModel() { }

		public ForumPostModel(ForumPost forumPost)
		{
			this.Id = forumPost.Id;
			this.Post = forumPost.Post;
			this.UserId = forumPost.UserId;
		}
	}
}
