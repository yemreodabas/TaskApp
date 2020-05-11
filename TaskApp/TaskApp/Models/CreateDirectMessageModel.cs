using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
	public class CreateDirectMessageModel
	{
		public string Message { get; set; }
		public int SenderId { get; set; }
		public int ReceiverId { get; set; }
	}
}
