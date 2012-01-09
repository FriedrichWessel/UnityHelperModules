using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class Frame : MonoBehaviour
{
	public List<Frame> DirectChildren{
		get;
		protected set;
	}
	protected delegate void InteractionEvent(InteractionBehaviour ib);
	protected delegate void ActionEvent(Frame b);
	
	public enum HorizontalFloatPositions {left, right,center, none}
	public enum VerticalFloatPositions {top, bottom,center, none}
	public enum ElementOrientation{horizontal, vertical}
	
	public Rect VirtualRegionOnScreen; 
	protected Rect originalVirtualRegionOnScreen;
	
	public VerticalFloatPositions verticalFloat = Panel.VerticalFloatPositions.none;
	public HorizontalFloatPositions horizontalFloat = Panel.HorizontalFloatPositions.none;
	
	public bool FullscreenElement = false;
	public bool KeepAspectRatio = true;
	
	protected bool created = false;
	protected bool firstUpdateFlag = true;
	protected bool currentVisibility = true;
	protected bool savedVisibility;
	
	protected bool inheritedVisibility = false;
	
	
	public virtual bool Visibility{
		get{
			return currentVisibility;
		}
		set{
			currentVisibility = value;
		}
	}
	
	public CameraScreen activeScreen{
		get;
		set;
	}
	
	public Rect RealRegionOnScreen{
		get;
		set;
	}
	
	// PROPERTYS
	public Vector2 Position{
		get{
			return new Vector2(VirtualRegionOnScreen.x, VirtualRegionOnScreen.y);
		}
		set{
			VirtualRegionOnScreen.x = value.x; 
			VirtualRegionOnScreen.y = value.y;
		}
	}

	

	public Vector2 Size{
		get{
			return new Vector2(VirtualRegionOnScreen.xMax, VirtualRegionOnScreen.yMin);
		}
		set{
			VirtualRegionOnScreen.width = value.x; 
			VirtualRegionOnScreen.height = value.y; 
		}
	}
	
	protected Frame parent;

	// DONT USE THIS!
	void Awake() {
		AwakeOverride();
	}

	// Use this for initialization
	protected virtual void AwakeOverride() {
		
		Visibility = true;
		savedVisibility = true;
		UpdateParent();
		if(FullscreenElement){
			VirtualRegionOnScreen.width = ScreenConfig.Instance.TargetScreenWidth;//Screen.width;
			VirtualRegionOnScreen.height = ScreenConfig.Instance.TargetScreenHeight;//Screen.height;
		}
		UpdateActiveScreen();
		originalVirtualRegionOnScreen = VirtualRegionOnScreen;
		
		initDirectChildren();
		
	}
	
	public void UpdateActiveScreen(){
		activeScreen = CameraScreen.GetScreenForObject(this.gameObject);
	}
	public void UpdateParent(){
		if(gameObject.transform.parent == null)
			parent = this;
		else
			parent = gameObject.transform.parent.GetComponent<Frame>() as Frame;
	}

	void Start() {
		StartOverride();
	}
	
	protected virtual void StartOverride(){
		Visibility = true;
	}
	
	// Update is called once per frame
	void Update() {
		UpdateOverride();
	}

	protected virtual void UpdateOverride() {
		if(firstUpdateFlag){
			firstUpdateFlag = false;
			firstUpdate();
		}
#if UNITY_EDITOR
		if(activeScreen.DebugModus)
			UpdateElement();
#endif	
		
	}
	
	
	protected virtual void firstUpdate(){
		// nothing here
	}

		
	/**
	 * This Function is called by Parent to force the child to arrange them selves 
	 **/
	public virtual void LayoutElement() {
		
		
		//do positioning etc. for this class here
	}

	public virtual void OnClick(object sender, MouseEventArgs e) {
		
		callHandler(ib => { ib.Click(e); }, action => { action.OnClick(sender, e); });
	}

	public virtual void OnHover(object sender, MouseEventArgs e) {
		callHandler(ib => { ib.Hover(e); }, action => { action.OnHover(sender, e); });
	}

	public virtual void OnDown(object sender, MouseEventArgs e) {
		callHandler(ib => { ib.Down(e); }, action => { action.OnDown(sender, e); });
	}

	public virtual void OnUp(object sender, MouseEventArgs e) {
		callHandler(ib => { ib.Up(e); }, action => { action.OnUp(sender, e); });
	}

	public virtual void OnMove(object sender, MouseEventArgs e) {
		callHandler(ib => { ib.Move(e); }, action => { action.OnMove(sender, e); });
	}

	public virtual void OnSwipe(object sender, MouseEventArgs e) {
		callHandler(ib => { ib.Swipe(e); }, action => { action.OnSwipe(sender, e); });
	}

	protected virtual void callHandler(InteractionEvent interaction, ActionEvent action) {
		foreach (Frame b in DirectChildren) {
			if(b == null)
				continue;
			if (b.checkMouseOverElement()) {
				if (action != null) {
					action(b);					
				}
				var behaviours = b.GetComponents<InteractionBehaviour>() as InteractionBehaviour[];
				if (behaviours != null) {
					foreach (var ib in behaviours) {
						interaction(ib);
					}	
				}
			} else {
				b.resetElement();
			}
		}
	}
	
	public virtual bool checkMouseOverElement(){
		return true;
	}
	
	public void UpdateDirectChildren() {
		initDirectChildren();
	}
	
	public virtual void UpdateElement(){
		
		//base.UpdateElement();
		
		//EditorDebug.LogWarning("Update Element: " + gameObject.name);
		UpdateDirectChildren();
		UpdateParent();
		//activeScreen = CameraScreen.GetScreenForObject(this.gameObject);
		
		// Get RealRegion
		if(activeScreen != null)
			this.RealRegionOnScreen = activeScreen.GetPhysicalRegionFromRect(this.VirtualRegionOnScreen, KeepAspectRatio);
		
		
		
			
		//Check for Flaoting
		//this.RealRegionOnScreen = this.RealRegionOnScreen.AddPosition(GetFloatingPosition());
		var position = GetFloatingPosition();
		this.RealRegionOnScreen = new Rect(position.x, position.y, RealRegionOnScreen.width, RealRegionOnScreen.height);
		
		
		// Move Parent Offset
	
		position = new Vector2(parent.RealRegionOnScreen.x + this.RealRegionOnScreen.x, parent.RealRegionOnScreen.y + this.RealRegionOnScreen.y);
		this.RealRegionOnScreen = new Rect(position.x, position.y, RealRegionOnScreen.width, RealRegionOnScreen.height);
		
		
		UpdateRegionOnScreen();
		
		
	
		updateVisibiltyAfterParent();	
		
		foreach (var frame in DirectChildren){
			frame.UpdateElement();
		}	
		
		/*UpdateDirectChildren();
		foreach (Panel panel in directChildren){
			panel.UpdateElement();
		}*/	
	}
	
	protected void updateVisibiltyAfterParent(){
		if(!parent.Visibility){
			savedVisibility = this.Visibility;
			this.Visibility = false;
			inheritedVisibility = true;
			EditorDebug.Log("Save VIs: " + savedVisibility + " " + gameObject.name);
		}
		if(parent.Visibility && inheritedVisibility){
			EditorDebug.Log("Load VIs: " + savedVisibility + " " + gameObject.name);
			this.Visibility = savedVisibility;
			
		}
			
	}
	public void removeFloat(){
		var realPosition = new Vector2(this.RealRegionOnScreen.x, this.RealRegionOnScreen.y);
		this.Position = CameraScreen.PhysicalToVirtualScreenPosition(realPosition);
		this.horizontalFloat = Frame.HorizontalFloatPositions.none;
		this.verticalFloat = Frame.VerticalFloatPositions.none;
		
	}
	
	public virtual void UpdateRegionOnScreen(){
		
	}
	
	public Vector2 GetFloatingPosition(){
		var ret = new Vector2(0,0);
		ret.y = getVerticalFloatPosition();
		ret.x = getHorizontalFloatPosition();
		
		return ret;
	}

	

	private float getVerticalFloatPosition(){
		float ret = RealRegionOnScreen.y ;
		switch(verticalFloat){
			case VerticalFloatPositions.none:
			break;
			case VerticalFloatPositions.top:
				ret =  0.0f;
			break;
			case VerticalFloatPositions.bottom:
				ret =  (parent.RealRegionOnScreen.height - this.RealRegionOnScreen.height);
			break;
			case VerticalFloatPositions.center:
				ret =  (parent.RealRegionOnScreen.height/2 - this.RealRegionOnScreen.height/2);
			break;
			default:
				EditorDebug.LogError("Unknown VerticalPosition: " + verticalFloat);
			break;
		}
		return ret;
	}
	
	private float getHorizontalFloatPosition(){
		float ret = RealRegionOnScreen.x;
		switch(horizontalFloat){
			case HorizontalFloatPositions.none:
			break;
			case HorizontalFloatPositions.left:
				ret = 0.0f;
			break;
			case HorizontalFloatPositions.right:
				ret = (parent.RealRegionOnScreen.width - this.RealRegionOnScreen.width);
			break;
			case HorizontalFloatPositions.center:
				ret = (parent.RealRegionOnScreen.width/2 - this.RealRegionOnScreen.width/2);
			break;
			default:
				EditorDebug.LogError("Unknown HorizontalPosition: " + horizontalFloat);
			break;
			
		}
		return ret;
	}
	public virtual void CreateElement(){
		if(created){
			EditorDebug.Log("Element: "+ gameObject.name + "already created");
			return;
		}
		
		if(activeScreen == null)
			activeScreen = CameraScreen.GetScreenForObject(this.gameObject);
		RealRegionOnScreen = new Rect(0,0,0,0);
		UpdateDirectChildren();
		UpdateElement();
		foreach (var frame in DirectChildren){
			frame.CreateElement();
		}
		created = true;
		
			
		
	}
	
	public virtual void resetElement(){
		
	}
	
	private void initDirectChildren() {
		DirectChildren = new List<Frame>();
		foreach (Transform child in transform) {
			var b = child.GetComponent<Frame>();
			if (b != null){
				DirectChildren.Add(b);
			}
				
		}
	}
}
