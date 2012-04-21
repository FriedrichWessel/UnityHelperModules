using UnityEngine;
using System.Collections;
using System;

public class MoveableEventArgs : EventArgs {
	
	public Panel sender;
	
	public MoveableEventArgs(Panel element){
		sender = element;	
	}
}
