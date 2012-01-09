using UnityEngine;
using System.Collections;

public class GUIGameObject : MonoBehaviour {

	public Material GUIMaterial{
		get{
			return renderer.sharedMaterial;
		}
		set{
			renderer.sharedMaterial = value;
		}
	}
	
	protected float textureFactor = 1.0f;
	
	protected CameraScreen activeScreen;
	
	
	public Mesh MeshObject{
		get{
			return GetComponent<MeshFilter>().mesh;
		}
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
	
	
	protected virtual void StartOverride(){
		
	}
	
	protected virtual void UpdateOverride(){
		
	}
	
	protected virtual void AwakeOverride(){
		
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
	
	protected Vector2 toUVSpace(Vector2 xy){
		if(xy.x < 1 && xy.y < 1)
			return xy;
		
		if(GUIMaterial == null)
			EditorDebug.LogWarning("Material : " + GUIMaterial + " on Object: " + gameObject.name);
		
		Texture t = GUIMaterial.GetTexture("_MainTex");
		var p = new Vector2(xy.x / ((float)t.width), xy.y / ((float)t.height));
		return p;
	}
	
	protected void updateTextureFactor(){
		if(GUIMaterial != null && activeScreen != null){
			Texture t = GUIMaterial.GetTexture("_MainTex");
			textureFactor = (float)(t.width) / activeScreen.TextureSize;
		}
		
	}
}
