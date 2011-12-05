using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MouseEventArgs : EventArgs {

	public int ButtonId{get;set;}
	public float MoveDistance{get{
			return MoveDirection.magnitude;		
		}
	}
	public Vector2 MoveDirection{get;set;}
	public Vector2 MousPosition{get;set;}
	
	public MouseEventArgs(int buttonId) {
		MouseDown = new Dictionary<int, bool>();
		MousPosition = new Vector2(0,0);
		MoveDirection = new Vector2(0,0);
		this.ButtonId = buttonId;	
		
	}

	public MouseEventArgs(Vector2 direction) {
		MoveDirection = direction;
		ButtonId = -1;
	}
	
	public Dictionary<int,bool> MouseDown;

}
