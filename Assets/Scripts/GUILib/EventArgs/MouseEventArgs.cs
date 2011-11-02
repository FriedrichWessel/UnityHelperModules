using UnityEngine;
using System.Collections;
using System;

public class MouseEventArgs : EventArgs {// where T : MonoBehaviour{

	public int ButtonId{get;set;}
	
	public MouseEventArgs(int buttonId) {
		this.ButtonId = buttonId;		
	}
		

}
