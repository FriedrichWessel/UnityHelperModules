using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScreen : Frame {

	public Dictionary<Box,Rect> childTransformation; 
	
	// Public Member - init in the inspector
	public Camera ScreenCamera;
	
	// Public Propertys
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
	
	void Awake(){
		initChildTransformations();
		
	}
	void Start(){
		
		CalculateAbsolutePositions();
		initEvents();
		
		
	}
	
	private void initChildTransformations(){
		childTransformation = new Dictionary<Box, Rect>();
		foreach(Box b in allChildren){
			childTransformation.Add(b, new Rect(0,0,0,0));
		}
	}
	private void initEvents(){
		InputEvents.Instance.ClickEvent += OnClick;	
	}
	
	void OnGUI(){
		LayoutElement();
	}
	
	public override void LayoutElement(){
		base.LayoutElement();
		createElements();
		
	}
	
	
	public void CalculateAbsolutePositions(){
		base.LayoutElement();
		foreach(KeyValuePair<Box, Rect> pair in childTransformation){
			childTransformation[pair.Key] = getRelativePosition(pair.Key.Transformation);
		}
		
			
		
	}
	
	private static Vector2 getFactor(){
		// Get the right Hight and Width proportional to screen
		float factorY = (float)(Screen.height) / (float)(ScreenConfig.TargetScreenHeight); 
		float factorX = (float)(Screen.width) / (float)(ScreenConfig.TargetScreenWidth);
		return new Vector2(factorX, factorY);
	}
	
	private Rect getRelativePosition(Rect rect){
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
		foreach (KeyValuePair<Box, Rect> pair in childTransformation)
			pair.Key.createGUIElement(pair.Value);
	}
	

	
	// STATIC FUNCTIONS
	public static bool cursorInside(Vector2 elementPosition, Vector2 elementSize) {
		bool flagX = false;
		bool flagY = false;
		
		if (mousePosition.x >= elementPosition.x && (mousePosition.x <= (elementPosition.x + elementSize.x)))
			flagX = true;
		if (mousePosition.y >= elementPosition.y && (mousePosition.y <= (elementPosition.y + elementSize.y)))
			flagY = true;
		
		//Debug.Log(mousePos.x + " " + mousePos.y + " " +elemPos.x + " " +elemPos.y +" " + elemSize.x + " " +elemSize.y + " " + (flagX && flagY && flagZ));
		
		return (flagX && flagY);
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
