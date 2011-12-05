using UnityEngine;
using System.Collections;

public class AnimatedUVBehaviour : UVMoveBehaviour {

	public int MovieSpeedFPS;
	public int FrameSize;
	public int FramesPerRow;
	public int RowCount;
	public int FramesPerTexture;
	public Texture2D[] Textures;
	
	private float frameTime;
	private int currentFrameNumber;
	private int frameCount;
	private int currentRow;
	private int currentColoum;
	private int currentTexture;
	//private int textureSize;
	private bool changeTexture;
	
	// Use this for initialization
	void Start () {
		StartOverride();
		
	}
	
	protected override void StartOverride(){
		base.StartOverride();
		//textureSize = mainTexture.height;
		//AbsoluteUVPosition = true;
		
	}
	
	void Awake(){
		AwakeOverride();
	}
	protected override void AwakeOverride(){
		base.AwakeOverride();
		frameTime = 0;
		currentFrameNumber = 0;
		currentRow = 0;
		currentColoum = 0;
		currentTexture = 0;
		frameCount = FramesPerRow * RowCount;
		changeTexture = false;
		
	}
	
	// Update is called once per frame
	/*void Update () {
	
	}*/
	
	
	void FixedUpdate(){
		
		frameTime += Time.deltaTime;
		
		if(frameTime > (1.0f/MovieSpeedFPS)){ // change frame
			
			currentFrameNumber ++;
			frameTime = 0;
			if(currentFrameNumber >= frameCount)
				currentFrameNumber = 0;
			
			changeFrame();
			
		}
	}
	
	private void changeFrame(){
		
		newUvs = new Rect(FrameSize*(currentColoum),FrameSize*(currentRow*-1) , 1,1);	
		if(changeTexture){
			//EditorDebug.LogWarning("ChangeTExture");
			renderer.material.SetTexture("_MainTex", Textures[currentTexture]);
			changeTexture = false;
		}
			
			
		currentColoum++;
		if(currentColoum >= FramesPerRow){
			currentColoum = 0;
			currentRow++;
		}			
		if(currentRow >= RowCount)
			currentRow = 0;
		if(currentFrameNumber%FramesPerTexture == 0){
			currentTexture++;
			if(currentTexture >= Textures.Length)
				currentTexture = 0;
			changeTexture = true;
			
		}
		//EditorDebug.Log("New UVs: " + newUvs + "Row: " + currentRow + "Coloumn: " + currentColoum);
		
	}
}
