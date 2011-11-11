using UnityEngine;
using System.Collections;

public class Panel : Frame {

	public enum HorizontalFloatPositions {left, right, none}
	public enum VerticalFloatPositions {top, bottom, none}
	
	public Rect VirtualRegionOnScreen; 
	public LayoutBehaviour Layout;
	public int layer;
	public VerticalFloatPositions verticalFloat;
	public HorizontalFloatPositions horizontalFloat;
	
	public Rect Uv;
	
	public GUIStyle Style; 
	
	private GUIStyle inactiveStyle;
	
	private GUIPlane plane;

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
#if UNITY_EDITOR
		if(activeScreen.DebugModus)
			plane.VirtualRegionOnScreen = RealRegionOnScreen;
#endif
	}
	
	public virtual void createGUIElement(){
		GameObject go = ResourceManager.CreateInstance<GameObject>("guiPlane");
		if(go == null){
			Debug.LogError("No GameObject found for Plane on Object "+ this.gameObject.name + "! Stop!");
			return;
		}
		
		plane = go.GetComponent<GUIPlane>();
		if(plane == null){
			Debug.LogError("No GUIPlane found on Object "+ this.gameObject.name + "! Stop!");
			return;
		}
		
		Camera cam = activeScreen.ScreenCamera; 
		plane.name = gameObject.name + "_guiPlane";
		plane.transform.parent = cam.transform;
				
		resetPlaneTransform();
		
		plane.transform.position = new Vector3(0,0,(activeScreen.ScreenCamera.nearClipPlane));
		plane.transform.LookAt(cam.transform);
		
		//Debug.Log("Material: " + activeScreen.GUIMaterial.name);
		plane.GUIMaterial = activeScreen.GUIMaterial;
		plane.UV = Uv;
		plane.VirtualRegionOnScreen = RealRegionOnScreen;
		
		
		
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
	
	private void resetPlaneTransform(){
		plane.transform.rotation = Quaternion.identity;
		plane.transform.localRotation = Quaternion.identity;
		plane.transform.position = Vector3.zero;
		plane.transform.localScale = Vector3.one;
	}
	
	
}
