using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class InputEvents : MonoBehaviour{
	
	public static InputEvents Instance;
	
	public event EventHandler<MouseEventArgs> ClickEvent;
	public event EventHandler<MouseEventArgs> DownEvent;
	public event EventHandler<MouseEventArgs> UpEvent;
	public event EventHandler<MouseEventArgs> MoveEvent;
	public event EventHandler<MouseEventArgs> SwipeEvent;
	
	private List<Frame> activeElements;
	
	//private Timer clickTimer;
	private Dictionary<int,bool> clickStarted;
	private Dictionary<int,double> clickStartTime;
	private Vector2 mouseStartPosition;
	
	private Vector2 mousePosition;
	private Vector2 actualMouseDirection;
	
	void Awake(){
		Instance = this;
		clickStarted = new Dictionary<int, bool>();
		clickStartTime = new Dictionary<int, double>();
		actualMouseDirection = new Vector2(0,0);
		mousePosition = new Vector2(0,0);
		mouseStartPosition = new Vector2(0,0);
		activeElements = new List<Frame>();
		
	}
	
	void Start(){
		
	}
	
	void Update(){
		checkMove();
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_WIN
		checkTouches();
#endif
#if UNITY_STANDALONE_OSX ||  UNITY_WEBPLAYER || UNITY_EDITOR || UNITY_STANDALONE_WIN
		checkClick();
#endif
		
		
	}
	
	private void checkMove(){
		Vector2 oldMouse = mousePosition;
		mousePosition = CameraScreen.PhysicalToVirtualScreenPosition(Input.mousePosition);
		
		actualMouseDirection = new Vector2(mousePosition.x - oldMouse.x, oldMouse.y - mousePosition.y); 
		if(actualMouseDirection.magnitude != 0)
			InvokeMoveEvent(actualMouseDirection, mousePosition);
		
	}
	
	private void checkClick(){
		
		if(Input.touches.Length > 0){
			//EditorDebug.LogError("Touch Detected");
			return;
		}
			
		// Diese unglaublichen If abfragen sind nötig, da sich die Unity mit windows Touch anders verhält als
		// mit der Maus oder anderen Touchdevices
		// Unter windows gilt folgendes: Bei FingerDown sind sowohl down und Up der Mouse False
		// sobald die Maus bewegt wird gitb ButtonDown true zurück 
		// wenn man ohne die Maus zu bewegen direkt den Finger wieder hochnimmt sind down und Up true
		// Der Touch wird aber als Maus behandelt - die Input.Touches ist auf jeden Fall leer
		if(Input.GetMouseButtonDown(0) && Input.GetMouseButtonUp(0)){
			//EditorDebug.LogError("0 both");
			clickStart(0);
			clickEnd(0);	
			
		}
		else if(Input.GetMouseButtonDown(0)){
			//EditorDebug.LogError("0 down");
			clickStart(0);
		}
			
		else if(Input.GetMouseButtonUp(0)){
			//EditorDebug.LogError("0 up");
			clickEnd(0);
			
		}
			
		else if(!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonUp(0)){
			//EditorDebug.LogError("0 nix");
		}
			
		
			


		
	}
	
	private void checkTouches(){
		Touch[] touches = Input.touches;
		if(touches.Length > 0 ){
			EditorDebug.LogWarning("Touches Length > 0");
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
		mouseStartPosition = mousePosition;
		setIsDown(buttonId, true);
		setButtonDownTime(buttonId, Time.timeSinceLevelLoad);
	}
	
	private void setIsDown(int key,bool value){
		if(clickStarted.ContainsKey(key)){
			clickStarted[key] = value;
		} else
			clickStarted.Add(key, value);
	}
	
	public bool GetIsDown(int key){
		if(clickStarted.ContainsKey(key)){
			return clickStarted[key];
		}
		return false;
	}
	
	private void setButtonDownTime(int key, double time){
		if(clickStartTime.ContainsKey(key)){
			clickStartTime[key] = time;
		} else
			clickStartTime.Add(key, time);
	}
	
	private double getButtonDownTime(int key){
		if(clickStartTime.ContainsKey(key)){
			return Time.timeSinceLevelLoad - clickStartTime[key];
		}
		return 0.0;
	}
	
	
	private void clickEnd(int buttonId){
		InvokeUpEvent(buttonId);
		if(GetIsDown(buttonId)){
			Vector2 moveDirection = mousePosition - mouseStartPosition;
			float clickDistance = moveDirection.magnitude;
			if(clickDistance <= ScreenConfig.Instance.SwipeMinDistance || getButtonDownTime(buttonId) < (ScreenConfig.Instance.SwipeMinTime*Time.timeScale)){
				InvokeClickEvent(buttonId);
			} else{ // Swipe detected
					InvokeSwipeEvent(moveDirection);
			}
			
			setIsDown(buttonId, false);
			
		}
	}
	
	public void RegisterActiveElement(Frame element){
		activeElements = insertActiveElement(activeElements, element);
	}
	
	public void DeregisterActiveElement(Frame element){
		var tmpList = new List<Frame>();
		foreach(Frame f in activeElements){
			if(f != element)
				tmpList.Add(f);
		}
		activeElements = tmpList;
	}
	
	public bool IsActiveElement(Frame element){
		foreach(Frame f in activeElements){
			if(f == element){
				return true;
			}
		}
		return false;
	}
	
	public bool ActiveElementIsEmpty(){
		return activeElements.Count == 0;
	}
	
	private List<Frame> insertActiveElement(List<Frame> list, Frame element){
		var helperList = new List<Frame>();
		bool inserted = false;
		bool propagate = true;
		
		if(list.Count > 0){
			list.Sort(Frame.CompareFramesByGUIDepth);
			foreach(Frame frame in list){
				if(!inserted && propagate && element.GUIDepth < frame.GUIDepth){
					helperList.Add(element);
					propagate = element.PropagateEvents;
					inserted = true;
				}
				
				if(!propagate)
						break;
				
				helperList.Add(frame);
				propagate = frame.PropagateEvents;
					
			}	
		} 
		
		if(!inserted && propagate)
			helperList.Add(element);
		
		return helperList;
	}
	
	#region Invoke Events
	
	private void InvokeClickEvent(int buttonId){
		var handler = ClickEvent;
		if (handler == null) {
			return;
		}
		var e = new MouseEventArgs(buttonId);
		e.MousPosition = mousePosition;
		handler(this, e);
	}
	
	private void InvokeUpEvent(int buttonId){
		var handler = UpEvent;
		if (handler == null) {
			return;
		}
		var e = new MouseEventArgs(buttonId);
		e.MousPosition = mousePosition;
		handler(this, e);
	}
	
	private void InvokeDownEvent(int buttonId){
		var handler = DownEvent;
		if (handler == null) {
			return;
		}
		var e = new MouseEventArgs(buttonId);
		e.MousPosition = mousePosition;
		handler(this, e);
	}

	private void InvokeMoveEvent(Vector2 direction, Vector2 currentMousePosition){
		var handler = MoveEvent;
		if (handler == null){
			return;
		}
		var e = new MouseEventArgs(direction);
		e.MouseDown = clickStarted;
		e.MousPosition = currentMousePosition;
		handler(this, e);
	}
	
	private void InvokeSwipeEvent(Vector2 direction){
		var handler = SwipeEvent;
		if (handler == null){
			return;
		}
		direction = new Vector2(direction.x, direction.y*-1);
		var e = new MouseEventArgs(direction);
		handler(this, e);
	}
	
	#endregion 
	
}
