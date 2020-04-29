function main() {
	let missionCreateBtn = document.getElementById("mission-create-btn");
	missionCreateBtn.onclick = tryInsertMission;

	TryGetActiveMission();
}

function TryGetActiveMission() {
	httpRequest("api/Mission/GetActiveMissions", "GET", null, handleGetMissions, showError.bind(null, "System Error"));
}

function redirectOperation(missionId) {
	redirect("User/MissionDetail/" + missionId);
}

function handleGetOnlineUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.onlineUserId = response.Data.Id;
	page.onlineUsername = response.Data.Username;

}

function handleGetMissions(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.missions = response.Data;

	for (let i = 0; i < page.missions.length; i++) {
		let mission = page.missions[i];
		appendMission(mission);
	}
}

function tryInsertMission() {
	let name = document.getElementById("mission-name").value;
	let userid = page.onlineUserId;
	let missionUsername = page.onlineUsername;

	let data = {
		Name: name,
		UserId: userid,
		MissionUsername: missionUsername,
	};

	httpRequest("api/Mission/CreateMission", "POST", data, handleInsertMission, showError.bind(null, "System Error"));
}

function handleInsertMission(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	hideError();

	let mission = response.Data;
	appendMission(mission);
}

function appendMission(mission) {
	let missionTemplate = '<div id="mission-id-##mission.Id##">';
	missionTemplate += '<div>##mission.Name## [User: ##mission.MissionUsername##]</div>';
	missionTemplate += '<button onclick="redirectOperation(##mission.Id##)" id="operation-detail-btn-##mission.Id##">Operation Detail</button>';
	missionTemplate += '<div style="margin-bottom:20px;"><button id="mission-delete-btn-##mission.Id##">Delete Mission</button></div>';
	missionTemplate += '</div>';

	let storyHtmlString = missionTemplate
		.split("##mission.Id##").join(mission.Id)//.replace("##user.Id##", userModel.Id)
		.split("##mission.Name##").join(mission.Name)//.replace("##user.Id##", userModel.Id)
		.split("##mission.UserId##").join(mission.UserId)//.replace("##user.Id##", userModel.Id)
		.split("##mission.MissionUsername##").join(mission.MissionUsername)//.replace("##user.Id##", userModel.Id)

	let missionHtml = toDom(storyHtmlString);

	let userListDiv = document.getElementById("mission-list");
	userListDiv.appendChild(missionHtml);

	let deleteBtn = document.getElementById("mission-delete-btn-" + mission.Id);
	deleteBtn.onclick = tryDeleteMission.bind(null, mission.Id);
}

function tryDeleteMission(missionId) {
	httpRequest("api/Mission/DeleteMission", "DELETE", missionId.toString(), handleDeleteMission.bind(null, missionId), showError.bind(null, "System Error"));
}

function handleDeleteMission(missionId, response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let missionDiv = document.getElementById("mission-id-" + missionId);
	missionDiv.parentNode.removeChild(missionDiv);
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
