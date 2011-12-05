using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIPlane : MonoBehaviour {

	private float textureFactor = 1.0f;
	private CameraScreen activeScreen;
	
	public Vector2 RotationCenter {
		get;
		set;
	}
	
	public float RotationAngle {
		get;
		set;
	}
	
	void Awake(){
		//updateTextureFactor();
	}
	void Start(){
		activeScreen = CameraScreen.GetScreenForObject(this.gameObject);
		if(activeScreen == null)
			EditorDebug.LogWarning("No activeScreen found on GUIPlane: " + gameObject.name);
		updateTextureFactor();
	}
	
	public Mesh MeshObject{
		get{
			return GetComponent<MeshFilter>().mesh;
		}
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
	public Material GUIMaterial{
		get{
			return renderer.sharedMaterial;
		}
		set{
			renderer.sharedMaterial = value;
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
	
	
	public Vector3 ScreenToWorldCoordinates(Vector2 screenCoordinate){
		
		Camera cam = transform.parent.GetComponent<Camera>();
		if(cam == null){
			EditorDebug.LogError("No camera found on Object: " + gameObject.name);
			throw new MissingComponentException("No camera found on Object: " + gameObject.name);
		}
		
		Ray r = cam.ScreenPointToRay(screenCoordinate);
		EditorDebug.DrawRay(r.origin, r.direction);
		// Switch x because Plane is looking at camera - so coordinate system is opposite, switching y because Camera has inverted space
		// in y in comparison to World
		//EditorDebug.Log("Origin: " + r.origin);
		//var ret = new Vector3(r.origin.x, r.origin.y*-1, 0);
		var ret = r.origin;
		return ret;
		
	}
	
	public Vector3 WorldToLocalCoordinates(Vector3 worldCoordinates){
		return gameObject.transform.InverseTransformPoint(worldCoordinates);
	}
	
	private Vector2 toUVSpace(Vector2 xy){
		if(xy.x < 1 && xy.y < 1)
			return xy;
		
		if(GUIMaterial == null)
			EditorDebug.LogWarning("Material : " + GUIMaterial + " on Object: " + gameObject.name);
		
		Texture t = GUIMaterial.GetTexture("_MainTex");
		var p = new Vector2(xy.x / ((float)t.width), xy.y / ((float)t.height));
		return p;
	}
	
	private void updateTextureFactor(){
		if(GUIMaterial != null && activeScreen != null){
			Texture t = GUIMaterial.GetTexture("_MainTex");
			textureFactor = (float)(t.width) / activeScreen.TextureSize;
		}
		
	}


}
