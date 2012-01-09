using UnityEngine;
using System.Collections;

public class DropDownElementBehaviour : InteractionBehaviour {

	private DropDownMenu containingDropDownElement;
	private Frame frame;
	public string Identifier;

	
	protected override void AwakeOverride (){
		base.AwakeOverride ();
		initDropDownElement();
		initGUIElement();
		if(Identifier == string.Empty)
			Identifier = gameObject.name;
	}
	
	private void initGUIElement(){
		frame = this.GetComponent<Frame>();
		if(frame == null)
			EditorDebug.LogError("DropDownElement: " + gameObject.name + " is not attached to an GUIElement");
	}
	private void initDropDownElement(){
		GameObject obj = this.gameObject;
		GameObject savedObj = obj;
		GameObject savedParent = null;
		DropDownMenu menu = null;
		while(obj != null){
			menu = obj.GetComponent<DropDownMenu>();
			if( menu != null){
				break;
			}
				
			if(obj.transform.parent != null)
				savedParent = obj.transform.parent.gameObject;
			else 
				savedParent = null;
			obj = savedParent;
		}
		if(menu == null){
			EditorDebug.LogWarning("Element: " + savedObj.gameObject.name + " is not a child of a DropDownMenu!");
		}
		containingDropDownElement = menu;
	}
	
	
	public override void Click (MouseEventArgs mouse){
		EditorDebug.Log("DropDownElement clicked");
		base.Click (mouse);
		if(containingDropDownElement == null){
			initDropDownElement();
			if(containingDropDownElement == null){
				EditorDebug.LogError("DropDownElement : " + gameObject.name + " has no DropDownElement as Parent");
				return;	
			}
			
		}
		containingDropDownElement.Selected = frame;
		containingDropDownElement.ToggleContentVisibility();
			
	}
}
