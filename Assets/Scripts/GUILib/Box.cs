using UnityEngine;
using System.Collections;

public class Box : Frame {

	public enum HorizontalFloatPositions {left, right, none}
	public enum VerticalFloatPositions {top, bottom, none}
	
	public Rect VirtualRegionOnScreen; 
	public LayoutBehaviour Layout;
	public int layer;
	public VerticalFloatPositions verticalFloat;
	public HorizontalFloatPositions horizontalFloat;
	
	public GUIStyle Style; 
	
	private GUIStyle inactiveStyle;

	public Rect RealRegionOnScreen{
		get;
		set;
	}	
	protected GUIStyle currentStyle;

	protected CameraScreen activeScreen;
	

	// PROPERTYS
	public Vector2 position{
		get{
			return new Vector2(VirtualRegionOnScreen.x, VirtualRegionOnScreen.y);
		}
		set{
			VirtualRegionOnScreen.x = value.x; 
			VirtualRegionOnScreen.y = value.y;
		}
		
	}
	
	public Vector2 size{
		get{
			return new Vector2(VirtualRegionOnScreen.xMax, VirtualRegionOnScreen.yMin);
		}
		set{
			VirtualRegionOnScreen.width = value.x; 
			VirtualRegionOnScreen.height = value.y; 
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
		initStyle();
		resetElement();
		activeScreen = CameraScreen.GetScreenForObject(this.gameObject);
	}
	
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public virtual void createGUIElement(){
		UnityEngine.GUI.Box(RealRegionOnScreen,"", currentStyle);	
	}
	
	public virtual  bool checkMouseOverElement(){
		return CameraScreen.cursorInside(RealRegionOnScreen);
	}
	
	
	
	public void resetElement(){
		currentStyle = inactiveStyle;
		
	}
	
	protected virtual void initStyle(){
		inactiveStyle = new GUIStyle();
		inactiveStyle.normal = Style.normal;
	}
	
	
}
