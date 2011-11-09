using UnityEngine;
using System.Collections;

public class TestAllInteractionsBehaviour : InteractionBehaviour {

	public override void Click(MouseEventArgs mouse){
		Debug.Log("Click Element: " + gameObject.name);
	}
	
	public override void Hover(MouseEventArgs mouse){
		Debug.Log("Hover Element: " + gameObject.name);
	}
	
	public override void Down(MouseEventArgs mouse){
		Debug.Log("Mouse Down on Element: " + gameObject.name);
	}
	
	public override void Up(MouseEventArgs mouse){
		Debug.Log("Mouse Up Element: " + gameObject.name);
	}
	
	public override void Move(MouseEventArgs mouse){
		Debug.Log("Mouse Move Element: " + gameObject.name + "\n" +
			"Moved Distance " + mouse.MoveDistance);
	}
	
	public override void Swipe(MouseEventArgs mouse){
		Debug.Log("Swipe Element: " + gameObject.name);
	}
}
