using UnityEngine;
using System.Collections;

public class DropDownMenu : Frame {

	public RolloutPanel Content;
	public Rect CurrentElementRegion;
	public bool InitialOpen;
	public Frame InitalSelectedObject;
	
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
	
	private void Select(Frame element){
		int count = 0;
		foreach(var frame in Content.DirectChildren){
			if(frame == element)
				break;
			count++;
		}
		Select(count);
	}
	
	private void Select(int index){
		if(oldSelection != null)
			Destroy(oldSelection.gameObject);
		
		var obj = Instantiate(Content.DirectChildren[index].gameObject) as GameObject;
		currentSelection = obj.GetComponent<Frame>() as Frame;
		var DropDownBehaviour = currentSelection.gameObject.GetComponent<DropDownElementBehaviour>() as DropDownElementBehaviour;
		if(DropDownBehaviour != null)
			Destroy(DropDownBehaviour);
		
		currentSelection.activeScreen = this.activeScreen;
		//currentSelection.UpdateActiveScreen();
		currentSelection.Visibility = true;
		currentSelection.gameObject.transform.parent = this.gameObject.transform;
		currentSelection.VirtualRegionOnScreen = CurrentElementRegion;
		currentSelection.CreateElement();
		currentSelection.UpdateElement();
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
		if(Content.Visibility)
			Content.Hide();
		else
			Content.Show();
	}
	
}
