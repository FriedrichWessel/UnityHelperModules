using UnityEngine;
using System.Collections;

public class GUIPlane : MonoBehaviour {
	
	public Mesh MeshObject;
	
	public Material GUIMaterial{
		get{
			return renderer.material;
		}
		set{
			renderer.material = value;
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
				Debug.Log(i + " PRE UVs : " + uvs[i]); 
				uvs[i] = toUVSpace(uvs[i]);
				//Debug.Log("POST UVs : " + uvs[i]); 
			}
				
			
			MeshObject.uv = uvs;
    	}
		
	}
	
	private Vector2 ScreenToWorldCoordinates(Vector2 screenCoordinate){
		return Vector2.zero;
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
