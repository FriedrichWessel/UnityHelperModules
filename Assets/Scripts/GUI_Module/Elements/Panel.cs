using UnityEngine;
using System;
using System.Collections;
using asdf.Resources;

public class Panel : Frame {

	
	public LayoutBehaviour Layout;
	
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
			}	
			currentVisibility = value;
		}
	}
	
	public Rect FieldOfIntrest{
		get;
		protected set;
	}
	
	public Rect Uv;
	
	private GUIStyle inactiveStyle;
	
	public GUIPlane plane{
		get;
		protected set;
	}

	
	//protected GUIStyle currentStyle;

	
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
		if(FullscreenElement){
			ResizeToFullScreen();
		}	
	}
	
	public static void CalculateFullScreenSize(ref Rect regionOnScreen, ref Rect uvs,Frame.TextureHandling textureHandling, out Rect fieldOfIntrest){
		fieldOfIntrest = new Rect(0,0,-1,-1);
		
		var aspectImage = regionOnScreen.width / regionOnScreen.height;
		var aspectScreen = ScreenConfig.Instance.ScreenAspect;
		if(textureHandling == Frame.TextureHandling.StretchTexture){
			regionOnScreen.width = ScreenConfig.Instance.TargetScreenWidth;//Screen.width;
			regionOnScreen.height = ScreenConfig.Instance.TargetScreenHeight;//Screen.height;
			fieldOfIntrest = regionOnScreen;
		} else if(textureHandling == Frame.TextureHandling.ResizeElement){
			
			if(aspectImage < aspectScreen){
				regionOnScreen.height = ScreenConfig.Instance.TargetScreenHeight;
				regionOnScreen.width = ScreenConfig.Instance.TargetScreenHeight * aspectImage;
			} else {
				regionOnScreen.width = ScreenConfig.Instance.TargetScreenWidth;
				regionOnScreen.height = ScreenConfig.Instance.TargetScreenWidth * aspectImage;
			}
			fieldOfIntrest = regionOnScreen;
		} else if(textureHandling == Frame.TextureHandling.ResizeUVs){
			
			if(aspectImage == aspectScreen)
				return;
			
			if(aspectImage < aspectScreen){
				
				var oldValue = uvs.width;
				uvs.width = uvs.height * aspectScreen;
				var diff = uvs.width - oldValue;
				uvs.x -= (int)(diff/2);
				
				
				fieldOfIntrest.height = ScreenConfig.Instance.TargetScreenHeight;
				fieldOfIntrest.width = ScreenConfig.Instance.TargetScreenHeight * aspectImage;
				fieldOfIntrest.x = (ScreenConfig.Instance.TargetScreenWidth - fieldOfIntrest.width) / 2;
				fieldOfIntrest.y = 0;
				
				
			} else{
				var oldValue = uvs.height;
				uvs.height = uvs.width / aspectScreen;
				var diff = uvs.height - oldValue;
				uvs.y -= (int)(diff/2);
				
				fieldOfIntrest.height = ScreenConfig.Instance.TargetScreenWidth / aspectImage;
				fieldOfIntrest.width = ScreenConfig.Instance.TargetScreenWidth;
				fieldOfIntrest.x = 0;
				fieldOfIntrest.y = (ScreenConfig.Instance.TargetScreenHeight - fieldOfIntrest.height) / 2;
		
			}
			

			
			regionOnScreen.height = ScreenConfig.Instance.TargetScreenHeight;
			regionOnScreen.width = ScreenConfig.Instance.TargetScreenWidth;
		}
		
	}
		
	protected void ResizeToFullScreen(){
		Rect tmpRegionOnScreen = VirtualRegionOnScreen;
		Rect tmpUV = Uv;
		Rect tmpFOI = FieldOfIntrest;
		Panel.CalculateFullScreenSize(ref tmpRegionOnScreen, ref tmpUV, KeepAspectRatio, out tmpFOI);
		VirtualRegionOnScreen = tmpRegionOnScreen;
		if(tmpFOI.width >= 0 && tmpFOI.height >= 0)
			FieldOfIntrest = tmpFOI;
		Uv = tmpUV;
	}

	void Start () {
		StartOverride();
		
	}
	
	protected override void StartOverride(){
		base.StartOverride();
		calculateVirtualRegionOnScreen();
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

	public override void UpdateElement(bool updateChildren = true){
		if(!created)
			return;
		base.UpdateElement();
		
		if(FullscreenElement)
			ResizeToFullScreen();
		if(plane != null){
			plane.UV = Uv;
			plane.VirtualRegionOnScreen = RealRegionOnScreen;
		}
			
		
	}
	public override void CreateElement(){
		base.CreateElement();
		this.createGUIElement();
		//UpdateElement();
	}
	
	
	protected virtual void createGUIElement(){
		
		CreateGUIPlane();
				
		Camera cam = activeScreen.ScreenCamera; 
		plane.name = gameObject.name + "_guiPlane";
		plane.transform.parent = cam.transform;
		
		// Orient Plane to Camera
		plane.resetPlaneTransform();
		float layer = (float)GUIDepth * guiDepthFactor;
		plane.transform.Translate(new Vector3(0,0,(activeScreen.ScreenCamera.nearClipPlane+layer)), Space.Self);
		plane.transform.LookAt(cam.transform);
		
		// set Materials
		plane.GUIMaterial = activeScreen.GUIMaterial;
		plane.UV = Uv;
		plane.VirtualRegionOnScreen = RealRegionOnScreen;
		
			
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
	
	
	public void SetRotationTransformations(Vector2 localCenter, float degrees){
		plane.RotationAngle = degrees;
		plane.RotationCenter = localCenter;
	}
	
	private Vector3 worldToLocalCoordinates(Vector3 worldCoordinates){
		return gameObject.transform.InverseTransformPoint(worldCoordinates);
	}
	

	
	public override  bool CheckMouseOverElement(){
		return CameraScreen.CursorInsidePhysicalRegion(RealRegionOnScreen);
	}
	
	
	
	public override void ResetElement(){
		base.ResetElement();
		if(plane != null)
			plane.UV = Uv;
		
	}
	
	
	
	
}
