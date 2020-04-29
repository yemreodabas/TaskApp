function main() {

	TryGetActiveOperation();
}

function TryGetActiveOperation() {
	httpRequest("api/Operation/GetActiveOperations", "GET", null, handleGetOperation, showError.bind(null, "System Error"));
}

function handleGetOperation(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	let missionId = parseInt(document.getElementById("missionid-box").value);
	page.operations = response.Data;

	for (let i = 0; i < page.operations.length; i++) {
		let operation = page.operations[i];
		if (operation.MissionId == missionId) {
			if (operation.OperationStatus == 0) {

				appendOperation(operation);
			}
			else {
				appendOperationDone(operation);
			}
		}
	}
}

function tryInsertOperation() {
	let name = document.getElementById("operation-name").value;
	let missionId = document.getElementById("missionid-box").value;
	let missionIdToInt = parseInt(missionId);

	let data = {
		Name: name,
		MissionId: missionIdToInt,
	};

	httpRequest("api/Operation/CreateOperation", "POST", data, handleInsertOperation, showError.bind(null, "System Error"));
}

function handleInsertOperation(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	hideError();
 
	let operation = response.Data;
	appendOperation(operation);
	
}

function appendOperation(operation) {
	let operationTemplate = '<div id="operation-id-##operation.Id##">';
	operationTemplate += '<div id="operation-name-##operation.Id##">##operation.Name##</div>';
	operationTemplate += '</div>';

	let storyHtmlString = operationTemplate
		.split("##operation.Id##").join(operation.Id)//.replace("##user.Id##", userModel.Id)
		.split("##operation.Name##").join(operation.Name)//.replace("##user.Id##", userModel.Id)
		.split("##operation.MissionId##").join(operation.MissionId)//.replace("##user.Id##", userModel.Id)

	let operationHtml = toDom(storyHtmlString);

	let userListDiv = document.getElementById("operation-list");
	userListDiv.appendChild(operationHtml);
}

function appendOperationDone(operation) {
	let operationTemplate = '<div id="operation-id-##operation.Id##">';
	operationTemplate += '<div id="operation-name-##operation.Id##">##operation.Name##</div>';
	operationTemplate += '</div>';

	let storyHtmlString = operationTemplate
		.split("##operation.Id##").join(operation.Id)//.replace("##user.Id##", userModel.Id)
		.split("##operation.Name##").join(operation.Name)//.replace("##user.Id##", userModel.Id)
		.split("##operation.MissionId##").join(operation.MissionId)//.replace("##user.Id##", userModel.Id)

	let operationHtml = toDom(storyHtmlString);

	let userListDiv = document.getElementById("operation-list");
	userListDiv.appendChild(operationHtml);

	let operationLine = document.getElementById("operation-name-" + operation.Id);
	operationLine.className = "operation-done";
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