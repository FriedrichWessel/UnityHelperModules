using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScreen : Frame {

	// If DebugModus is in ScreenPosition is update every OnGUI Call usefull for positioning elements
	// but not good for the framerate
	public bool DebugModus;

	// Public Member - init in the inspector
	public Camera ScreenCamera;
	
	//  Propertys
	public static Vector2 mousePosition{
		get{
			return NormalizeMousePosition(Input.mousePosition); 
		}
	}
	
	private Box[] allChildren{
		get{
			return (gameObject.GetComponentsInChildren<Box>() as Box[]);
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
		CalculateAbsolutePositions();
		initEvents();
		
		
	}
	
	
	private void initEvents(){
		InputEvents.Instance.ClickEvent += OnClick;	
		InputEvents.Instance.HoverEvent += OnHover;
		InputEvents.Instance.DownEvent += OnDown;
		InputEvents.Instance.UpEvent += OnUp;
	}
	
	void OnGUI(){
		
		LayoutElement();
		
	}
	
	public override void LayoutElement(){
		base.LayoutElement();
		if(DebugModus)
			CalculateAbsolutePositions();
		createElements();
		
	}
	
	
	public void CalculateAbsolutePositions(){
		base.LayoutElement();
		foreach(Box box in allChildren){
			box.RealRegionOnScreen = GetRelativePosition(box.Transformation);
		}
		
	}
	
	private static Vector2 getFactor(){
		// Get the right Hight and Width proportional to screen
		float factorY = (float)(Screen.height) / (float)(ScreenConfig.TargetScreenHeight); 
		float factorX = (float)(Screen.width) / (float)(ScreenConfig.TargetScreenWidth);
		return new Vector2(factorX, factorY);
	}
	
	public Rect GetRelativePosition(Rect rect){
		Rect camPosition = ScreenCamera.pixelRect;
		// Inverse Screenposition on y because GUI (0,0) is on top camera (0,0) is on Bottom 
		if(ScreenCamera.pixelHeight != Screen.height)
			camPosition.y = ScreenCamera.pixelHeight - camPosition.y;
		
		Vector2 factor = getFactor();
		Vector2 newPosition = new Vector2((camPosition.x+rect.x)*factor.x, (camPosition.y +  rect.y)*factor.y);
		Vector2 newSize = new Vector2(rect.width*factor.x,rect.height*factor.y);
		
		return new Rect (  newPosition.x, newPosition.y, newSize.x, newSize.y );
	} 
	
	// PRIVATE METHODS
	private void createElements(){
		foreach(Box box in allChildren){
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
		/*Debug.Log("Cursor Inside Check! \n" +
					"Mouse Position: " + mousePosition.x + " " + mousePosition.y + "\n "
		          +"Element Position: " +elementPosition.x + " " +elementPosition.y +"\n "
		          +"ElementSize: " + elementSize.x + " " +elementSize.y + "\n" +
		          "Flags: " + flagX + " " + flagY + " " + (flagX && flagY));*/
		
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
	private static Vector3 NormalizeMousePosition(Vector2 mousePosition){
		float factorY = (float)(Screen.height) / (float)(ScreenConfig.TargetScreenHeight); 
		float factorX = (float)(Screen.width) / (float)(ScreenConfig.TargetScreenWidth);
		mousePosition.y = Screen.height - mousePosition.y;
		mousePosition.x /= factorX;
		mousePosition.y /= factorY;
		return mousePosition;
	}
	

}
