using UnityEngine;
using System.Collections;

public class GUIPlane : MonoBehaviour {
	
	public Mesh MeshObject;
	public Rect VirtualRegionOnScreen{
		set{
			var vertices = MeshObject.vertices;
			
			Debug.Log("ScreenCoords: " + value);
			Rect tmp = value;
			//tmp.x *= -1; tmp.y*=-1;
			vertices[3] = new Vector2(tmp.x, tmp.y); 
			vertices[0] = new Vector2(tmp.x+tmp.width, tmp.y);
			vertices[2] = new Vector2(tmp.x, tmp.y+tmp.height);
			vertices[1] = new Vector2(tmp.x+tmp.width, tmp.y+tmp.height); 
			
			//vertices[3] = CameraScreen.NormalizeScreenPosition(new Vector2(tmp.x, tmp.y));
			//vertices[0] = CameraScreen.NormalizeScreenPosition(new Vector2(tmp.x+tmp.width, tmp.y));
			//vertices[2] = CameraScreen.NormalizeScreenPosition(new Vector2(tmp.x, tmp.y+tmp.height));
			//vertices[1] = CameraScreen.NormalizeScreenPosition(new Vector2(tmp.x+tmp.width, tmp.y+tmp.height));
			
			for(int i = 0; i < vertices.Length; i++){
				Debug.Log(i + " PRE Coordinates : " + vertices[i]); 
				vertices[i] = ScreenToWorldCoordinates(vertices[i]);
				Debug.Log(i + " POST Coordinates : " + vertices[i]); 
	
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
        	Vector2[] uvs = new Vector2[4];
        	uvs[2] = new Vector2(value.x, value.y);
			uvs[1] = new Vector2(value.x+value.width, value.y);
			uvs[3] = new Vector2(value.x, value.y+value.height);
			uvs[0] = new Vector2(value.x+value.width, value.y+value.height);
			
			for(int i = 0; i < uvs.Length; i++){
				//Debug.Log(i + " PRE UVs : " + uvs[i]); 
				uvs[i] = toUVSpace(uvs[i]);
				//Debug.Log("POST UVs : " + uvs[i]); 
			}
				
			
			MeshObject.uv = uvs;
    	}
		
	}
	
	private Vector3 ScreenToWorldCoordinates(Vector2 screenCoordinate){
		//Debug.Log("ScreenCoords" +  screenCoordinate);
		
		Camera cam = transform.parent.GetComponent<Camera>();
		if(cam == null){
			Debug.LogError("No camera found on Object: " + gameObject.name);
			throw new MissingComponentException("No camera found on Object: " + gameObject.name);
		}
		
		Ray r = cam.ScreenPointToRay(screenCoordinate);
		Debug.DrawRay(r.origin, r.direction);
		// TODO COMMENT
		var ret = new Vector3(r.origin.x*-1, r.origin.y*-1, 0);
		//var ret = new Vector3(r.origin.x, r.origin.y, 0);
		return ret;//r.origin;//
		
	}
	
	private Vector2 WorldToScreenCoordinates(Vector3 worldCoordinate){
		
		return Vector2.zero;	
	}
	
	private Vector2 toUVSpace(Vector2 xy){
		if(xy.x < 1 && xy.y < 1)
			return xy;

		Texture t = GUIMaterial.GetTexture("_MainTex");
		var p = new Vector2(xy.x / ((float)t.width), xy.y / ((float)t.height));
		//p.y = 1.0f - p.y;
		return p;
	}


}
