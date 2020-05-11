﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Entities
{
	public class DirectMessage
	{
		[Key]
		public int Id { get; set; }
		public string Message { get; set; }
		public int SenderId { get; set; }
		public int ReceiverId { get; set; }
	}
}
