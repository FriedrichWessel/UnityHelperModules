using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DropDownMenuEventArgs : EventArgs {

	public DropDownMenu DropDownMenu{
		get;
		private set;
	}
	
	
	public DropDownMenuEventArgs(DropDownMenu menu) {
		this.DropDownMenu = menu;
		
	}
	
	

}
