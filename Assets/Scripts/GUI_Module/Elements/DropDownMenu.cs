using UnityEngine;
using System.Collections;
using System;

public class DropDownMenu : Frame {

	public event EventHandler<DropDownMenuEventArgs> SelectionChanged;
	public event EventHandler<DropDownMenuEventArgs> Open;
	public event EventHandler<DropDownMenuEventArgs> Closed;
	
	public RolloutPanel Content;
	public Button HeaderButton;
	public Rect CurrentElementRegion;
	public bool InitialOpen;
	public Frame InitalSelectedObject;
	
	private bool saveHeaderButtonConstantActive;
	public Frame Selected{
		get{
			return currentSelection;
		}
		set{
			oldSelection = currentSelection;
			currentSelection = value;
			Select(currentSelection);
			
		}
	}
	private Frame currentSelection;
	private Frame oldSelection;
	
	public void Select(string identifier){
		int count = 0;
		var children = Content.GetComponentsInChildren<DropDownElementBehaviour>() as DropDownElementBehaviour[];
		if(children != null){
			foreach(var dropDownElement in children){
				if(dropDownElement.Identifier == identifier){
					break;
				}
					
				count++;
			}
			Select(count);	
		}
	}
	
	public void Select(Frame element){
		int count = 0;
		var children = Content.GetComponentsInChildren<DropDownElementBehaviour>() as DropDownElementBehaviour[];
		var elementAsDropDown = element.GetComponent<DropDownElementBehaviour>() as DropDownElementBehaviour;
		if(elementAsDropDown == null){
			return;
		}
		if(children != null){
			foreach(var dropDownElement in children){
				
				if(dropDownElement.Identifier == elementAsDropDown.Identifier){
					break;
				}
					
				count++;
			}
			Select(count);	
		}
		
	}
	
	public void Select(int index){
		
		if(oldSelection != null){
			Destroy(oldSelection.gameObject);
		}
		
		var children = Content.GetComponentsInChildren<DropDownElementBehaviour>() as DropDownElementBehaviour[];
		if(index >= children.Length){
			EditorDebug.LogError("Index: " + index + " is out of bounds! While select DropDownElement");
			return;
		}
		var obj = Instantiate(children[index].gameObject) as GameObject;
		currentSelection = obj.GetComponent<Frame>() as Frame;
		var dropDownBehaviour = currentSelection.gameObject.GetComponent<DropDownElementBehaviour>() as DropDownElementBehaviour;
		if(dropDownBehaviour != null)
			dropDownBehaviour.Activate = false;
		
		currentSelection.activeScreen = this.activeScreen;
		currentSelection.Visibility = true;
		currentSelection.gameObject.transform.parent = this.gameObject.transform;
		currentSelection.VirtualRegionOnScreen = CurrentElementRegion;
		currentSelection.CreateElement();
		currentSelection.UpdateElement();
		invokeSelectionChanged();
	}
	
	public string GetSelectedIdentifier(){
		var dropDownBehaviour = currentSelection.gameObject.GetComponent<DropDownElementBehaviour>() as DropDownElementBehaviour;
		return dropDownBehaviour.Identifier;
	}
	protected override void firstUpdate (){
		base.firstUpdate ();
		if(!InitialOpen){
			Content.Hide(0);
		}
			

		if(InitalSelectedObject != null)
			Select(InitalSelectedObject);
		else{
			EditorDebug.LogWarning("DropDownMenu: " + gameObject.name + " has no InitiaElement Set - take Element zero");
			Select(0);
		}
		
			
	}
	
	public void ToggleContentVisibility(){
		if(Content.Visibility){
			Content.Hide();
			invokeClosed();
			HeaderButton.ConstantActive = saveHeaderButtonConstantActive;
			HeaderButton.UpdateElement();
		} else{
			saveHeaderButtonConstantActive = HeaderButton.ConstantActive;
			Content.Show();
			invokeOpen();
			HeaderButton.ConstantActive = true;
			HeaderButton.UpdateElement();
			
		}
			
	}
	
	private void invokeSelectionChanged() {
		var handler = SelectionChanged;
		if (handler == null) {
			return;
		}
		var e = new DropDownMenuEventArgs(this);
		handler(this, e);
	}
	
	private void invokeOpen() {
		var handler = Open;
		if (handler == null) {
			return;
		}
		var e = new DropDownMenuEventArgs(this);
		handler(this, e);
	}
	
	private void invokeClosed() {
		var handler = Closed;
		if (handler == null) {
			return;
		}
		var e = new DropDownMenuEventArgs(this);
		handler(this, e);
	}
	
}
