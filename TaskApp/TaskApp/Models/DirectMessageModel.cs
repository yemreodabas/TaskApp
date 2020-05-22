using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;

namespace TaskApp.Models
{
	public class DirectMessageModel
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public int SenderId { get; set; }
		public int ReceiverId { get; set; }
		public int IsDeleted { get; set; }

		public DirectMessageModel() { }

		public DirectMessageModel(DirectMessage directMessage)
		{
			this.Id = directMessage.Id;
			this.Message = directMessage.Message;
			this.SenderId = directMessage.SenderId;
			this.ReceiverId = directMessage.ReceiverId;
			this.IsDeleted = directMessage.IsDeleted;
		}
	}
}
