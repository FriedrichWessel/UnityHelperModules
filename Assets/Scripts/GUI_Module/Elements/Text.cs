using UnityEngine;
using System.Collections;
using asdf.Resources;

public class Text : Frame {

	public int targetFontSize;
	public string TextValue;
	public bool Editable = false;
	public bool MultiLine;
	public int LineLength = 1;
	public int MaxInputTextLength = 10; 
	public int GUIDepth = 5;
	public Color TextColor = Color.black;
	

	public Rect TextRegion;
	//private Rect realTextRegion;
	private string formatetText;
	private string lastRenderdText;
	
	private GUITextPlane textComponent;
	
	//public FontStyle textStyle;
	
	// DONT USE THIS
	void Awake(){
		AwakeOverride();
	}
	
	protected override void AwakeOverride (){
		base.AwakeOverride ();
		
	}
	
	// Use this for initialization
	void Start () {
		StartOverride();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateOverride();
	}
	
	public override void CreateElement(){
		base.CreateElement();
		
		this.createGUIElement();
		created = true;
		UpdateElement();
	}
	
	
	public virtual void createGUIElement(){
		CreateTextMesh();
		
		if(textComponent == null){
			EditorDebug.LogError("Text Object: " + gameObject.name + " has no TextMesh assigned!");
			return;
		}
			
		
		textComponent.TextValue = TextValue;
		
		Camera cam = activeScreen.ScreenCamera; 
		//plane.name = gameObject.name + "_guiPlane";
		textComponent.transform.parent = cam.transform;
		
		// Orient Plane to Camera
		resetTextTransform();
		float layer = (float)GUIDepth * 0.0001f;
		textComponent.transform.Translate(new Vector3(0,0,(activeScreen.ScreenCamera.nearClipPlane+layer)), Space.Self);
		EditorDebug.LogError("Depth: " + (activeScreen.ScreenCamera.nearClipPlane+layer));
		//textComponent.transform.LookAt(cam.transform, new Vector3(0,1,0));
		textComponent.TextColor = TextColor;
		// set Materials
		//textComponent.GUIMaterial = activeScreen.GUIMaterial;
		//plane.UV = Uv;
		textComponent.VirtualRegionOnScreen = RealRegionOnScreen;
		//textComponent
		
	}
	
	public override void UpdateElement (){
		base.UpdateElement ();
		if(created)
			textComponent.VirtualRegionOnScreen = RealRegionOnScreen;	
		
	}
	
	private void CreateTextMesh(){
		GameObject go = ResourceManager.CreateInstance<GameObject>("TextMesh");
		if(go == null){
			EditorDebug.LogError("No GameObject found for TextMesh on Object "+ this.gameObject.name + "! Stop!");
			return;
		}
		
		textComponent = go.GetComponent<GUITextPlane>() as GUITextPlane;
		
		if(textComponent == null){
			EditorDebug.LogError("No TextMesh found on Object "+ this.gameObject.name + "! Stop!");
			return;
		}
		
		
	}
	
	private void resetTextTransform(){
		textComponent.transform.rotation = Quaternion.identity;
		textComponent.transform.localRotation = Quaternion.identity;
		textComponent.transform.localPosition = Vector3.zero;
		textComponent.transform.localScale = Vector3.one;
	}
	
	protected override void UpdateOverride(){
		base.UpdateOverride();
		formatText();
		if(this.Visibility){
			/*textComponent.fontStyle = textStyle;
			if(Editable)
				Text = UnityEngine.GUI.TextField(realTextRegion, formatetText, MaxInputTextLength, textStyle);
			else
				UnityEngine.GUI.Label(realTextRegion, formatetText, textStyle);	*/
		}
	}
	private void formatText(){
		if(!created)
			return;
		//EditorDebug.Log("Format Text Element: " + gameObject.name);
		//if(activeScreen.DebugModus)
		//	initTextRegion();
		textComponent.FontSize = targetFontSize;
#if UNITY_IPHONE || UNITY_ANDROID
		changeFontForMobile();
#elif UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
		textComponent.FontSize = CameraScreen.GetPhysicalTextSize(targetFontSize);
#endif
		scaleTextToSize();
		if(TextValue != lastRenderdText){ // Do this every on GUI because text can be changed from outside
			formatMultilineText();
			InvokeTextChanged();
		}
#if UNITY_EDITOR
		if(activeScreen.DebugModus)
			formatMultilineText();
#endif
		//UpdateElement();
	}
	
	private void scaleTextToSize(){
		float scale = textComponent.FontSize / 2500.0f;
		textComponent.transform.localScale = new Vector3(scale, scale, scale);
		
		
	}
	private void changeFontForMobile(){
		int index = 0;
		int size = targetFontSize;
		int fontSize = CameraScreen.GetPhysicalTextSize(size);
		foreach(int step in ScreenConfig.Instance.FontSizes){
			if(step >= fontSize){
				break;
			}
			index++;
				
		}
		if(index < ScreenConfig.Instance.Fonts.Length){
			textComponent.FontValue = ScreenConfig.Instance.Fonts[index];
		} else
			EditorDebug.LogWarning("No Font found that matches TargetFontSize: " + targetFontSize + " index: " + index + "Object: " + gameObject.name);
		
	}
	
	private void formatMultilineText(){
		string tmp = string.Empty;
		if(MultiLine){
			for(int i = 0; i < TextValue.Length; i++){
				tmp += TextValue[i];
				if((i+1)%LineLength == 0 && LineLength > 0)
					tmp += "\n";
			}
		} else
			tmp = TextValue;
		formatetText = tmp;
		lastRenderdText = TextValue;
	}
	
	private void InvokeTextChanged(){
		InteractionBehaviour[] behaviours = gameObject.GetComponents<InteractionBehaviour>() as InteractionBehaviour[];
		foreach(InteractionBehaviour ib in behaviours){
			ib.TextChanged(TextValue);
		}	
	}
	
}
