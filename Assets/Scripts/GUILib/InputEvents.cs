using UnityEngine;
using System.Collections;
using System;

public class InputEvents : MonoBehaviour{

	
	public float ClickTimeInSeconds = 0.1f;
	
	public static InputEvents Instance;
	
	public event EventHandler<MouseEventArgs> ClickEvent;
	public event EventHandler<MouseEventArgs> DownEvent;
	public event EventHandler<MouseEventArgs> UpEvent;
	public event EventHandler<MouseEventArgs> MoveEvent;
	
	private Timer clickTimer;
	private bool clickStarted = false;
	
	private float mouseX; 
	private float mouseY;
	
	void Awake(){
		Instance = this;
		clickTimer = new Timer();
		clickTimer.TimerFinished += OnClickTimerFinished;
	}
	
	void Update(){
		checkMove();
#if UNITY_IPHONE || UNITY_ANDROID
		checkTouches();
#endif
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
		checkClick();
#endif
		
		
	}
	
	private void checkMove(){
		float oldMouseX = mouseX;
		float oldMouseY = mouseY;
		mouseX = Input.mousePosition.x;
		mouseY = Input.mousePosition.y;
		Vector2 direction = new Vector2(oldMouseX - mouseX, oldMouseY - mouseY);
		if(direction.magnitude != 0)
			InvokeMoveEvent(direction);
		
	}
	private void checkClick(){
		if(Input.touches.Length > 0)
			return;
		if(Input.GetMouseButtonDown(0)){
			clickStart(0);
		} else if(Input.GetMouseButtonUp(0)){
			clickEnd(0);
		}
	}
	
	private void checkTouches(){
		Touch[] touches = Input.touches;
		if(touches.Length > 0 ){
			foreach(Touch touch in touches){
				if(touch.phase == TouchPhase.Began){
					clickStart(touch.fingerId);
				} else if(touch.phase == TouchPhase.Ended){
					clickEnd(touch.fingerId);
				}
				
					
			}
		}
	}
	
	private void clickStart(int buttonId){
		InvokeDownEvent(buttonId);
		clickTimer.StartTimer(ClickTimeInSeconds);
		clickStarted = true;
	}
	
	private void clickEnd(int buttonId){
		InvokeUpEvent(buttonId);
		if(clickStarted){
			InvokeClickEvent(buttonId);
			clickStarted = false;
		}
	}
	
	
	private void OnClickTimerFinished(object sender, EventArgs e){
		clickStarted = false;
	}
	
	private void InvokeClickEvent(int buttonId){
		var handler = ClickEvent;
		if (handler == null) {
			return;
		}
		var e = new MouseEventArgs(buttonId);
		handler(this, e);
	}
	
	private void InvokeUpEvent(int buttonId){
		var handler = UpEvent;
		if (handler == null) {
			return;
		}
		var e = new MouseEventArgs(buttonId);
		handler(this, e);
	}
	
	private void InvokeDownEvent(int buttonId){
		var handler = DownEvent;
		if (handler == null) {
			return;
		}
		var e = new MouseEventArgs(buttonId);
		handler(this, e);
	}

	private void InvokeMoveEvent(Vector2 direction){
		var handler = MoveEvent;
		if (handler == null){
			return;
		}
		var e = new MouseEventArgs(direction);
		handler(this, e);
	}
	
}
