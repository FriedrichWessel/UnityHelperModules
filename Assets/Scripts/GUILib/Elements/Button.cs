using UnityEngine;
using System.Collections;

public class Button : Control {

	private GUIStyle hoverStyle;
	private GUIStyle activeStyle;
	
	
	
	public override void OnClick(object sender, MouseEventArgs e){
		base.OnClick(sender,e);
		currentStyle = activeStyle;	
	}
	
	public override void OnHover(object sender, MouseEventArgs e){
		base.OnHover(sender,e);
		currentStyle = hoverStyle;
	}
	
	protected override void initStyle(){
		base.initStyle();
		hoverStyle = new GUIStyle();
		activeStyle = new GUIStyle();
		hoverStyle.normal = Style.hover;
		activeStyle.normal = Style.active;
	}
	
	
}
