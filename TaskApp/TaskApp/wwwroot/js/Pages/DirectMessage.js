function main() {

	page.lastMessage = "";

	page.users = tryGetUsers();
}

function tryGetUsers() {
	httpRequest("api/User/GetActiveUsers", "GET", null, handleGetUser, showError.bind(null, "System Error"));
}

function tryGetMessageById(receiverId) {
	httpRequest("api/User/GetMessageById/?receiverId=" + receiverId, "GET", null, handleGetMessage, showError.bind(null, "System Error"));
}
/*
function tryGetReceiverById(receiverId) {
	httpRequest("api/User/GetReceiverById/?receiverId=" + receiverId, "GET", null, handleGetReceiverMessage, showError.bind(null, "System Error"));
}

function checkMessages() {
	receiverId = page.receiverId;
	lastMessage = page.lastMessage.Id;

	httpRequest("api/User/GetMessageById/?receiverId=" + receiverId, "GET", null, handleGetLastMessage, showError.bind(null, "System Error"));
}*/

function checkMessages() {
	//receiverId = page.receiverId;
	let lastMessage;
	if (page.lastMessage.Id != undefined) {
		lastMessage = page.lastMessage.Id;
    }
 	
	if (lastMessage) {
		httpRequest("api/User/GetLastMessage/?lastMessage=" + lastMessage, "GET", null, handleGetLastMessage, showError.bind(null, "System Error"));
    }

	
}

function setup() {
	setInterval(checkMessages, 500);
}

function tryGetSenderAndReceiver(receiverId) {
	let sendMessageRemove = document.getElementById("send-message-box");
	if (sendMessageRemove != undefined) {
		sendMessageRemove.parentNode.removeChild(sendMessageRemove);
	}

	let messageListRemove = document.getElementById("message-list");
	if (messageListRemove != undefined) {
		messageListRemove.parentNode.removeChild(messageListRemove);
	}
	
	for (let i = 0; i < page.users.length; i++) {
		if (page.users[i].Id == receiverId) {
			page.currentlyTalkingName = page.users[i].Username;
			break;
        }
    }

	page.receiverId = receiverId;

	//setup();

	let sendMessageDiv = document.createElement("div");
	document.getElementById("container").appendChild(sendMessageDiv);
	sendMessageDiv.id = "message-list";
	sendMessageDiv.classList.add("clearfix");

	let currentlyTalkingUserName = document.createElement("h2");
	document.getElementById("message-list").appendChild(currentlyTalkingUserName);
	currentlyTalkingUserName.innerHTML = page.currentlyTalkingName;
	currentlyTalkingUserName.id = "current-username";
	currentlyTalkingUserName.classList.add("current-username");


	tryGetMessageById(receiverId);

	let title = document.createElement("h2");
	let messageList = document.createElement("div");
	let message = document.createElement("input");
	let sendMessageBtn = document.createElement("button");
	let updateBtn = document.createElement("button");

	

	document.getElementById("container").appendChild(messageList);
	messageList.id = "send-message-box";

	document.getElementById("send-message-box").appendChild(title);
	title.innerHTML = "Send Message";
	title.id = "message-title";
	title.classList.add("message-title");

	document.getElementById("send-message-box").appendChild(message);
	message.innerHTML = "Send Message";
	message.id = "message-input";
	message.classList.add("message-input");

	document.getElementById("send-message-box").appendChild(sendMessageBtn);
	sendMessageBtn.innerHTML = "Send!";
	sendMessageBtn.id = "send-message-btn-" + receiverId;
	sendMessageBtn.classList.add("btns");
	sendMessageBtn.onclick = tryInsertMessage;

	document.getElementById("send-message-box").appendChild(updateBtn);
	updateBtn.innerHTML = "Update!";
	updateBtn.id = "updateBtn-btn";
	updateBtn.classList.add("btns");
	updateBtn.onclick = setup;
}

/*
function tryGetUsers() {
	httpRequest("api/User/GetActiveUsers", "GET", null, handleGetUsers, showError.bind(null, "System Error"));
}*/

function redirectDirectMessage() {
	redirect("User/DirectMessage/");
}

function handleGetMessage(response) {
	let receiverId = page.receiverId;
	let onlineId = document.getElementById("online-user-box").value;
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.Messages = response.Data;

	for (let i = 0; i < page.Messages.length; i++) {
		let message = page.Messages[i];
		if (message.IsDeleted == 1) {
			appendDeletedMessage();
		}
		if (message != null || message != "" && message.IsDeleted != 1) {
			if (message.SenderId == receiverId) {
				appendReceiverMessage(message);
				page.lastMessage = message;
			}
			else if (message.SenderId == onlineId && message.IsDeleted != 1) {
				appendSenderMessage(message);
				//page.lastMessage = message;
			}
		}
	}

	setTimeout(checkMessages, 500);
}

