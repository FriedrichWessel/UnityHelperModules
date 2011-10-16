using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class Facebook : MonoBehaviour {
	public delegate void LoginCallback(int result);
	public delegate void RequestCallback(int result, string data);
	private LoginCallback lcb;
	private RequestCallback rcb;
	public string AppId;

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void _init(string AppId);
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void _authorize(int permissions);
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void _logout();
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void _graphRequest(string methodname, string[] param, string method);
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void _deleteSession();

	private static Facebook instance = null;

	public enum Permission {
		PUBLISH_STREAM = 0,
	}

	public static int REQUEST_SUCCESS = 0;
	public static int REQUEST_FAIL = 1;

	public static int LOGGED_IN = 0;
	public static int LOGGED_OUT = 1;

	private void Awake() {
		instance = this;
		_init(AppId);
	}

	public static Facebook getInstance() {
		return instance;
	}

	public void authorize(int permissions, LoginCallback cb) {
		lcb = cb;
		_authorize(permissions);
	}

	public void logout() {
		_logout();
	}

	public void postToStream(string message, RequestCallback cb) {
		rcb = cb;
		_graphRequest("feed", new string[] {"message", message}, "POST");
	}

	public void postToStream(string message, string picture, string link, string name, string caption, string description, RequestCallback cb) {
		rcb = cb;
		_graphRequest("feed", new string[] {"message", message, "picture", picture, "link", link, "name", name, "caption", caption, "description", description}, "POST");
	}

	public void deleteSession() {
		_deleteSession();
	}

	public static int compilePermissions(params Permission[] permissions) {
		int result = 0;
		foreach(int i in permissions) {
			result |= 1<<i;
		}
		return result;
	}

	// Callbacks

	public void loggedIn() {
		if(lcb == null){
			return;
		}
		lcb(LOGGED_IN);
	}

	public void loggedOut() {
		if(lcb == null){
			return;
		}
		lcb(LOGGED_OUT);
	}

	public void requestFailed(string error) {
		if(rcb == null) {
			return;
		}
		rcb(REQUEST_FAIL, error);
	}

	public void requestSucceeded(string data) {
		rcb(REQUEST_SUCCESS, data);
	}

}
