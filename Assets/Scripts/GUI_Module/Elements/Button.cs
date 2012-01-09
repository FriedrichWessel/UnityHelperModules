using UnityEngine;
using System.Collections;

public class Button : Control {

	public Rect hoverUV;
	public Rect activeUV;
	
	// Two Bools to emulate Hover and Active for better Texture placing
	public bool ConstantHover;
	public bool ConstantActive;
	
	protected bool down = false;
	
	
	public override void OnClick(object sender, MouseEventArgs e){
		base.OnClick(sender,e);
		
	}
	
	public override void OnDown(object sender, MouseEventArgs e){
		
			base.OnDown(sender,e);
			down = true;
			plane.UV = activeUV;	
		
		
	}
	
	public override void OnUp(object sender, MouseEventArgs e){
		if(down){	
			base.OnUp(sender,e);
			down = false;
			plane.UV = Uv;
		}
	}
	
	public override void OnHover(object sender, MouseEventArgs e){
		//EditorDebug.Log("HOVER: " + gameObject.name);
		base.OnHover(sender,e);
		if(!down)
			plane.UV = hoverUV;
	}
	
	public override void resetElement(){
		if(!down && plane != null){
			plane.UV = Uv;
#if UNITY_EDITOR
		if(ConstantHover)
				plane.UV = hoverUV;
		if(ConstantActive)
				plane.UV = activeUV;
#endif 
		}
	}
	
	
	
	
}
