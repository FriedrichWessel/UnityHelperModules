using UnityEngine;
using System.Collections;

public class Control : Panel {

	// Show Active Region is an EditorDebug Option that makes the active array visible
	public Text Label;
	public bool ShowActiveRegion = false;
	public Rect ActiveRegion;
	public bool ReadOnly = false;
	public Rect ReadOnlyUV;
	protected Rect realActiveRegion;
	protected bool down = false;
	
	
	
	void Awake(){
		AwakeOverride();
	}
	protected override void AwakeOverride(){
		base.AwakeOverride();
		
	}
	
	void Start () {
		
	}
	
	void OnGUI(){
#if UNITY_EDITOR
		if(ShowActiveRegion){
			initActiveRegion();
			UnityEngine.GUI.Box(realActiveRegion, "");	
		}
#endif 
	}
	
	public override void ResetElement (){
		base.ResetElement();
		if(Label != null)
			Label.IsActive = false;
		if(ReadOnly && plane != null)
			plane.UV = ReadOnlyUV;	
	}
	
	public override void CreateElement (){
		base.CreateElement();
		initActiveRegion();
	}
	public override bool CheckMouseOverElement(){
		if(ShowActiveRegion)
			initActiveRegion();
		if(!this.Visibility && this.ReadOnly)
			return false;
		
	
		return CameraScreen.CursorInsidePhysicalRegion(realActiveRegion);
	}
	
	#region Update
	
	public override void UpdateElement(bool updateChildren = true){
		base.UpdateElement(updateChildren);
		initActiveRegion();
		ResetElement();
		
	}
	
	protected override void firstUpdate (){
		base.firstUpdate ();
		if(ReadOnly && plane != null){
			plane.UV = ReadOnlyUV;	
		}
			
	}
	
	#endregion Update
	// Caclulate the Absolute Values on the physical screen - because ActiveRegion is virtual an relative to the Control Position

	private void initActiveRegion(){
		if(activeScreen == null){
			EditorDebug.LogWarning("ActiveScreen is not set on Object: " + gameObject.name);
			return;
		}
		var activeRegion = activeScreen.GetPhysicalRegionFromRect(ActiveRegion, KeepAspectRatio);
		realActiveRegion = new Rect(RealRegionOnScreen.x + activeRegion.x , RealRegionOnScreen.y + activeRegion.y, activeRegion.width, activeRegion.height);
	}
	
	#region Event Handler 
	

	public override void OnHover (object sender, MouseEventArgs e){
		base.OnHover (sender, e);
		if(ReadOnly)
			return;
		if(Label != null && ( !InputEvents.Instance.GetIsDown(0) || down) ){
			Label.IsActive = true;
		}
			
	}
	public override void OnDown(object sender, MouseEventArgs e){
		base.OnDown(sender,e);
		if(ReadOnly)
			return;
		down = true;
		if(Label != null){
			Label.IsActive = true;	
		}
			
	}
	
	public override void OnUp(object sender, MouseEventArgs e){
		base.OnUp(sender,e);
		if(ReadOnly)
			return;
		down = false;
		if(Label != null)
			Label.IsActive = false;
	}
	
	#endregion EventHandler

}
