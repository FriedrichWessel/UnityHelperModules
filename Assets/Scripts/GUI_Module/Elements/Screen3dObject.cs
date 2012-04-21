using UnityEngine;
using System.Collections;

public class Screen3dObject : Frame {
	
	public float MaxHitDistance = 100;
	private Collider colliderObject;
	
	// Use this for initialization
	void Start () {
		StartOverride();
	}
	
	protected override void StartOverride (){
		base.StartOverride ();
		initCollider();
	}
	
	private void initCollider(){
		colliderObject = gameObject.GetComponent<Collider>() as Collider;
		if(colliderObject == null){
			EditorDebug.LogError("No Collider set for Screen3dObject: "+ gameObject.name );
		}
	}

	
	public override bool CheckMouseOverElement(){
		base.CheckMouseOverElement();
		if(colliderObject == null)
			return false;

		var activeCamera = activeScreen.ScreenCamera;
		Ray ray  = activeCamera.ScreenPointToRay(Input.mousePosition);
    	RaycastHit hit = new RaycastHit();
    	if (colliderObject.Raycast (ray, out hit, MaxHitDistance)) {
			return true;
		} 
		return false;
		
		
	}
}
