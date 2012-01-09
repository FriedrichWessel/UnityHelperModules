using UnityEngine;
using System;
using System.Collections;
using asdf.Resources;

public class Panel : Frame {

	
	public LayoutBehaviour Layout;
	public string help1 = "NOT WORKING LIVE:";
	public int GUIDepth = 5;
	
	//public bool FullscreenElement = false;
	
	public override bool Visibility{
		get{
			bool flag = currentVisibility;
			if(plane != null){
				flag = plane.renderer.enabled;
				currentVisibility = flag;
			}
				
			return flag;
		}
		set{
			
			if(plane != null){
				plane.renderer.enabled = value;
				EditorDebug.Log("Change vis for: " + gameObject.name + " to " + value);
			}
				
		}
	}
	
	
	
	public Rect Uv;
	
	private GUIStyle inactiveStyle;
	
	protected GUIPlane plane;

	
	protected GUIStyle currentStyle;

	
	

	
	
	
	
	// DONT USE THIS
	void Awake(){
		AwakeOverride();
	}
	
	void OnDestroy(){
		OnDestroyOverride();
	}
	
	protected virtual void OnDestroyOverride(){
		
		if(plane != null){
			GameObject.Destroy(plane.gameObject);
		}
	}
	
	// Use this for initialization
	protected override void AwakeOverride(){
		base.AwakeOverride();
		this.Visibility = true;
	}
	

	void Start () {
		StartOverride();
		
	}
	
	protected override void StartOverride(){
		base.StartOverride();
		UpdateRegionOnScreen();
	}
	
	void OnGUI(){
		OnGUIOverride();
	}
	
	protected virtual void OnGUIOverride(){
		
	}
	
	// Update is called once per frame
#if UNITY_EDITOR
	void Update () {
		UpdateOverride();
	}
	
	protected override void UpdateOverride(){
		base.UpdateOverride();
		if(created && activeScreen.DebugModus ){
			UpdateElement();
		}
	}
#endif		

	
	public override void CreateElement(){
		base.CreateElement();
	

		
		this.createGUIElement();
		created = true;
		UpdateElement();
	}
	
	

	public override void UpdateRegionOnScreen(){
		base.UpdateRegionOnScreen();
		if(plane != null)
			plane.VirtualRegionOnScreen = RealRegionOnScreen;
		
		//resetElement();
	}
	
	public virtual void createGUIElement(){
		
		CreateGUIPlane();
				
		Camera cam = activeScreen.ScreenCamera; 
		plane.name = gameObject.name + "_guiPlane";
		plane.transform.parent = cam.transform;
		
		// Orient Plane to Camera
		resetPlaneTransform();
		float layer = (float)GUIDepth * 0.0001f;
		plane.transform.Translate(new Vector3(0,0,(activeScreen.ScreenCamera.nearClipPlane+layer)), Space.Self);
		plane.transform.LookAt(cam.transform);
		
		// set Materials
		plane.GUIMaterial = activeScreen.GUIMaterial;
		plane.UV = Uv;
		plane.VirtualRegionOnScreen = RealRegionOnScreen;
		
			
	}
	
	public void SetRotationTransformations(Vector2 localCenter, float degrees){
		plane.RotationAngle = degrees;
		plane.RotationCenter = localCenter;
	}
	
	private Vector3 WorldToLocalCoordinates(Vector3 worldCoordinates){
		return gameObject.transform.InverseTransformPoint(worldCoordinates);
	}
	
	private void CreateGUIPlane(){
		GameObject go = ResourceManager.CreateInstance<GameObject>("guiPlane");
		if(go == null){
			EditorDebug.LogError("No GameObject found for Plane on Object "+ this.gameObject.name + "! Stop!");
			return;
		}
		
		plane = go.GetComponent<GUIPlane>();
		if(plane == null){
			EditorDebug.LogError("No GUIPlane found on Object "+ this.gameObject.name + "! Stop!");
			return;
		}
		
		
	}
	
	public override  bool checkMouseOverElement(){
		return CameraScreen.CursorInsidePhysicalRegion(RealRegionOnScreen);
	}
	
	
	
	public override void resetElement(){
		if(plane != null)
			plane.UV = Uv;
		
	}
	
	private void resetPlaneTransform(){
		plane.transform.rotation = Quaternion.identity;
		plane.transform.localRotation = Quaternion.identity;
		plane.transform.localPosition = Vector3.zero;
		plane.transform.localScale = Vector3.one;
	}
	
	
}
