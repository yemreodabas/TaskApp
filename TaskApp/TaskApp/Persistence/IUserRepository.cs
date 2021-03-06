﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence
{
    public interface IUserRepository
    {
        void Insert(User user);
        void Delete(int id);
        void Update(User user);
        IEnumerable<UserModel> GetAll();
        UserModel GetById(int id);
        int GetUserIdByLogin(string username, string password);
        void FollowUser(int followerUserId, int targetUserId);
        void UnFollowUser(int followerUserId, int targetUserId);
        IEnumerable<UserModel> GetFollowerUsers(int onlineUserId);
        IEnumerable<UserModel> GetTargetUsers(int onlineUserId);
    }
}
