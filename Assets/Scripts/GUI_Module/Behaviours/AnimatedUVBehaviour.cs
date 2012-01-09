using UnityEngine;
using System.Collections;

public class AnimatedUVBehaviour : UVMoveBehaviour {

	public int MovieSpeedFPS;
	public int FrameSizeX;
	public int FrameSizeY;
	public int TotalFramesCount;
	public int RowCount;
	public int ColumnCount;
	public Texture2D[] Textures;
	
	private float frameTime;
	private int currentFrameNumber;
	private int currentRow;
	private int currentColoum;
	private int currentTexture;
	private int framesPerTexture;
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
		framesPerTexture= ColumnCount * RowCount;
		changeTexture = false;
		
	}
	
	// Update is called once per frame
	/*void Update () {
	
	}*/
	
	
	void FixedUpdate(){
		
		frameTime += Time.deltaTime;
		
		if(frameTime > (1.0f/MovieSpeedFPS)){ // change frame
			
			var framesPlayed = (int)(frameTime*MovieSpeedFPS);
  			currentFrameNumber = (currentFrameNumber + framesPlayed) % TotalFramesCount;
  			frameTime -= framesPlayed/MovieSpeedFPS;
  			changeFrame();
			
			
		}
	}
	
	private void changeFrame(){
		
		newUvs = new Rect(FrameSizeX*(currentColoum),FrameSizeY*(currentRow*-1) , 1,1);	
		if(changeTexture){
			//EditorDebug.LogWarning("ChangeTExture");
			renderer.material.SetTexture("_MainTex", Textures[currentTexture]);
			changeTexture = false;
		}
			
			
		currentRow = (int)(currentFrameNumber%framesPerTexture)/ColumnCount;
		currentColoum = (int)(currentFrameNumber%framesPerTexture)/RowCount;
		int oldTexture = currentTexture;
		currentTexture = (int)(currentFrameNumber/framesPerTexture);
		if(oldTexture != currentTexture)
				changeTexture = true;
		
		
		//EditorDebug.Log("New UVs: " + newUvs + "Row: " + currentRow + "Coloumn: " + currentColoum);
		
	}
}