function handleGetLastMessage(response) {
	let receiverId = page.receiverId;
	let onlineId = document.getElementById("online-user-box").value;
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.Messages = response.Data;

	for (let i = page.messageCounter; i < page.Messages.length; i++) {
		let message = page.Messages[i];
		if (message != null || message != "") {
			if (message.SenderId == receiverId) {
				appendReceiverMessage(message);
				page.lastMessage = message;
			}
			else if (message.SenderId == onlineId) {
				appendSenderMessage(message);
				page.lastMessage = message;
			}
		}
	}

	setTimeout(checkMessages, 500);
}

function tryInsertMessage() {
	let message = document.getElementById("message-input").value;
	let receiverId = parseInt(page.receiverId);
	let tempOnlineUser = document.getElementById("online-user-box").value;
	let onlineUser = parseInt(tempOnlineUser);

	let data = {
		Message: message,
		SenderId: onlineUser,
		ReceiverId: receiverId,
	};

	httpRequest("api/User/NewMessageAdd", "POST", data, handleInsertMessage, showError.bind(null, "System Error"));
}

function handleInsertMessage(response) {
	let receiverId = page.receiverId;
	let onlineId = document.getElementById("online-user-box").value;
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let message = response.Data;
	if (message.SenderId == receiverId) {
		appendReceiverMessage(message);
		page.messageCounter++;
	}
	else if (message.SenderId == onlineId) {
		appendSenderMessage(message);
		page.messageCounter++;
	}
}

function handleGetUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let onlineUser = document.getElementById("online-user-box").value;

	page.users = response.Data;

	for (let i = 0; i < page.users.length; i++) {
		let user = page.users[i];
		if (user.Id != onlineUser) {
			appendUser(user);
		}
	}
}

function appendUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div>##user.Username##</div>';
	userTemplate += '<button style="margin-bottom:15px; margin-top:15px;" class="btns" onclick="tryGetSenderAndReceiver(##user.Id##)" id="message-btn-##user.Id##">Message</button>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)
		//.split("##user.Email##").join(user.Email)//.replace("##user.Email##", userModel.Email)
		//.split("##user.BirthYear##").join(user.BirthYear)//.replace("##user.BirthYear##", userModel.BirthYear)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("user-list");
	userListDiv.appendChild(userHtml);
}

function appendDeletedMessage() {
	let messageTemplate = '<div class="sender-message deleted-message deleted-message clearfix">';
	messageTemplate += '<div>Deleted Message</div>';
	messageTemplate += '</div>';

	let messageHtmlString = messageTemplate;

	let messageHtml = toDom(messageHtmlString);

	let messageListDiv = document.getElementById("message-list");
	messageListDiv.appendChild(messageHtml);

}

function appendSenderMessage(message) {
	let messageTemplate = '<div class="sender-message clearfix" id="message-id-##message.Id##">';
	messageTemplate += '<div class="sender-message-div">##message.Message##</div>';
	messageTemplate += '<button class="btns" id="delete-message-btn-##message.Id##">Delete Message</button>';
	messageTemplate += '</div>';

	let messageHtmlString = messageTemplate
		.split("##message.Id##").join(message.Id)//.replace("##message.Id##", userModel.Id)
		.split("##message.Message##").join(message.Message)//.replace("##message.Username##", userModel.Username)

	let messageHtml = toDom(messageHtmlString);

	let messageListDiv = document.getElementById("message-list");
	messageListDiv.appendChild(messageHtml);

	let deleteBtn = document.getElementById("delete-message-btn-" + message.Id);
	deleteBtn.classList.add("delete-message-btn");
	deleteBtn.onclick = tryDeleteMessage.bind(null, message.Id);

}

function appendReceiverMessage(message) {
	let messageTemplate = '<div class="receiver-message clearfix" id="message-id-##message.Id##">';
	messageTemplate += '<div class="receiver-message-div">##message.Message##</div>';
	messageTemplate += '</div>';

	let messageHtmlString = messageTemplate
		.split("##message.Id##").join(message.Id)//.replace("##user.Id##", userModel.Id)
		.split("##message.Message##").join(message.Message)//.replace("##user.Username##", userModel.Username)

	let messageHtml = toDom(messageHtmlString);

	let messageListDiv = document.getElementById("message-list");
	messageListDiv.appendChild(messageHtml);
}

function tryDeleteMessage(messageId) {
	httpRequest("api/User/DeleteMessage", "DELETE", messageId.toString(), handleDeleteMessage.bind(null, messageId), showError.bind(null, "System Error"));
}

function handleDeleteMessage(messageId, response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let messageDiv = document.getElementById("message-id-" + messageId);
	messageDiv.innerHTML = "Deleted Message";
	//messageDiv.parentNode.removeChild(messageDiv);
}

function showError(message) {
	let errorDiv = document.getElementById("error");
	errorDiv.innerHTML = message;
	errorDiv.style.display = "block";
}

function hideError() {
	let errorDiv = document.getElementById("error");
	errorDiv.style.display = "none";
}
