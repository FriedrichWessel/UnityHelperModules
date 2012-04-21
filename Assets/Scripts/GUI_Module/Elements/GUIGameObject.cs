using UnityEngine;
using System.Collections;

public class GUIGameObject : MonoBehaviour {

	public virtual Material GUIMaterial{
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
		activeScreen = CameraScreen.GetScreenForObject(this.gameObject);
	}
	
	protected virtual void UpdateOverride(){
		
	}
	
	protected virtual void AwakeOverride(){
		
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
