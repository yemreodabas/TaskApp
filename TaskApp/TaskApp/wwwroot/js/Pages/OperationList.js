function main() {
	let operationCreateBtn = document.getElementById("operation-create-btn");
	operationCreateBtn.onclick = tryInsertOperation;

	let opList = document.getElementById("operation-list");
	opList.counter = 1;

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
	let opList = document.getElementById("operation-list");
	let counter = opList.counter;

	let missionId = parseInt(document.getElementById("missionid-box").value);
	page.operations = response.Data;

	for (let i = 0; i < page.operations.length; i++) {
		let operation = page.operations[i];
		if (operation.MissionId == missionId) {
			if (operation.OperationStatus == 0) {

				opList.counter++;
				appendOperation(operation);
			}
			else {

				opList.counter++;
				appendOperationDone(operation);
			}
		}
	}
}

function tryInsertOperation() {
	let opList = document.getElementById("operation-list");
	let counter = opList.counter;
	let name = document.getElementById("operation-name").value;
	let missionId = document.getElementById("missionid-box").value;
	let missionIdToInt = parseInt(missionId);

	if (counter > 9)
	{
		showError("operations cannot be greater than 9");
		return;
	}
	let data = {
		Name: name,
		MissionId: missionIdToInt,
	};

	httpRequest("api/Operation/CreateOperation", "POST", data, handleInsertOperation, showError.bind(null, "System Error"));
}

function handleInsertOperation(response) {
	let opList = document.getElementById("operation-list");
	let counter = opList.counter;
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	hideError();

	if (counter <= 9) {
		let operation = response.Data;
		appendOperation(operation);
		opList.counter++;
	}
	else {
		showError("operations cannot be greater than 9");
    }
	
}

function appendOperation(operation) {
	let operationTemplate = '<div id="operation-id-##operation.Id##">';
	operationTemplate += '<div id="operation-name-##operation.Id##">##operation.Name##</div>';
	operationTemplate += '<div style="margin-bottom:10px;"><button id="operation-delete-btn-##operation.Id##">Delete Operation</button></div>';
	operationTemplate += '<div style="margin-bottom:25px;"><button id="operation-done-btn-##operation.Id##">Done!</button></div>';
	operationTemplate += '</div>';

	let storyHtmlString = operationTemplate
		.split("##operation.Id##").join(operation.Id)//.replace("##user.Id##", userModel.Id)
		.split("##operation.Name##").join(operation.Name)//.replace("##user.Id##", userModel.Id)
		.split("##operation.MissionId##").join(operation.MissionId)//.replace("##user.Id##", userModel.Id)

	let operationHtml = toDom(storyHtmlString);

	let userListDiv = document.getElementById("operation-list");
	userListDiv.appendChild(operationHtml);

	let deleteBtn = document.getElementById("operation-delete-btn-" + operation.Id);
	deleteBtn.onclick = tryDeleteMission.bind(null, operation.Id);

	let doneBtn = document.getElementById("operation-done-btn-" + operation.Id);
	doneBtn.onclick = tryDoneOperation.bind(null, operation.Id);
}

function appendOperationDone(operation) {
	let operationTemplate = '<div id="operation-id-##operation.Id##">';
	operationTemplate += '<div id="operation-name-##operation.Id##">##operation.Name##</div>';
	operationTemplate += '<div style="margin-bottom:10px;"><button id="operation-delete-btn-##operation.Id##">Delete Operation</button></div>';
	operationTemplate += '</div>';

	let storyHtmlString = operationTemplate
		.split("##operation.Id##").join(operation.Id)//.replace("##user.Id##", userModel.Id)
		.split("##operation.Name##").join(operation.Name)//.replace("##user.Id##", userModel.Id)
		.split("##operation.MissionId##").join(operation.MissionId)//.replace("##user.Id##", userModel.Id)

	let operationHtml = toDom(storyHtmlString);

	let userListDiv = document.getElementById("operation-list");
	userListDiv.appendChild(operationHtml);

	let deleteBtn = document.getElementById("operation-delete-btn-" + operation.Id);
	deleteBtn.onclick = tryDeleteMission.bind(null, operation.Id);

	tryDoneOperation.bind(null, operation.Id);

	let operationLine = document.getElementById("operation-name-" + operation.Id);
	operationLine.className = "operation-done";
}

function tryDeleteMission(operationId) {
	httpRequest("api/Operation/DeleteOperation", "DELETE", operationId.toString(), handleDeleteOperation.bind(null, operationId), showError.bind(null, "System Error"));
}

function tryDoneOperation(operationId) {
	httpRequest("api/Operation/UpdateOperation", "Put", operationId.toString(), handleDoneOperation.bind(null, operationId), showError.bind(null, "System Error"));
}

function handleDeleteOperation(operationId, response) {
	let opList = document.getElementById("operation-list");
	let counter = opList.counter;
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let operationDiv = document.getElementById("operation-id-" + operationId);
	operationDiv.parentNode.removeChild(operationDiv);

	opList.counter--;
	hideError();
}

function handleDoneOperation(operationId) {

	let doneBtn = document.getElementById("operation-done-btn-" + operationId);
	doneBtn.classList.add("hidden");
	let operationLine = document.getElementById("operation-name-" + operationId);
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