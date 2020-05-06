function main() {
	let userUpdateBtn = document.getElementById("user-update-btn");
	userUpdateBtn.onclick = tryInsertUser;

	tryGetOnlineUser();
	tryGetUsers();
}

function tryGetOnlineUser() {
	httpRequest("api/User/GetOnlineUser", "GET", null, handleGetOnlineUser, showError.bind(null, "System Error"));
}

function tryGetUsers() {
	httpRequest("api/User/GetActiveUsers", "GET", null, handleGetUser, showError.bind(null, "System Error"));
}

function redirectMyProfile() {
	redirect("User/MyProfile");
}

function handleGetOnlineUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.onlineUserId = response.Data.Id;
}

function handleGetUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.users = response.Data;

	for (let i = 0; i < page.users.length; i++) {
		let user = page.users[i];
		if (user.Id == page.onlineUserId) {
			appendUser(user);
        }
	}
}

function tryInsertUser() {
	let userId = page.onlineUserId;
	let username = document.getElementById("user-update-username").value;
	let email = document.getElementById("user-update-email").value;
	let password = document.getElementById("user-update-password").value;
	let birthYear = parseInt(document.getElementById("user-update-birthyear").value);

	let data = {
		Id: userId,
		Username: username,
		Email: email,
		Password: password,
		BirthYear: birthYear,
	};

	httpRequest("api/User/UpdateUser", "Put", data, handleInsertUser, showError.bind(null, "System Error"));
}

function handleInsertUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let user = response.Data;
	appendUser(user);

	handleUpdateUser();
}

function appendUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div>##user.Username## [Email: ##user.Email##] [Birth Year: ##user.BirthYear##]</div>';
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

function handleUpdateUser() {
	
	redirectMyProfile();
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