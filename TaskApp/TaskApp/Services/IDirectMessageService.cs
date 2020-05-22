using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Services
{
    public interface IDirectMessageService
    {
        void AddNewMessage(DirectMessage message);
        void Delete(int id);
        void UpdateMessageStatus(DirectMessageModel directMessage);
        DirectMessageModel GetById(int id);
        //List<DirectMessageModel> GetById(int id);
        List<DirectMessageModel> GetAllMessages();
        List<DirectMessageModel> GetMessageById(int onlineUserId, int receiverId);
        List<DirectMessageModel> GetReceiverMessage(int onlineUserId, int receiverId);
        List<DirectMessageModel> GetLastMessage(int onlineId, int receiverId, int lastMessageId);
    }
}
