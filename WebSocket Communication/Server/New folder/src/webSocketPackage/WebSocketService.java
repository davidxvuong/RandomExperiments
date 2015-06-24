package webSocketPackage;

import javax.websocket.OnClose;
import javax.websocket.OnError;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;
import javax.websocket.server.ServerEndpoint;

@ServerEndpoint("/test")
public class WebSocketService {
	@OnOpen
	public void handleOpen() {
		System.out.println("Client connected!");
	}
	
	@OnClose
	public void handleClose() {
		System.out.println("Client closed.");
	}
	
	@OnError
	public void handleError(Throwable t) {
		t.printStackTrace();
	}
	
	@OnMessage
	public String handleMessage(String msg) {
		System.out.println("Client: " + msg);
		System.out.println("Echo message: " + msg);
		return ("Echo message: " + msg);
	}
}
