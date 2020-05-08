using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Entities
{
	public class ForumPost
	{
		[Key]
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Post { get; set; }
	}
}
