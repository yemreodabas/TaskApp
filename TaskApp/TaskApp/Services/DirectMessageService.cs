using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Persistence;

namespace TaskApp.Services
{
	public class DirectMessageService : IDirectMessageService
	{
		private readonly ILogRepository _logRepository;
		private readonly IDirectMessageRepository _directMessageRepository;

		public DirectMessageService(ILogRepository logRepository, IDirectMessageRepository directMessageRepository)
		{
			this._logRepository = logRepository;
			this._directMessageRepository = directMessageRepository;
		}

		public void AddNewMessage(DirectMessage messages)
		{
			this._directMessageRepository.Insert(messages);
			this._logRepository.Log(Enums.LogType.Info, $"Inserted New DirectMessage : {messages.Message}");
			// this._emailService.SendEmail("fsdf", "dsfsdfsdfdsf");
		}
		public void Delete(int id)
		{
			var message = this._directMessageRepository.GetById(id);
			this._directMessageRepository.Delete(id);
			this._logRepository.Log(Enums.LogType.Info, $"Deleted Message : {message.Message}");
		}

		public void UpdateMessageStatus(DirectMessageModel directMessage)
		{
			this._directMessageRepository.Update(directMessage);
			this._logRepository.Log(Enums.LogType.Info, $"Inserted New User : {directMessage.Message}");
			// this._emailService.SendEmail("fsdf", "dsfsdfsdfdsf");
		}

		public DirectMessageModel GetById(int id)
		{
			return this._directMessageRepository.GetById(id);
		}

		public List<DirectMessageModel> GetMessageById(int onlineId, int receiverId)
		{
			return this._directMessageRepository.GetMessageByUserId(onlineId, receiverId).ToList();
		}

		public List<DirectMessageModel> GetLastMessage(int onlineId, int receiverId, int lastMessageId)
		{
			return this._directMessageRepository.GetLastMessage(onlineId, receiverId, lastMessageId).ToList();
		}

		public List<DirectMessageModel> GetReceiverMessage(int onlineId, int receiverId)
		{
			return this._directMessageRepository.GetReceiverMessage(receiverId, onlineId).ToList();
		}

		/*
		public List<DirectMessageModel> GetById(int id)
		{
			return this._directMessageRepository.GetById(id).ToList();
		}
		*/
		public List<DirectMessageModel> GetAllMessages()
		{
			var messages = this._directMessageRepository.GetAll().ToList();

			return messages;
		}
	}
}
