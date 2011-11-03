using UnityEngine;
using System.Collections;

public class Control : Box {

	// Show Active Region is an Debug Option that makes the active array visible
	public bool ShowActiveRegion;
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
		if(ShowActiveRegion){
			initActiveRegion();
			//Debug.Log("RealActive Region for draw");
			//Debug.Log(realActiveRegion);
			UnityEngine.GUI.Box(realActiveRegion, "");	
		}
	}
	
	public override bool checkMouseOverElement(){
		if(ShowActiveRegion)
			initActiveRegion();
		Debug.Log("RealActiveRegion For Check : ");
		Debug.Log(realActiveRegion);
		Rect t = new Rect(RealRegionOnScreen.x+ ActiveRegion.x, 
		                            RealRegionOnScreen.y + ActiveRegion.y, ActiveRegion.width, ActiveRegion.height);
		return CameraScreen.cursorInside(t);
	}
	
	private void initActiveRegion(){
		
		realActiveRegion = new Rect(RealRegionOnScreen.x+ ActiveRegion.x, 
		                            RealRegionOnScreen.y + ActiveRegion.y, ActiveRegion.width, ActiveRegion.height);
		Rect t = activeScreen.GetRelativePosition(realActiveRegion);
		realActiveRegion = new Rect(realActiveRegion.x, realActiveRegion.y, t.width, t.height);
	}

}
