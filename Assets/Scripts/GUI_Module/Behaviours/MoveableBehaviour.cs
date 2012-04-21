using UnityEngine;
using System.Collections;
using System;

public class MoveableBehaviour : MonoBehaviour {

	public event EventHandler<MoveableEventArgs> IsDone;
	
	public float MovementDuration = 0.5f; // in seconds
	
	public Panel element{
		get;
		private set;
	}
	private Vector2 diffToTargetSize;
	private Vector2 diffToTargetPosition;
	private bool isMoving = false;
	private float movingTime = 0.0f;
	private float currentMovementDuration;
	
	// Use this for initialization
	void Start () {
		StartOverride();
	}
	
	void Awake(){
		AwakeOverride();
	}
	
	protected virtual void AwakeOverride(){
		element = gameObject.GetComponent<Panel>() as Panel;
		if(element == null){
			EditorDebug.LogError("Moveable Behaviour is not attached to a Panel!");
		}
		currentMovementDuration = MovementDuration;
	}
	protected void StartOverride(){
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		UpdateOverride();
	}
	
	protected virtual void UpdateOverride(){
		if(isMoving){
			float step;
			if(currentMovementDuration > 0)
				step = Time.deltaTime / currentMovementDuration; 
			else 
				step = 1.0f;
			
		
			
			var newPosition = new Rect(diffToTargetPosition.x * step,
			                                         diffToTargetPosition.y * step,
			                                         diffToTargetSize.x * step,
			                                         diffToTargetSize.y * step );
			
			newPosition = new Rect(element.VirtualRegionOnScreen.x + newPosition.x,
			                       element.VirtualRegionOnScreen.y + newPosition.y,
			                       element.VirtualRegionOnScreen.width + newPosition.width,
			                       element.VirtualRegionOnScreen.height + newPosition.height);
			
			/*EditorDebug.Log("Duration: " + currentMovementDuration);
			EditorDebug.Log("Step: " + step);
			EditorDebug.Log("TargetPos: " + diffToTargetPosition);
			EditorDebug.Log("Move: " + newPosition);*/
			
			element.VirtualRegionOnScreen = newPosition;
			element.UpdateElement();
			movingTime += Time.deltaTime;
			if(movingTime >= currentMovementDuration){
				//EditorDebug.LogWarning("Finished: " + gameObject.name + " Time: " + currentMovementDuration);
				//EditorDebug.Log("Position: " + element.VirtualRegionOnScreen);
				isMoving = false;
				movingTime = 0.0f;
				InvokeIsDone();
				
			}
			
				
			
		}	
	}
	
	public void MorphTo(Rect newRect){
		MorphTo(newRect,MovementDuration);
	}
	
	public void MorphTo(Rect newRect, float duration){
		diffToTargetSize = new Vector2(newRect.width - element.VirtualRegionOnScreen.width , newRect.height - element.VirtualRegionOnScreen.height);
		diffToTargetPosition = new Vector2( newRect.x - element.VirtualRegionOnScreen.x ,newRect.y -  element.VirtualRegionOnScreen.y);
		isMoving = true;
		currentMovementDuration = duration;
		
	}
	
	public void MoveTo(Vector2 newPosition){
		MoveTo(newPosition, MovementDuration);
	}
	public void MoveTo(Vector2 newPosition, float duration){
		diffToTargetSize = new Vector2(0,0);
		diffToTargetPosition = new Vector2(newPosition.x - element.VirtualRegionOnScreen.x, newPosition.y - element.VirtualRegionOnScreen.y);
		isMoving = true;
		currentMovementDuration = duration;
	}
	
	protected virtual void InvokeIsDone(){
		var handler = IsDone;
		if (handler == null) {
			return;
		}
		var e = new MoveableEventArgs(element);
		handler(this, e);
	}
	
}
