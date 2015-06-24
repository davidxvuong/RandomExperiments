var delimiter = "EOD";
var ws;
var serverLanguage = "Other";

$(document).ready(function () {
	$('#name').keydown(function(event) {
		var key = event.keyCode || event.which;
		
		if (key == 13) {
			startConnection();
		}
	});
	
	$('#message').keydown(function(event) {
		var key = event.keyCode || event.which;
		
		if (key == 13) {
			sendMessage();
		}
	});
});


function startConnection() {
	var name = document.getElementById('name').value;
	if (name == "")
	{
		alert ("Name field is empty!");
		return;
	}
	ws = new WebSocket("ws://localhost:8080/Server/test");
	
	ws.onopen = function() {
		document.getElementById("text").innerHTML += getDateTime() + ": Connection established.<br>";
		console.log("Connected! Sending name...");
		ws.send(name + delimiter);
		console.log("Sent");
	};
	
	ws.onmessage = function(evt) {
		if (evt.data != "C#"){
			document.getElementById("text").innerHTML += getDateTime() + ": Server - " + evt.data + "<br>";
			console.log("Server: " + evt.data);
		}
		else 
			serverLanguage = evt.data;
	};
	
	ws.onerror = function(err) {
		document.getElementById("text").innerHTML += getDateTime() + ": An error in the WebSocket connection occurred. Connection closed.<br>";
	};
	
	ws.onclose = function() {
		document.getElementById("text").innerHTML += getDateTime() + ": Connection closed.<br>";
		console.log("Closed connection");
	};
}

function sendMessage() {
	if ("WebSocket" in window) {
		var input = document.getElementById('message').value;
		if (input == "")
		{
			alert("Message field is empty!");
			return;
		}
		else {
			
			document.getElementById("text").innerHTML += getDateTime() + ": Client - " + input + "<br>";
			ws.send(input + delimiter);
		}
	}
}

function disconnect() {
	if (serverLanguage == "C#")
		ws.send("Close" + delimiter);
	ws.close();
}

function getDateTime() {
	var currentdate = new Date(); 
	var datetime = currentdate.getDate() + "/"
					+ (currentdate.getMonth()+1)  + "/" 
					+ currentdate.getFullYear() + " @ "  
					+ currentdate.getHours() + ":"  
					+ currentdate.getMinutes() + ":" 
					+ currentdate.getSeconds();
	return datetime;
}

window.addEventListener("unload", disconnect, false); 