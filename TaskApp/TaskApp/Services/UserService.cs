﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Persistence;

namespace TaskApp.Services
{
	public class UserService : IUserService
	{
		private readonly ILogRepository _logRepository;
		private readonly IUserRepository _userRepository;

		public UserService(ILogRepository logRepository, IUserRepository userRepository)
		{
			this._logRepository = logRepository;
			this._userRepository = userRepository;
		}

		public bool AddNewUser(User user)
		{
			bool userCheck = true;

			List<UserModel> userList = GetAllUsers();

			for(int i = 0; i < userList.Count;i++)
			{
				if(user.Username == userList[i].Username)
				{
					return false;
				}
			}

			this._userRepository.Insert(user);
			this._logRepository.Log(Enums.LogType.Info, $"Inserted New User : {user.Username}");
			// this._emailService.SendEmail("fsdf", "dsfsdfsdfdsf");

			return userCheck;
		}

		public bool UpdateUser(User user)
		{
			bool userCheck = true;

			List<UserModel> userList = GetAllUsers();

			for (int i = 0; i < userList.Count; i++)
			{
				if (user.Username == userList[i].Username)
				{
					return false;
				}
			}

			this._userRepository.Update(user);
			this._logRepository.Log(Enums.LogType.Info, $"Inserted New User : {user.Username}");
			// this._emailService.SendEmail("fsdf", "dsfsdfsdfdsf");

			return userCheck;
		}
		public void Delete(int id)
		{
			var user = this._userRepository.GetById(id);
			this._userRepository.Delete(id);
			this._logRepository.Log(Enums.LogType.Info, $"Deleted User : {user.Username}");
		}

		public UserModel GetById(int id)
		{
			return this._userRepository.GetById(id);
		}

		public List<UserModel> GetAllUsers()
		{
			var users = this._userRepository.GetAll().ToList();

			return users;
		}

		public void Logout(HttpContext httpContext)
		{
			httpContext.Session.Remove("onlineUserId");
		}

		public bool TryLogin(UserLoginModel loginData, HttpContext httpContext)
		{
			int userId = this._userRepository.GetUserIdByLogin(loginData.Username, loginData.Password);

			if (userId > 0)
			{
				httpContext.Session.SetInt32("onlineUserId", userId);
				return true;
			}

			return false;
		}

		public UserModel GetOnlineUser(HttpContext httpContext)
		{
			int? onlineUserId = httpContext.Session.GetInt32("onlineUserId");
			if (!onlineUserId.HasValue)
			{
				return null;
			}

			return this._userRepository.GetById(onlineUserId.Value);
		}

		public void FollowUser(int followUserId, int targetUserId)
		{
			this._userRepository.FollowUser(followUserId, targetUserId);
		}
		public void UnFollowUser(int followUserId, int targetUserId)
		{
			this._userRepository.UnFollowUser(followUserId, targetUserId);
		}

		public List<UserModel> GetFollowUsers(int onlineId)
		{
			var followUsers = this._userRepository.GetFollowerUsers(onlineId).ToList();

			return followUsers;
		}

		public List<UserModel> GetTargetUsers(int onlineId)
		{
			var targetUsers = this._userRepository.GetTargetUsers(onlineId).ToList();

			return targetUsers;
		}
	}
}
