using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Frame : MonoBehaviour{
	
	protected float guiDepthFactor = 0.0001f;
	
	public List<Frame> DirectChildren{
		get;
		protected set;
	}
	protected delegate void InteractionEvent(InteractionBehaviour ib);
	protected delegate void ActionEvent(Frame b);
	
	public enum HorizontalFloatPositions {left, right,center, none}
	public enum VerticalFloatPositions {top, bottom,center, none}
	public enum ElementOrientation{horizontal, vertical}
	public enum TextureHandling{StretchTexture,ResizeUVs,ResizeElement}
	
	// Only relevant in automatic position elements like HV Panel - Lower Number positioned Higher
	public int ListIndex = 10;
	
	public Rect VirtualRegionOnScreen; 
	protected Rect originalVirtualRegionOnScreen;
	
	public VerticalFloatPositions verticalFloat = VerticalFloatPositions.none;
	public HorizontalFloatPositions horizontalFloat = HorizontalFloatPositions.none;
	
	public bool FullscreenElement = false;
	public TextureHandling KeepAspectRatio = Frame.TextureHandling.ResizeElement;
	public string help1 = "NOT WORKING LIVE:";
	public int GUIDepth = 5;
	public bool PropagateEvents = true;
	
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
	
	#region init
	
	// DONT USE THIS!
	void Awake() {
		AwakeOverride();
	}

	// Use this for initialization
	protected virtual void AwakeOverride() {
	}
	
	void Start() {
		StartOverride();
	}
	
	protected virtual void StartOverride(){
	}
	
	protected void initDirectChildren() {
		DirectChildren = new List<Frame>();
		foreach (Transform child in transform) {
			var b = child.GetComponent<Frame>();
			if (b != null){
				DirectChildren.Add(b);
			}
				
		}
		DirectChildren.Sort(CompareFramesByIndex);
	}
	
	public virtual void CreateElement(){
		if(created){
			EditorDebug.LogWarning("Element: "+ gameObject.name + "already created");
			return;
		}
		initParent();
		initActiveScreen();
		RealRegionOnScreen = new Rect(0,0,0,0);
		initDirectChildren();
		
		Visibility = true;
		savedVisibility = true;
		
		if(FullscreenElement && KeepAspectRatio == TextureHandling.ResizeElement){
			VirtualRegionOnScreen.width = ScreenConfig.Instance.TargetScreenWidth;//Screen.width;
			VirtualRegionOnScreen.height = ScreenConfig.Instance.TargetScreenHeight;//Screen.height;
		}
		originalVirtualRegionOnScreen = VirtualRegionOnScreen;
		
		
		created = true;
		
	}
	

	protected void initActiveScreen(){
		activeScreen = CameraScreen.GetScreenForObject(this.gameObject);
	}
	
	protected void initParent(){
		if(gameObject.transform.parent == null)
			parent = this;
		else
			parent = gameObject.transform.parent.GetComponent<Frame>() as Frame;
	}

	
	#endregion init
	
	#region Update
	// Update is called once per frame
	void Update() {
		UpdateOverride();
	}

	protected virtual void UpdateOverride() {
		if(!created)
			return;
		
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
	
	}
	
	
	public virtual void UpdateElement(bool updateChildren = true){
		if(!created){
			EditorDebug.LogError("Cannot Update not Created Element: "  + gameObject.name);
			return;
		}
			
		calculateVirtualRegionOnScreen();
		calculateRealRegionOnScreen();
		calculateVisibility();	
		
		// Update all direct Children
		if(updateChildren){
			foreach (var frame in DirectChildren){
				frame.UpdateElement();
			}			
		}
		

		
	
	}
	
	protected void calculateRealRegionOnScreen(){
		// Get RealRegion
		this.RealRegionOnScreen = activeScreen.GetPhysicalRegionFromRect(this.VirtualRegionOnScreen, KeepAspectRatio);
			
		//Check for Flaoting
		var position = GetFloatingPosition();
		this.RealRegionOnScreen = new Rect(position.x, position.y, RealRegionOnScreen.width, RealRegionOnScreen.height);
		
		// Move Parent Offset
		position = new Vector2(parent.RealRegionOnScreen.x + this.RealRegionOnScreen.x, parent.RealRegionOnScreen.y + this.RealRegionOnScreen.y);
		this.RealRegionOnScreen = new Rect(position.x, position.y, RealRegionOnScreen.width, RealRegionOnScreen.height);
	}
	
	protected void calculateVisibility(){
		if(!parent.Visibility && !inheritedVisibility){
			savedVisibility = this.Visibility;
			this.Visibility = false;
			inheritedVisibility = true;
		}
		if(parent.Visibility && inheritedVisibility){
			this.Visibility = savedVisibility;
			inheritedVisibility = false;
		}
			
	}
	
	protected virtual void calculateVirtualRegionOnScreen(){
		
	}
	#endregion


	#region LayoutElement	
	public void ResetTransformation(){
		this.transform.rotation = Quaternion.identity;
		this.transform.localRotation = Quaternion.identity;
		this.transform.localPosition = Vector3.zero;
		this.transform.localScale = Vector3.one;
	}
	public Vector3 WorldToLocalCoordinates(Vector3 worldCoordinates){
		return gameObject.transform.InverseTransformPoint(worldCoordinates);
	}
	#endregion
	
	#region EventHandling 
	
	public virtual void OnClick(object sender, MouseEventArgs e) {
		e.ElementIsActive = InputEvents.Instance.IsActiveElement(this);
		callHandler(ib => { ib.Click(e); }, action => { action.OnClick(sender, e); });	
		InputEvents.Instance.DeregisterActiveElement(this);
			
	}

	public virtual void OnHover(object sender, MouseEventArgs e) {
		e.ElementIsActive = InputEvents.Instance.IsActiveElement(this);
		callHandler(ib => { ib.Hover(e); }, action => { action.OnHover(sender, e); });			
		

	}

	public virtual void OnDown(object sender, MouseEventArgs e) {
		InputEvents.Instance.RegisterActiveElement(this);
		e.ElementIsActive = InputEvents.Instance.IsActiveElement(this);
		callHandler(ib => { ib.Down(e); }, action => { action.OnDown(sender, e); });	
		
		
	}

	public virtual void OnUp(object sender, MouseEventArgs e) {
		e.ElementIsActive = InputEvents.Instance.IsActiveElement(this);
		callHandler(ib => { ib.Up(e); }, action => { action.OnUp(sender, e); });
		
	}

	public virtual void OnMove(object sender, MouseEventArgs e) {
		e.ElementIsActive = InputEvents.Instance.IsActiveElement(this);
		callHandler(ib => { ib.Move(e); }, action => { action.OnMove(sender, e); });
		
		
	}

	public virtual void OnSwipe(object sender, MouseEventArgs e){
		e.ElementIsActive = InputEvents.Instance.IsActiveElement(this);
		callHandler(ib => { ib.Swipe(e); }, action => { action.OnSwipe(sender, e); });
		InputEvents.Instance.DeregisterActiveElement(this);	
	}

	protected virtual void callHandler(InteractionEvent interaction, ActionEvent action) {
		if(!created)
			return;
		
		// Call all InteractionBehaviours for this Object
		var behaviours = gameObject.GetComponents<InteractionBehaviour>() as InteractionBehaviour[];
		if (behaviours != null) {
			foreach (var ib in behaviours) {
				interaction(ib);
			}	
		}
		
		// We dont use this to block eventPropagation because Propagation is based on GUILevel not on
		// Parent Child Hirarchy
		foreach (Frame frame in DirectChildren) {
			if(frame == null)
				continue;
			if (frame.CheckMouseOverElement()){
				// Call OnClick, OnHover etc on all Children
				if (action != null) {
					action(frame);					
				}
			} else {
				frame.ResetElement();
			}
		}
	}
	
	#endregion
	
	public virtual bool CheckMouseOverElement(){
		return true;
	}
	
	
	public void RemoveFloat(){
		var realPosition = new Vector2(this.RealRegionOnScreen.x, this.RealRegionOnScreen.y);
		this.Position = CameraScreen.PhysicalToVirtualScreenPosition(realPosition);
		this.horizontalFloat = Frame.HorizontalFloatPositions.none;
		this.verticalFloat = Frame.VerticalFloatPositions.none;
		
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
	
	public virtual void ResetElement(){
		
	}
	
	
	public static int CompareFramesByIndex(Frame x, Frame y){
		if(x.ListIndex > y.ListIndex){
			return 1;
		} else if(x.ListIndex == y.ListIndex){
			return 0;
		} else {
			return -1;
		}
	}
	
	public static int CompareFramesByGUIDepth(Frame x, Frame y){
		if(x.GUIDepth > y.GUIDepth){
			return 1;
		} else if(x.GUIDepth == y.GUIDepth){
			return 0;
		} else {
			return -1;
		}
	}
}
