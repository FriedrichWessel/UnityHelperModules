using UnityEngine;
using System.Collections;

public class Box : Frame {

	public enum positionFloat {left, right, top, bottom}
	
	public Rect Transformation; 
	public LayoutBehaviour Layout;
	public int layer;
	public positionFloat verticalFloat;
	public positionFloat horizontalFloat;
	
	public GUIStyle InactiveStyle; 

	public Rect RealRegionOnScreen{
		get;
		set;
	}	
	protected GUIStyle currentStyle;

	protected CameraScreen activeScreen;
	

	// PROPERTYS
	public Vector2 position{
		get{
			return new Vector2(Transformation.x, Transformation.y);
		}
		set{
			value.x = Transformation.x;
			value.y = Transformation.y;
		}
		
	}
	
	public Vector2 size{
		get{
			return new Vector2(Transformation.xMax, Transformation.yMin);
		}
		set{
			value.x = Transformation.width;
			value.y = Transformation.height;
		}
	}
	
	
	
	// DONT USE THIS
	void Awake(){
		
		AwakeOverride();
		
	}
	
	// Use this for initialization
	protected override void AwakeOverride(){
		base.AwakeOverride();
		RealRegionOnScreen = new Rect(0,0,0,0);
		currentStyle = InactiveStyle;
		activeScreen = CameraScreen.GetScreenForObject(this.gameObject);
	}
	
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public virtual void createGUIElement(){
		//Debug.Log("CurrenStyle: " + currentStyle.name);
		//Debug.Log("RealRegion: " + RealRegionOnScreen);
		UnityEngine.GUI.Box(RealRegionOnScreen,"", currentStyle);	
	}
	
	public virtual  bool checkMouseOverElement(){
		return CameraScreen.cursorInside(RealRegionOnScreen);
	}
	
	
	
	protected void resetElement(){
		currentStyle = InactiveStyle;
	}
	
	
}
