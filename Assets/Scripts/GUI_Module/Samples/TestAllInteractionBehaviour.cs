using UnityEngine;
using System.Collections;

public class TestAllInteractionBehaviour : InteractionBehaviour {

	public bool ReactOnClick = true;
	public bool ReactOnHover = true;
	public bool ReactOnMove = true;
	public bool ReactOnUp = true;
	public bool ReactOnDown = true;
	public bool ReactOnSwipe = true; 
	
	public override void Click(MouseEventArgs mouse){
		if(!ReactOnClick)
			return; 
		
		if(mouse.ElementIsActive)
			EditorDebug.Log("Click active Element: " + gameObject.name);
		else 
			EditorDebug.Log("Click inactive Element: " + gameObject.name);
			
	}
	
	public override void Hover(MouseEventArgs mouse){
		if(!ReactOnHover)
			return;
		
		if(mouse.ElementIsActive)
			EditorDebug.Log("Hover on active Element: " + gameObject.name);
		else
			EditorDebug.Log("Hover on inactive Element: " + gameObject.name);
	}
	
	public override void Down(MouseEventArgs mouse){
		if(!ReactOnDown)
			return;
		
		if(mouse.ElementIsActive)
			EditorDebug.Log("Mouse Down on active Element: " + gameObject.name);
		else 
			EditorDebug.Log("Mouse Down on inactive Element: " + gameObject.name);
	}
	
	public override void Up(MouseEventArgs mouse){
		if(!ReactOnUp)
			return;
		
		if(mouse.ElementIsActive)
			EditorDebug.Log("Mouse Up on active Element: " + gameObject.name);
		else 
			EditorDebug.Log("Mouse Up on inactive Element: " + gameObject.name);
	}
	
	public override void Move(MouseEventArgs mouse){
		if(!ReactOnMove)
			return;
		
		if(mouse.ElementIsActive)
			EditorDebug.Log("Mouse Move on active Element: " + gameObject.name + "\n" +
			"Moved Distance " + mouse.MoveDistance);
		else 
			EditorDebug.Log("Mouse Move on inactive Element: " + gameObject.name + "\n" +
			"Moved Distance " + mouse.MoveDistance);
	}
	
	public override void Swipe(MouseEventArgs mouse){
		if(!ReactOnSwipe)
			return;
		
		if(mouse.ElementIsActive)
			EditorDebug.Log("Swipe on active Element: " + gameObject.name);
		else 
			EditorDebug.Log("Swipe on inactive Element: " + gameObject.name);
	}
}
