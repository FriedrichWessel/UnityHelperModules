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

	
	protected GUIStyle currentStyle;
	
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
			value.x = Transformation.xMax;
			value.y = Transformation.yMin;
		}
	}
	
	
	// Use this for initialization
	void Start () {
		currentStyle = InactiveStyle;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public virtual void createElement(Rect transformation){
		UnityEngine.GUI.Box(transformation,"", currentStyle);	
	}
	
	public bool checkMouseOverElement(){
		return CameraScreen.cursorInside(position, size);
	}
	
	
	
	protected void resetElement(){
		currentStyle = InactiveStyle;
	}
	
	
}
