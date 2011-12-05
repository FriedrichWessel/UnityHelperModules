using UnityEngine;
using System.Collections;

public class TestMoveWithMouse : InteractionBehaviour {

	private bool down = false;
	
	/** 
	 * BTW: This class is super cool for position Elements with Drag Drop on Screen in EditorDebugModus :) 
	 * */
	public override void Down(MouseEventArgs mouse){
		down = true;	
	}
	
	public override void Up(MouseEventArgs mouse){
		down = false;
	}
	
	public override void Move(MouseEventArgs mouse){
		if(down){
			Panel ui = gameObject.GetComponent<Panel>();
			Vector2 newPosition = new Vector2(ui.VirtualRegionOnScreen.x, ui.VirtualRegionOnScreen.y);
			newPosition += mouse.MoveDirection;
			Rect newRegion = new Rect(newPosition.x, newPosition.y, ui.VirtualRegionOnScreen.width, ui.VirtualRegionOnScreen.height);
			ui.VirtualRegionOnScreen = newRegion;
		}
	}
}
