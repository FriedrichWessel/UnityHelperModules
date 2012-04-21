using UnityEngine;
using System.Collections;
using System;

public class CheckBox : Button {


	public event EventHandler<CheckBoxEventArgs> CheckboxStatusChanged;
	public bool DefaultValue = false;
	
	public bool Checked{
		get{
			return checkedFlag;
		}
		
		set{
			checkedFlag = value;
			if(!checkedFlag){
				ConstantActive = false;
				ResetElement();
			}
				
			else{
				ConstantActive = true;
				if(Label != null)
					Label.IsActive = true;
				UpdateElement();
			}
			InvokeCheckboxStatusChanged();	
		}
	}
	
	private bool checkedFlag;
	
	protected override void AwakeOverride (){
		base.AwakeOverride ();
		Checked = DefaultValue;
	}
	
	
	public override void OnClick (object sender, MouseEventArgs e){
		if(ReadOnly){
			base.OnClick (sender, e);
			return;
		}
			
		if(InputEvents.Instance.IsActiveElement(this)){
			Checked = !Checked;	
			ConstantActive = Checked;
			
		}
		base.OnClick (sender, e);
		
		
	}
	
	public override void ResetElement (){
		base.ResetElement();
		if(checkedFlag && Label != null){
			Label.IsActive = true;
		}	
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