using UnityEngine;
using System.Collections;

public class Control : Box {

	// Show Active Region is an Debug Option that makes the active array visible
	public bool ShowActiveRegion = false;
	public Rect ActiveRegion;
	
	private Rect realActiveRegion;
	
	
	void Awake(){
		AwakeOverride();
	}
	protected override void AwakeOverride(){
		base.AwakeOverride();
		initActiveRegion();
	}
	
	
	void Start () {
		
	}
	
	public override void createGUIElement(){
		base.createGUIElement();

#if UNITY_EDITOR
		if(ShowActiveRegion){
			initActiveRegion();
			UnityEngine.GUI.Box(realActiveRegion, "");	
		}
#endif 
	}
	
	public override bool checkMouseOverElement(){
		if(ShowActiveRegion)
			initActiveRegion();
		Rect t = new Rect(Transformation.x+ ActiveRegion.x, 
		                          Transformation.y + ActiveRegion.y, ActiveRegion.width, ActiveRegion.height);
		return CameraScreen.cursorInside(t);
	}
	
	private void initActiveRegion(){
		
		realActiveRegion = new Rect(RealRegionOnScreen.x+ ActiveRegion.x, 
		                            RealRegionOnScreen.y + ActiveRegion.y, ActiveRegion.width, ActiveRegion.height);
		Rect t = activeScreen.GetRelativePosition(realActiveRegion);
		realActiveRegion = new Rect(realActiveRegion.x, realActiveRegion.y, t.width, t.height);
	}

}
