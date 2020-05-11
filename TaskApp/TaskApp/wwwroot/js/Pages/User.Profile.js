function main() {

	tryGetUsers();
	
	let userId = document.getElementById("userid-box").value;
	let onlineUser = document.getElementById("online-user-box").value;

	if (onlineUser == null || onlineUser == undefined || onlineUser == "") {

		tryGetTargets();
		tryGetFollowers;
	}
	else {
		tryGetTargetById(userId);
		tryGetFollowerById(userId)
    }
}

function tryGetUsers() {
	httpRequest("api/User/GetActiveUsers", "GET", null, handleGetUser, showError.bind(null, "System Error"));
}

function tryGetTargets() {
	httpRequest("api/User/GetTargetUsers", "GET", null, handleGetTargetUsers, showError.bind(null, "System Error"));
}

function tryGetFollowers() {
	httpRequest("api/User/GetFollowUsers", "GET", null, handleGetFollowersUsers, showError.bind(null, "System Error"));
}

function tryGetTargetById(userId) {
	httpRequest("api/User/GetTargetById/?userId="+userId, "GET", null, handleGetTargets, showError.bind(null, "System Error"));
}

function tryGetFollowerById(userId) {
	httpRequest("api/User/GetFollowerById/?userId="+userId, "GET", null, handleGetFollowers, showError.bind(null, "System Error"));
}

function handleGetTargetUsers(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.onlineUserTargets = response.Data;
}

function handleGetFollowersUsers(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.onlineUserFollowers = response.Data;
}

function handleGetTargets(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let onlineUserTargets = page.onlineUserTargets;
	page.targets = response.Data;

	for (let i = 0; i < page.targets.length; i++)
	{
		let targetUser = page.targets[i];
		if (targetUser != null || targetUser != "") {
			appendTargetUser(targetUser);
        }

		for (let j = 0; j < onlineUserTargets.length; j++) {
			let onlineUserTarget = onlineUserTargets[j];

			if (targetUser.Id == onlineUserTarget.Id) {
				handleUnFollowUser(targetUser.Id);
			}
		}
    }
}

function handleGetFollowers(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	let onlineUserFollowers = page.onlineUserFollowers;
	page.followers = response.Data;

	for (let i = 0; i < page.followers.length; i++) {
		let followerUser = page.followers[i];
		if (followerUser != null || followerUser != "") {
			appendFollowerUser(followerUser);
		}

		for (let j = 0; j < onlineUserFollowers.length; j++) {
			let onlineUserFollower = onlineUserFollowers[j];

			if (followerUser.Id == onlineUserFollower.Id) {
				handleUnFollowUser(followerUser.Id);
			}
		}
	}
}

function handleGetUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let userId = document.getElementById("userid-box").value;

	page.users = response.Data;

	for (let i = 0; i < page.users.length; i++) {
		let user = page.users[i];
		if (user.Id == userId) {
			appendUser(user);
			break;
        }
	}
}

function appendUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div style="margin-bottom:15px;">##user.Username##</div>';
	userTemplate += '<h2 style="margin-bottom:15px;">TARGETS</h2>';
	userTemplate += '<div style="margin-bottom:15px; id="target-list" ></div>';
	userTemplate += '<h2 style="margin-bottom:15px;">Follower</h2>';
	userTemplate += '<div style="margin-bottom:15px; id="follow-list" ></div>';
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

function appendTargetUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div style="margin-bottom:15px;">##user.Username##</div>';
	userTemplate += '<div style="margin-bottom:25px;"><button class="btns" id="follow-user-btn-##user.Id##">FOLLOW</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("target-list");
	userListDiv.appendChild(userHtml);

	let followBtn = document.getElementById("follow-user-btn-" + user.Id);
	followBtn.onclick = tryFollowUser.bind(null, user.Id);
}

function appendFollowerUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div style="margin-bottom:15px;">##user.Username##</div>';
	userTemplate += '<div style="margin-bottom:25px;"><button id="follow-user-btn-##user.Id##">FOLLOW</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("follow-list");
	userListDiv.appendChild(userHtml);

	let followBtn = document.getElementById("follow-user-btn-" + user.Id);
	followBtn.onclick = tryFollowUser.bind(null, user.Id);
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