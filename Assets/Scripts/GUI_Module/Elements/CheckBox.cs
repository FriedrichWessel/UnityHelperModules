using UnityEngine;
using System.Collections;
using System;

public class CheckBox : Control {

	public Rect ActiveUV; 
	public Rect HoverUV;
	
	public event EventHandler<CheckBoxEventArgs> CheckboxStatusChanged;
		
	public bool Checked{
		get{
			return checkedFlag;
		}
		
		set{
			checkedFlag = value;
			if(!checkedFlag)
				resetElement();
			else{
				if(plane != null)
					plane.UV = ActiveUV;
			}
		}
	}
	
	private bool checkedFlag;
	
	protected override void AwakeOverride (){
		base.AwakeOverride ();
		Checked = false;
	}
	
	
	public override void OnClick (object sender, MouseEventArgs e){
		base.OnClick (sender, e);
		Checked = !Checked;	
		InvokeCheckboxStatusChanged();
	}

	
	public override void resetElement (){
		if(!checkedFlag)
			base.resetElement ();
		
		
	}
	
	private void InvokeCheckboxStatusChanged(){
		var handler = CheckboxStatusChanged;
		if (handler == null) {
			return;
		}
		
		var e = new CheckBoxEventArgs(this);
		CheckboxStatusChanged(this, e);
	}
}