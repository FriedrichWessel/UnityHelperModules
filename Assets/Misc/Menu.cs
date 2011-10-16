using UnityEngine;

public class Menu : MonoBehaviour {
	private string logdata = "";
	public void Awake() {
	}

	public void OnGUI() {
		if(GUI.Button(new Rect(0, 0, 640, 80), "Test Facebook")) {
			int perms = Facebook.compilePermissions(Facebook.Permission.PUBLISH_STREAM);
			log("Requesting authorization...");
			Facebook.getInstance().authorize(perms, state1 => {
				if(state1 == Facebook.LOGGED_IN) {
					log("Logged in successfully.");
					log("Posting to profile...");
					Facebook.getInstance().postToStream("This an automated message. Dont pay any attention to it.", (state2, data) => {
						if(state2 == Facebook.REQUEST_SUCCESS) {
							log("Posting succeeded:");
							log(data);
						} else {
							log("Posting failed:");
							log(data);
						}
					});
				} else {
					log("Log in failed.");
				}
			});
		}
		logdata = GUI.TextArea(new Rect(0, 90, 640, 500), logdata, 200);
	}

	private void log(string s) {
		logdata += s+"\n";
	}
}
