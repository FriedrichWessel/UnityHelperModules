using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CheckBoxEventArgs : EventArgs {

	public CheckBox Checkbox{
		get;
		private set;
	}
	public Vector2 MoveDirection{get;set;}
	public Vector2 MousPosition{get;set;}
	
	public CheckBoxEventArgs(CheckBox checkbox) {
		this.Checkbox = checkbox;
		
	}


}
