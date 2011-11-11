using UnityEngine;
using System.Collections;

public class Button : Control {

	public Rect hoverUV;
	public Rect activeUV;
	
	private bool active = false;
	
	
	
	public override void OnClick(object sender, MouseEventArgs e){
		base.OnClick(sender,e);
		
	}
	
	public override void OnDown(object sender, MouseEventArgs e){
		base.OnDown(sender,e);
		active = true;
		plane.UV = activeUV;
	}
	
	public override void OnUp(object sender, MouseEventArgs e){
		base.OnUp(sender,e);
		active = false;
		plane.UV = Uv;
	}
	
	public override void OnHover(object sender, MouseEventArgs e){
		base.OnHover(sender,e);
		if(!active)
			plane.UV = hoverUV;
		
	}
	
	public override void resetElement(){
		if(!active && plane != null){
			plane.UV = Uv;
		}
	}
	
	
	
	
}
