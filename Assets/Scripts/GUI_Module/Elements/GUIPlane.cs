using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIPlane : GUIGameObject {

	
	
	public Vector2 RotationCenter {
		get;
		set;
	}
	
	public float RotationAngle {
		get;
		set;
	}
	
	// Use this for initialization
	void Start () {
		StartOverride();
	}
	// Update is called once per frame
	void Update () {
		UpdateOverride();
	}
	void Awake(){
		AwakeOverride();
	}
	
	
	protected override void StartOverride(){
		base.StartOverride();
		activeScreen = CameraScreen.GetScreenForObject(this.gameObject);
		if(activeScreen == null)
			EditorDebug.LogWarning("No activeScreen found on GUIPlane: " + gameObject.name);
		updateTextureFactor();
	}
	
	protected override void UpdateOverride(){
		base.UpdateOverride();
	}
	
	protected override void AwakeOverride(){
		base.AwakeOverride();
	}
		
	private Vector2 RotateVertex(Vector2 vertex, Vector2 center, float degrees){
		var centeredScreen = vertex - center;
		return centeredScreen.Rotate(degrees) + center;
	}
	
	public Rect VirtualRegionOnScreen{
		set{
			var vertices = MeshObject.vertices;
			
			Rect tmp = value;
			vertices[3] = new Vector2(tmp.x, tmp.y); 
			vertices[0] = new Vector2(tmp.x+tmp.width, tmp.y);
			vertices[2] = new Vector2(tmp.x, tmp.y+tmp.height);
			vertices[1] = new Vector2(tmp.x+tmp.width, tmp.y+tmp.height); 
			
			var centerX = tmp.x + tmp.width * RotationCenter.x;
			var centerY = tmp.y + tmp.height * RotationCenter.y;
			var center = new Vector2(centerX, centerY);
			
			for(int i = 0; i < vertices.Length; i++){
				vertices[i] = RotateVertex(vertices[i], center, RotationAngle);
				vertices[i] = ScreenToWorldCoordinates(vertices[i]);
				//EditorDebug.Log("PRE Position: " + i + " " + vertices[i]);
				vertices[i] = WorldToLocalCoordinates(vertices[i]);
				vertices[i] = new Vector3(vertices[i].x, vertices[i].y*-1,0);
			}
			MeshObject.vertices = vertices;
		
		}	
		
	}

	
	//public Rect VirtualPosition
	public Rect UV{
		set{
			//updateTextureFactor();
        	Vector2[] uvs = new Vector2[4];
        	uvs[2] = new Vector2(value.x, value.y);
			uvs[1] = new Vector2(value.x+value.width, value.y);
			uvs[3] = new Vector2(value.x, value.y+value.height);
			uvs[0] = new Vector2(value.x+value.width, value.y+value.height);
			
			for(int i = 0; i < uvs.Length; i++){
				uvs[i] = toUVSpace(uvs[i]*textureFactor);
			}

			MeshObject.uv = uvs;
    	}
		
	}
	
	
	


}
