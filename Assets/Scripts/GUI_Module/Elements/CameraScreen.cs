using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScreen : Frame {

	// If EditorDebugModus is checked, ScreenPosition is update every OnGUI call this is usefull for positioning elements
	// but not good for the framerate
	public bool DebugModus;
	public int TextureSize = 512;
	public Material GUIMaterial;
	
	// Public Member - init in the inspector
	public Camera ScreenCamera;
	
	
	//  Propertys
	public static Vector2 mousePosition{
		get{
			return PhysicalToVirtualScreenPosition(Input.mousePosition); 
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
		StartOverride();
	}
	
	protected override void StartOverride(){
		base.StartOverride();
		CreateElement();
		
		initEvents();
	}
	
	private void initEvents(){
		InputEvents.Instance.ClickEvent += OnClick;	
		InputEvents.Instance.MoveEvent += OnMove;
		InputEvents.Instance.MoveEvent += OnHover;
		InputEvents.Instance.DownEvent += OnDown;
		InputEvents.Instance.UpEvent += OnUp;
		InputEvents.Instance.SwipeEvent += OnSwipe;
	}
	
	
#if UNITY_EDITOR
	void Update(){
		UpdateOverride();
	}
	
	protected override void UpdateOverride(){
		base.UpdateOverride();

		if(DebugModus)
			UpdateElement();

	}
#endif	
	
	public override void UpdateElement(){
		this.RealRegionOnScreen = GetPhysicalRegionFromRect(VirtualRegionOnScreen,KeepAspectRatio);
	}
	private static Vector2 getFactor(bool withAspect = true){
		// Get the right Hight and Width proportional to screen
		float rightAspectHeight = Screen.height;
		float rightAspectWidth = Screen.width;
		float aspectRatio = (float)(Screen.width) / Screen.height;
		if(withAspect && aspectRatio < ScreenConfig.Instance.ScreenAspect){
			rightAspectHeight = (float)(Screen.width) / ScreenConfig.Instance.ScreenAspect;	
		} else if(withAspect && aspectRatio > ScreenConfig.Instance.ScreenAspect){
			rightAspectWidth = (float)(Screen.height) * ScreenConfig.Instance.ScreenAspect;
		}
		
		float factorY = rightAspectHeight / (float)(ScreenConfig.Instance.TargetScreenHeight);
		float factorX = rightAspectWidth / (float)(ScreenConfig.Instance.TargetScreenWidth);
		return new Vector2(factorX, factorY);
	}
	
	
	public Rect GetPhysicalRegionFromRect(Rect rect, bool withAspect = true){
		Rect camPosition = ScreenCamera.pixelRect;
		// Move Camera is needed for Splitscreen
		if(((int)ScreenCamera.pixelHeight) != Screen.height){
			//EditorDebug.Log("ScreenCamera Height: " + ScreenCamera.pixelHeight +  "\n Screen Height: " + Screen.height);
			camPosition.y = ScreenCamera.pixelHeight - camPosition.y;
		}
		
		Vector2 factor = getFactor(withAspect);
		Vector2 newPosition = new Vector2((camPosition.x+rect.x)*factor.x, (camPosition.y +  rect.y)*factor.y);
		Vector2 newSize = new Vector2(rect.width*factor.x,rect.height*factor.y);
		
		return new Rect (  newPosition.x, newPosition.y, newSize.x, newSize.y );
	} 
	
	
	
	
	
	
	public static int GetPhysicalTextSize(int size) {
		Vector2 factor = getFactor();
		return (int)(size * factor.y);
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
	
	public static bool CursorInsidePhysicalRegion(Vector2 elementPosition, Vector2 elementSize){
		bool flagX = false;
		bool flagY = false;
		
		Vector2 mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		if (mouse.x >= elementPosition.x && (mouse.x <= (elementPosition.x + elementSize.x)))
			flagX = true;
		if (mouse.y >= elementPosition.y && (mouse.y <= (elementPosition.y + elementSize.y)))
			flagY = true;
		return (flagX && flagY);
	}
	
	public static bool CursorInsidePhysicalRegion(Rect physicalRegion){
		return CursorInsidePhysicalRegion(new Vector2(physicalRegion.x, physicalRegion.y), new Vector2(physicalRegion.width, physicalRegion.height));
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
				
			if(obj.transform.parent != null)
				savedParent = obj.transform.parent.gameObject;
			else 
				savedParent = null;
			obj = savedParent;
		}
		if(screen == null){
			EditorDebug.LogWarning("Element: " + savedObj.gameObject.name + " is not a child of a Screen!");
		}
		return screen;
		
		
	}
	
	public static Vector2 PhysicalToVirtualScreenPosition(Vector2 screenPosition){
		//float factorY = (float)(Screen.height) / (float)(ScreenConfig.Instance.TargetScreenHeight); 
		var factor = getFactor();
		//float factorX = (float)(Screen.width) / (float)(ScreenConfig.Instance.TargetScreenWidth);
		//float factorY = factorX / ScreenConfig.Instance.ScreenAspect;
		//screenPosition.y = Screen.height - screenPosition.y;
		screenPosition.x /= factor.x;
		screenPosition.y /= factor.y;
		return screenPosition;
	}
	
	
	

}
