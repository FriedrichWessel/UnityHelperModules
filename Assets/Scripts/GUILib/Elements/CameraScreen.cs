using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScreen : Frame {

	// If DebugModus is checked, ScreenPosition is update every OnGUI call this is usefull for positioning elements
	// but not good for the framerate
	public bool DebugModus;
	
	public Material GUIMaterial;
	
	// Public Member - init in the inspector
	public Camera ScreenCamera;
	
	
	//  Propertys
	public static Vector2 mousePosition{
		get{
			return NormalizeScreenPosition(Input.mousePosition); 
		}
	}
	
	private Panel[] allChildren{
		get{
			return (gameObject.GetComponentsInChildren<Panel>() as Panel[]);
		}
	}
	
	// DONT USE THIS
	void Awake(){
		AwakeOverride();
	}
	
	protected override void AwakeOverride(){
		base.AwakeOverride();
	}
	
	void Start(){
		CalculatePhysicalRegion();
		initEvents();
		LayoutElement();
		
		
	}
	
	
	private void initEvents(){
		InputEvents.Instance.ClickEvent += OnClick;	
		InputEvents.Instance.MoveEvent += OnMove;
		InputEvents.Instance.MoveEvent += OnHover;
		InputEvents.Instance.DownEvent += OnDown;
		InputEvents.Instance.UpEvent += OnUp;
	}
	
	void OnGUI(){
		//LayoutElement();
	}
	
	void Update(){
		if(DebugModus)
			CalculatePhysicalRegion();
	}
	public override void LayoutElement(){
		base.LayoutElement();
		CalculatePhysicalRegion();
		createElements();
		
	}
	
	
	public void CalculatePhysicalRegion(){
		base.LayoutElement();
		foreach(Panel box in allChildren){
			box.RealRegionOnScreen = GetPhysicalRegionFromRect(box.VirtualRegionOnScreen);
		}
		
	}
	
	private static Vector2 getFactor(){
		// Get the right Hight and Width proportional to screen
		float factorY = (float)(Screen.height) / (float)(ScreenConfig.TargetScreenHeight); 
		float factorX = (float)(Screen.width) / (float)(ScreenConfig.TargetScreenWidth);
		return new Vector2(factorX, factorY);
	}
	
	public Rect GetPhysicalRegionFromRect(Rect rect){
		Rect camPosition = ScreenCamera.pixelRect;
		// Move Camera is needed for Splitscreen
		if(((int)ScreenCamera.pixelHeight) != Screen.height){
			//Debug.Log("ScreenCamera Height: " + ScreenCamera.pixelHeight +  "\n Screen Height: " + Screen.height);
			camPosition.y = ScreenCamera.pixelHeight - camPosition.y;
		}
		
		Vector2 factor = getFactor();
		Vector2 newPosition = new Vector2((camPosition.x+rect.x)*factor.x, (camPosition.y +  rect.y)*factor.y);
		Vector2 newSize = new Vector2(rect.width*factor.x,rect.height*factor.y);
		
		return new Rect (  newPosition.x, newPosition.y, newSize.x, newSize.y );
	} 
	
	protected override void callHandler(InteractionEvent interaction, ActionEvent action){
		InteractionBehaviour[] behaviours = gameObject.GetComponents<InteractionBehaviour>() as InteractionBehaviour[];
		foreach(InteractionBehaviour ib in behaviours){
			interaction(ib);
		}
		base.callHandler(interaction, action);
		
		
			
		
	}
	
	private void createElements(){
		foreach(Panel box in allChildren){
			box.createGUIElement();
		}
	}
	

	
	// STATIC FUNCTIONS
	public static bool cursorInside(Vector2 elementPosition, Vector2 elementSize) {
		bool flagX = false;
		bool flagY = false;
		
		if (mousePosition.x >= elementPosition.x && (mousePosition.x <= (elementPosition.x + elementSize.x)))
			flagX = true;
		if (mousePosition.y >= elementPosition.y && (mousePosition.y <= (elementPosition.y + elementSize.y)))
			flagY = true;
		return (flagX && flagY);
	}
	
	
	public static bool cursorInside(Rect region){
		return cursorInside(new Vector2(region.x, region.y), new Vector2(region.width, region.height));
	}
	
	public static CameraScreen GetScreenForObject(GameObject obj){
		GameObject savedObj = obj;
		GameObject savedParent = null;
		CameraScreen screen = null;
		while(obj != null){
			screen = obj.GetComponent<CameraScreen>();
			if( screen != null){
				break;
			}
				
			savedParent = obj.transform.parent.gameObject;
			obj = savedParent;
		}
		if(screen == null){
			Debug.LogWarning("Element: " + savedObj.gameObject.name + " is not a child of a Screen!");
		}
		return screen;
		
		
	}
	
	public static Vector3 NormalizeScreenPosition(Vector2 screenPosition){
		float factorY = (float)(Screen.height) / (float)(ScreenConfig.TargetScreenHeight); 
		float factorX = (float)(Screen.width) / (float)(ScreenConfig.TargetScreenWidth);
		screenPosition.y = Screen.height - screenPosition.y;
		screenPosition.x /= factorX;
		screenPosition.y /= factorY;
		return screenPosition;
	}
	
	
	

}
