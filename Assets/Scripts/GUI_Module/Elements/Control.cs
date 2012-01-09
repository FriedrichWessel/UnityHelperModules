using UnityEngine;
using System.Collections;

public class Control : Panel {

	// Show Active Region is an EditorDebug Option that makes the active array visible
	public bool ShowActiveRegion = false;
	public Rect ActiveRegion;
	protected Rect realActiveRegion;
	
	void Awake(){
		AwakeOverride();
	}
	protected override void AwakeOverride(){
		base.AwakeOverride();
		
	}
	
	void Start () {
		
	}
	
	public override void createGUIElement(){
		base.createGUIElement();
	}
	
	void OnGUI(){
#if UNITY_EDITOR
		if(ShowActiveRegion){
			initActiveRegion();
			UnityEngine.GUI.Box(realActiveRegion, "");	
		}
#endif 
	}
	
	public override void CreateElement (){
		base.CreateElement ();
		initActiveRegion();
	}
	public override bool checkMouseOverElement(){
		if(ShowActiveRegion)
			initActiveRegion();
		if(!this.Visibility)
			return false;
		
		//Rect t = new Rect(VirtualRegionOnScreen.x+ ActiveRegion.x, 
	//	                          VirtualRegionOnScreen.y + ActiveRegion.y, ActiveRegion.width, ActiveRegion.height);
		
		return CameraScreen.CursorInsidePhysicalRegion(realActiveRegion);
	}
	
	public override void UpdateElement(){
		base.UpdateElement();
		initActiveRegion();
		
	}
	// Caclulate the Absolute Values on the physical screen - because ActiveRegion is virtual an relative to the Control Position

	private void initActiveRegion(){
		if(activeScreen == null){
			EditorDebug.LogWarning("ActiveScreen is not set on Object: " + gameObject.name);
			return;
		}
		var activeRegion = activeScreen.GetPhysicalRegionFromRect(ActiveRegion, KeepAspectRatio);
		realActiveRegion = new Rect(RealRegionOnScreen.x + activeRegion.x , RealRegionOnScreen.y + activeRegion.y, activeRegion.width, activeRegion.height);
		/*realActiveRegion = new Rect(VirtualRegionOnScreen.x+ ActiveRegion.x, 
		                            VirtualRegionOnScreen.y + ActiveRegion.y, ActiveRegion.width, ActiveRegion.height);
		realActiveRegion = activeScreen.GetPhysicalRegionFromRect(realActiveRegion);
		var position = activeScreen.GetFloatingPosition();
		realActiveRegion = new Rect(position.x, position.y, RealRegionOnScreen.width, RealRegionOnScreen.height);*/
	}

}
