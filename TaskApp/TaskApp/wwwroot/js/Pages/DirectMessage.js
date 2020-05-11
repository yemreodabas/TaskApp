function main() {

	page.users = tryGetUsers();
}

function tryGetUsers() {
	httpRequest("api/User/GetActiveUsers", "GET", null, handleGetUser, showError.bind(null, "System Error"));
}

function tryGetSenderAndReceiver(receiverId) {

	for (let i = 0; i < page.users.length; i++) {
		if (page.users[i].Id == receiverId) {
			page.currentlyTalkingName = page.users[i].Username;
        }
    }

	page.receiverId = receiverId;

	let currentlyTalkingUserName = document.createElement("h2");
	document.getElementById("message-list").appendChild(currentlyTalkingUserName);
	currentlyTalkingUserName.innerHTML = page.currentlyTalkingName;
	currentlyTalkingUserName.id = "current-username";
	currentlyTalkingUserName.classList.add("current-username");


	tryGetMessageById(receiverId);

	let title = document.createElement("h2");
	let message = document.createElement("input");
	let sendMessageBtn = document.createElement("button");

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
}

function tryGetMessageById(receiverId) {
	httpRequest("api/User/GetMessageById/?receiverId=" + receiverId, "GET", null, handleGetMessage, showError.bind(null, "System Error"));
}

function tryGetReceiverById(receiverId) {
	httpRequest("api/User/GetReceiverById/?receiverId=" + receiverId, "GET", null, handleGetReceiverMessage, showError.bind(null, "System Error"));
}
/*
function tryGetUsers() {
	httpRequest("api/User/GetActiveUsers", "GET", null, handleGetUsers, showError.bind(null, "System Error"));
}

function redirectDirectMessage(userId) {
	redirect("User//" + userId);
}*/

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
		if (message != null || message != "") {
			if (message.SenderId == receiverId) {
				appendReceiverMessage(message);
			}
			else if (message.SenderId == onlineId) {
				appendSenderMessage(message);
			}
		}
	}
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
	}
	else if (message.SenderId == onlineId) {
		appendSenderMessage(message);
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

function appendSenderMessage(message) {
	let messageTemplate = '<div class="clearfix" id="message-id-##message.Id##">';
	messageTemplate += '<div class="sender-message">##message.Message##</div>';
	messageTemplate += '</div>';

	let messageHtmlString = messageTemplate
		.split("##message.Id##").join(message.Id)//.replace("##message.Id##", userModel.Id)
		.split("##message.Message##").join(message.Message)//.replace("##message.Username##", userModel.Username)

	let messageHtml = toDom(messageHtmlString);

	let messageListDiv = document.getElementById("message-list");
	messageListDiv.appendChild(messageHtml);
}

function appendReceiverMessage(message) {
	let messageTemplate = '<div class="clearfix" id="message-id-##message.Id##">';
	messageTemplate += '<div class="receiver-message">##message.Message##</div>';
	messageTemplate += '</div>';

	let messageHtmlString = messageTemplate
		.split("##message.Id##").join(message.Id)//.replace("##user.Id##", userModel.Id)
		.split("##message.Message##").join(message.Message)//.replace("##user.Username##", userModel.Username)

	let messageHtml = toDom(messageHtmlString);

	let messageListDiv = document.getElementById("message-list");
	messageListDiv.appendChild(messageHtml);
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
