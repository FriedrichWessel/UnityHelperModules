using UnityEngine;
using System.Collections;
using asdf.Resources;



public class Text : Frame {

	public int targetFontSize;
	public string TextValue;
	public bool Editable = false;
	public bool MultiLine;
	public bool ShowActiveRegion = false;
	public int LineLength = 1;
	public int MaxInputTextLength = 10; 
	public Color NormalTextColor = Color.black;
	public Color ActiveTextColor = Color.white;
	public Color DisabledTextColor = Color.gray;
	public Font FontType;
	
	public float TEXT_OBJECT_SCALE = 0.00225f;
	//private static float TEXT_OBJECT_SCALE = 0.0015f;	
	
	public bool IsActive{
		get{
			return isActive;
		}
		set{
			isActive = value;
			if(isActive)
				isDisabled = false;
			changeTextColor();
		}
	}
	
	public bool IsDisabled{
		get{
			return isDisabled;
		}
		set{
			isDisabled = value;
			if(isDisabled)
				isActive = false;
			changeTextColor();
		}
	}
	
	public override bool Visibility{
		get{
			return currentVisibility;
		}
		set{
			currentVisibility = value;
			if(textComponent != null)
				textComponent.renderer.enabled = value;
			
		}
	}
	
	public Rect ActiveRegion;
	
	protected Rect realActiveRegion;
	
	private bool isActive = false;
	private bool isDisabled = false;

	
	//private Rect realTextRegion;
	private string lastRenderdText;
	
	private GUITextPlane textComponent;
	
	//public FontStyle textStyle;
	
	#region Init Functions
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
	
	private void initActiveRegion(){
		if(activeScreen == null){
			EditorDebug.LogWarning("ActiveScreen is not set on Object: " + gameObject.name);
			return;
		}
		var activeRegion = activeScreen.GetPhysicalRegionFromRect(ActiveRegion, KeepAspectRatio);
		realActiveRegion = new Rect(RealRegionOnScreen.x + activeRegion.x , RealRegionOnScreen.y + activeRegion.y, activeRegion.width, activeRegion.height);
	}
	#endregion Init
	
	#region Creation Functions
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
		changeTextColor();
		textComponent.VirtualRegionOnScreen = RealRegionOnScreen;
		//textComponent
		
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
		go.name = gameObject.name + "_Text";
		textComponent.GUIMaterial = activeScreen.TextMaterial;
		textComponent.FontType = FontType;
		
		
	}
	
	#endregion Creation Functions
	
	void OnDestroy(){
		OnDestroyOverride();
	}
	
	protected virtual void OnDestroyOverride(){
		
		if(textComponent != null){
			GameObject.Destroy(textComponent.gameObject);
		}
	}
	
	void OnGUI(){
#if UNITY_EDITOR
		if(ShowActiveRegion){
			initActiveRegion();
			UnityEngine.GUI.Box(realActiveRegion, "");	
		}
#endif 
	}
	
	public override void UpdateElement(bool updateChildren = true){
		base.UpdateElement(updateChildren);
		if(created){
			textComponent.VirtualRegionOnScreen = RealRegionOnScreen;	
			textComponent.TextValue = TextValue;
		}
			
		
	}
	
	protected override void firstUpdate (){
		base.firstUpdate ();
		//TEXT_OBJECT_SCALE = 0.004f;
		//targetFontSize = 16;
		//if(targetFontSize == 22)
		//	VirtualRegionOnScreen.y += 2;
	}
	
	public override bool CheckMouseOverElement(){
		base.CheckMouseOverElement();
		initActiveRegion();
		if(!this.Visibility)
			return false;
		
		var flag = CameraScreen.CursorInsidePhysicalRegion(realActiveRegion);
		//if(flag)
		//	EditorDebug.LogError("Mouse Over Text: " + gameObject.name);
		return flag;
	}
	
	
	private void changeTextColor(){
		if(created){
			if(isActive){
				textComponent.TextColor = ActiveTextColor;
			} else if(isDisabled){
				textComponent.TextColor = DisabledTextColor;
			} else {
				textComponent.TextColor = NormalTextColor;
			}	
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
		textComponent.FontSize = targetFontSize;
		scaleTextToSize();
		if(TextValue != lastRenderdText){ // Do this every on GUI because text can be changed from outside
			formatMultilineText();
			InvokeTextChanged();
		}
#if UNITY_EDITOR
		if(activeScreen.DebugModus)
			formatMultilineText();
#endif
		
	}
	
	private void scaleTextToSize(){
		float scale = TEXT_OBJECT_SCALE;
		textComponent.transform.localScale = new Vector3(scale, scale, scale);
		
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
		lastRenderdText = TextValue;
	}
	
	private void InvokeTextChanged(){
		InteractionBehaviour[] behaviours = gameObject.GetComponents<InteractionBehaviour>() as InteractionBehaviour[];
		foreach(InteractionBehaviour ib in behaviours){
			ib.TextChanged(TextValue);
		}	
	}
	
}
