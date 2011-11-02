using UnityEngine;
using System.Collections;
using System;

public class SwipeEventArgs : EventArgs {// where T : MonoBehaviour{

	public int FingerId{get;set;}
	public float Degree{get;set;}
	
	public SwipeEventArgs(int fingerId, float degree) {
		this.FingerId = fingerId;		
	}
		

}
