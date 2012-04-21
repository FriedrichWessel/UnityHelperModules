using UnityEngine;
using System.Collections;

public class OpenDropDownMenuBehaviour : InteractionBehaviour {

	public DropDownMenu DropDown;
	
	public override void Click (MouseEventArgs mouse){
		base.Click (mouse);
		if(mouse.ElementIsActive)
			DropDown.ToggleContentVisibility();	
	}
}
