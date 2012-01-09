using UnityEngine;
using System.Collections;

public class GUITextPlane : GUIGameObject {

	private TextMesh textComponent;
	
	#region PROPERTYS
	public Rect VirtualRegionOnScreen{
		set{
			
			var tmpValue = value;
			
			tmpValue.y = Screen.height - tmpValue.y;
			
			var worldPosition =  ScreenToWorldCoordinates(new Vector2(tmpValue.x, tmpValue.y));
			textComponent.transform.position = new Vector3(worldPosition.x, worldPosition.y, textComponent.transform.position.z);
			
			
		}
		
	}	
	
	public string TextValue{
		get{
			return textComponent.text;
		}
		set{
			textComponent.text = value;
		}
	}
	
	public int FontSize{
		get{
			return textComponent.fontSize;
		}
		set{
			textComponent.fontSize = value;
		}
	}
	
	public FontStyle Style{
		get{
			return textComponent.fontStyle;
		}
		set{
			textComponent.fontStyle = value;
		}
	}
	
	public Font FontValue{
		get{
			return textComponent.font;
		} set{
			textComponent.font = value;
		}
	}
	
	public Color TextColor{
		get{
			return GUIMaterial.color;
		}
		set{
			GUIMaterial.color = value;
		}
	}
	#endregion
	
	#region Start,Awake,Update
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
	}
	
	protected override void UpdateOverride(){
		base.UpdateOverride();
	}
	
	protected override void AwakeOverride(){
		base.AwakeOverride();
		textComponent = GetComponent<TextMesh>() as TextMesh;
	}
	#endregion
	
	
}
