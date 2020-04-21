using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Services
{
	public interface IUserService
	{
		void AddNewUser(User user);
		void Delete(int id);
		UserModel GetById(int id);
		List<UserModel> GetAllUsers();
		List<UserModel> GetUsersByGroupId(int userGroupId);
		int GetUserCountByGroupId(int userGroupId);
		void Logout(HttpContext httpContext);
		bool TryLogin(UserLoginModel loginData, HttpContext httpContext);
		UserModel GetOnlineUser(HttpContext httpContext);
	}
}
