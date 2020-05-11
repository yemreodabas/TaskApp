using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence
{
    public interface IDirectMessageRepository
    {
        void Insert(DirectMessage messages);
        void Delete(int id);
        IEnumerable<DirectMessageModel> GetAll();
        //IEnumerable<DirectMessageModel> GetById(int onlineUserId);
        DirectMessageModel GetById(int id);
        IEnumerable<DirectMessageModel> GetMessageByUserId(int onlineUserId, int receiverId);
        IEnumerable<DirectMessageModel> GetReceiverMessage(int onlineUserId, int receiverId);
    }
}
