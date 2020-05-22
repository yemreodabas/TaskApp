using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace NetCoreMvcExample.Helpers
{
	public static class Converter
	{
		public static UserModel ToModel(this User user)
		{
			return new UserModel(user);
		}
		
		public static User ToEntity(this UserModel userModel)
		{
			return new User()
			{
				Id = userModel.Id,
				Username = userModel.Username,
				Email = userModel.Email,
				BirthYear = userModel.BirthYear,
			};
		}

		public static DirectMessage ToEntity(this DirectMessageModel directMessageModel)
		{
			return new DirectMessage()
			{
				Id = directMessageModel.Id,
				Message = directMessageModel.Message,
				SenderId = directMessageModel.SenderId,
				ReceiverId = directMessageModel.ReceiverId,
				IsDeleted = directMessageModel.IsDeleted,
			};
		}
	}
}
