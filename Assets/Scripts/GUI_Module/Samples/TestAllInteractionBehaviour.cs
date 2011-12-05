using UnityEngine;
using System.Collections;

public class TestAllInteractionBehaviour : InteractionBehaviour {

	public override void Click(MouseEventArgs mouse){
		EditorDebug.Log("Click Element: " + gameObject.name);
	}
	
	public override void Hover(MouseEventArgs mouse){
		EditorDebug.Log("Hover Element: " + gameObject.name);
	}
	
	public override void Down(MouseEventArgs mouse){
		EditorDebug.Log("Mouse Down on Element: " + gameObject.name);
	}
	
	public override void Up(MouseEventArgs mouse){
		EditorDebug.Log("Mouse Up Element: " + gameObject.name);
	}
	
	public override void Move(MouseEventArgs mouse){
		EditorDebug.Log("Mouse Move Element: " + gameObject.name + "\n" +
			"Moved Distance " + mouse.MoveDistance);
	}
	
	public override void Swipe(MouseEventArgs mouse){
		EditorDebug.Log("Swipe Element: " + gameObject.name);
	}
}
