function main() {

	let onlineUser = document.getElementById("online-user-box").value;

	if (onlineUser == null || onlineUser == undefined || onlineUser == "") {

		tryGetUsers();
	}
	else {
		tryGetNotTargets();
		tryGetTargets();
    }
}

function tryGetUsers() {
	httpRequest("api/User/GetActiveUsers", "GET", null, handleGetUsers, showError.bind(null, "System Error"));
}

function tryGetNotTargets() {
	httpRequest("api/User/GetNotTargetUsers", "GET", null, handleNotGetTargets, showError.bind(null, "System Error"));
}

function tryGetTargets() {
	httpRequest("api/User/GetTargetUsers", "GET", null, handleGetTargets, showError.bind(null, "System Error"));
}
/*
function tryGetFollowers() {
	httpRequest("api/User/GetFollowUsers", "GET", null, handleGetFollower, showError.bind(null, "System Error"));
}*/

function redirectUserProfile(userId) {
	redirect("User/UserProfile/" + userId);
}
/*
function handleGetOnlineUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	
	page.onlineUserId = response.Data.Id;
}*/

function handleNotGetTargets(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.targets = response.Data;

	for (let i = 0; i < page.targets.length; i++) {
		appendOnlineUser(page.targets[i])
    }
}

function handleGetTargets(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.targets = response.Data;

	for (let i = 0; i < page.targets.length; i++) {
		appendOnlineUser(page.targets[i])
		handleUnFollowUser(page.targets[i].Id)
	}
}
/*
function handleGetFollower(response) {
	let onlineUser = page.onlineUserId;
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	if (onlineUser != null) {
		page.followers = response.Data;
	}
}

function handleGetTargets(response) {
	let onlineUser = page.onlineUserId;
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	if (onlineUser != null) {
		page.targets = response.Data;
	}
}*/

function handleGetUsers(response) {
	//let onlineUser = page.onlineUserId;
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	
	page.users = response.Data;

	for (let i = 0; i < page.users.length; i++) {
		let user = page.users[i];
		appendUser(user);
    }
			
}

function appendOnlineUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<a class="user-list-username" href="">##user.Username## [Email: ##user.Email##] [Birth Year: ##user.BirthYear##]</a>';
	//userTemplate += '<button style="margin-bottom:15px; margin-top:15px;" class="btns" onclick="redirectUserProfile(##user.Id##)" id="user-profile-btn-##user.Id##">User Profile</button>';
	userTemplate += '<div style="margin-bottom:25px;"><button class="btns" id="follow-user-btn-##user.Id##">FOLLOW</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)
		.split("##user.Email##").join(user.Email)//.replace("##user.Email##", userModel.Email)
		.split("##user.BirthYear##").join(user.BirthYear)//.replace("##user.BirthYear##", userModel.BirthYear)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("user-list");
	userListDiv.appendChild(userHtml);

	let followBtn = document.getElementById("follow-user-btn-" + user.Id);
	followBtn.onclick = tryFollowUser.bind(null, user.Id);
}

function appendUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<a class="user-list-username" href="">##user.Username## [Email: ##user.Email##] [Birth Year: ##user.BirthYear##]</a>';
	//userTemplate += '<button style="margin-bottom:15px; margin-top:15px;" class="btns" onclick="redirectUserProfile(##user.Id##)" id="user-profile-btn-##user.Id##">User Profile</button>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)
		.split("##user.Email##").join(user.Email)//.replace("##user.Email##", userModel.Email)
		.split("##user.BirthYear##").join(user.BirthYear)//.replace("##user.BirthYear##", userModel.BirthYear)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("user-list");
	userListDiv.appendChild(userHtml);
}

function tryFollowUser(userId) {
	httpRequest("api/User/FollowUser", "POST", userId.toString(), handleUnFollowUser.bind(null, userId), showError.bind(null, "System Error"));
}

function tryUnFollowUser(userId) {
	httpRequest("api/User/UnFollowUser", "DELETE", userId.toString(), handleFollowUser.bind(null, userId), showError.bind(null, "System Error"));
}

function handleFollowUser(userId) {

	let followBtn = document.getElementById("follow-user-btn-" + userId);
	followBtn.onclick = tryFollowUser.bind(null, userId);
	followBtn.innerHTML = "FOLLOW";
}

function handleUnFollowUser(userId) {

	let followBtn = document.getElementById("follow-user-btn-" + userId);
	followBtn.onclick = tryUnFollowUser.bind(null, userId);
	followBtn.innerHTML = "UNFOLLOW";
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
