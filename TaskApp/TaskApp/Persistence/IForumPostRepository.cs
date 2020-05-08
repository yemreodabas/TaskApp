using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence
{
    public interface IForumPostRepository
    {
        void Insert(ForumPost mission);
        void Delete(int id);
        void Update(ForumPost mission);
        IEnumerable<ForumPostModel> GetAll();
        ForumPostModel GetById(int id);
    }
}
