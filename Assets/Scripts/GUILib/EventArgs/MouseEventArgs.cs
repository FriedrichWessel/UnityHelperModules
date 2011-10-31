using UnityEngine;
using System.Collections;
using System;

public class MouseEventArgs : EventArgs {// where T : MonoBehaviour{

	public int buttonId{get;set;}
	
	public MouseEventArgs(int buttonId) {
		this.buttonId = buttonId;		
	}
		

}
