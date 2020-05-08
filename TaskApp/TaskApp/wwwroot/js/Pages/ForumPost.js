function main() {

	let onlineUser = document.getElementById("online-user-box").value;
	let postBtn = document.getElementById("post-create-btn");

	if (onlineUser == null || onlineUser == undefined || onlineUser == "") {

		let postInput = document.getElementById("post-content");

		postInput.remove();
		postBtn.remove();

		tryGetAllPosts();
	}
	else {
		tryGetOnlineUser();
		tryGetAllPosts();

		postBtn.onclick = tryInsertPost;
    }
}

function tryGetOnlineUser() {
	httpRequest("api/User/GetOnlineUser", "GET", null, handleGetOnlineUser, showError.bind(null, "System Error"));
}
/*
function tryGetUsers() {
	httpRequest("api/User/GetActiveUsers", "GET", null, handleGetUsers, showError.bind(null, "System Error"));
}*/

function tryGetAllPosts() {
	httpRequest("api/Forum/GetAllForumPost", "GET", null, handleGetPosts, showError.bind(null, "System Error"));
}

function redirectForumPost() {
	redirect("Forum/ForumPost");
}

function handleGetOnlineUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.onlineUserId = response.Data.Id;
	//page.onlineUsername = response.Data.Username;
}

function handleGetUsers(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	
	page.users = response.Data;
}

function handleGetPosts(response) {
	let userId = page.onlineUserId;
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.posts = response.Data;

	for (let i = (page.posts.length - 1); i >= 0; i--) {
		let post = page.posts[i];
		if (post.UserId == userId) {
			appendNewForumPost(post);
			appendDeleteButton(post)
		}
		else {
			appendNewForumPost(post);
        }
	}
}

function tryInsertPost() {
	let post = document.getElementById("post-content").value;
	let userId = page.onlineUserId;
	//let username = page.onlineUsername;

	let data = {
		Post: post,
		UserId: userId,
		//Username: username,
	};

	httpRequest("api/Forum/CreateForumPost", "POST", data, handleInsertForumPost, showError.bind(null, "System Error"));
}

function handleInsertForumPost(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	hideError();

	let forumPost = response.Data;
	appendNewForumPost(forumPost);
	redirectForumPost();
}

function appendNewForumPost(Post) {
	let forumPostTemplate = '<div id="post-id-##Post.Id##">';
	forumPostTemplate += '<div style="margin-bottom:15px;">##Post.Post##</div>';
	forumPostTemplate += '<div style="margin-bottom:15px;font-weight:bold;">##Post.Username## </div>';
	forumPostTemplate += '</div>';

	let postHtmlString = forumPostTemplate
		.split("##Post.Id##").join(Post.Id)//.replace("##user.Id##", userModel.Id)
		.split("##Post.Post##").join(Post.Post)//.replace("##user.Id##", userModel.Id)
		.split("##Post.UserId##").join(Post.UserId)//.replace("##user.Id##", userModel.Id)
		.split("##Post.Username##").join(Post.Username)//.replace("##user.Id##", userModel.Id)

	let userHtml = toDom(postHtmlString);

	let forumPostListDiv = document.getElementById("post-list");
	forumPostListDiv.appendChild(userHtml);
}

function appendDeleteButton(Post) {
	let forumPostTemplate = '<div style="margin-bottom:10px;"><button class="btns" id="post-delete-btn-##Post.Id##">Delete Post</button></div>';

	let postHtmlString = forumPostTemplate
		.split("##Post.Id##").join(Post.Id)//.replace("##user.Id##", userModel.Id)
		.split("##Post.Post##").join(Post.Post)//.replace("##user.Id##", userModel.Id)
		.split("##Post.UserId##").join(Post.UserId)//.replace("##user.Id##", userModel.Id)
		.split("##Post.Username##").join(Post.Username)//.replace("##user.Id##", userModel.Id)

	let userHtml = toDom(postHtmlString);

	let forumPostListDiv = document.getElementById("post-list");
	forumPostListDiv.appendChild(userHtml);

	let deleteBtn = document.getElementById("post-delete-btn-" + Post.Id);
	deleteBtn.onclick = tryDeletePost.bind(null, Post.Id);
}

function tryDeletePost(postId) {
	httpRequest("api/Forum/DeletePost", "DELETE", postId.toString(), handleDeletePost.bind(null, postId), showError.bind(null, "System Error"));
}

function handleDeletePost(postId, response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let postDiv = document.getElementById("post-id-" + postId);
	postDiv.parentNode.removeChild(postDiv);
	redirectForumPost();
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
