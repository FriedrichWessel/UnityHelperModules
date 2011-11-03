using UnityEngine;
using System.Collections;

public class TestAllInteractionsBehaviour : InteractionBehaviour {

	public override void Click(){
		Debug.Log("Click Element: " + gameObject.name);
	}
	
	public override void Hover(){
		Debug.Log("Hover Element: " + gameObject.name);
	}
	
	public override void Down(){
		Debug.Log("Mouse Down on Element: " + gameObject.name);
	}
	
	public override void Up(){
		Debug.Log("Mouse Up Element: " + gameObject.name);
	}
	
	public override void Move(){
		Debug.Log("Mouse Move Element: " + gameObject.name);
	}
	
	public override void Swipe(float degree){
		Debug.Log("Swipe Element: " + gameObject.name);
	}
}
