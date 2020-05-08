using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Services
{
    public interface IForumPostService
    {
        void AddNewForumPost(ForumPost forumPost);
        void Delete(int id);
        ForumPostModel GetById(int id);
        List<ForumPostModel> GetAllForumPost();
    }
}
