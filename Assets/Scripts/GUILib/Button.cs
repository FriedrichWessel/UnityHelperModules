using UnityEngine;
using System.Collections;

public class Button : Control {

	public GUIStyle HoverStyle;
	public GUIStyle ActiveStyle;
	
	
	
	public override void OnClick(object sender, MouseEventArgs e){
		base.OnClick(sender,e);
		currentStyle = ActiveStyle;	
	}
	
	public override void OnHover(object sender, MouseEventArgs e){
		base.OnHover(sender,e);
		currentStyle = InactiveStyle;
	}
	
	
}
