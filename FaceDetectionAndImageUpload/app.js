var API_KEY = "5305005f8dd2426696adedfb6b159da1";
var API_SECRET = "325e64602e874234af4d833e073e6f99";

function uploadFile() {
	var formData = new FormData();
	formData.append("fileToUpload", document.getElementById('fileToUpload').files[0]);
	
	var phpServer = new XMLHttpRequest();
	phpServer.addEventListener("load", uploadComplete, false);
	phpServer.addEventListener("error", uploadFailed, false);
	phpServer.addEventListener("abort", uploadCancelled, false);
	phpServer.open("POST", "upload.php");
	phpServer.send(formData);
	console.log("Uploading...");
}

function uploadComplete(event) {
	console.log(event.target.responseText);
	//get file name and perform get request
	
	$.get (
		"http://api.skybiometry.com/fc/faces/detect.json?api_key=" + API_KEY + "&api_secret=" + API_SECRET + "&urls=http://davidvuong.ca/Upload/uploads/" + event.target.responseText + "&attributes=all", function(data) {
			console.log(data);
		});
}

function uploadFailed(event) {
	console.log("An error occured while uploading the file.");
}

function uploadCancelled(event) {
	console.log("The upload has been cancelled by the user or the browser dropped the connection.");
}