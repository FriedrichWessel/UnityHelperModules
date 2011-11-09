using UnityEngine;
using System.Collections;
using System;

public class MouseEventArgs : EventArgs {// where T : MonoBehaviour{

	public int ButtonId{get;set;}
	public float MoveDistance{get{
			return MoveDirection.magnitude;		
		}
	}
	public Vector2 MoveDirection{get;set;}
	public Vector2 MousPosition{get;set;}
	
	public MouseEventArgs(int buttonId) {
		MousPosition = new Vector2(0,0);
		MoveDirection = new Vector2(0,0);
		this.ButtonId = buttonId;	
		
	}

	public MouseEventArgs(Vector2 direction) {
		MoveDirection = direction;
		ButtonId = -1;
	}

}
