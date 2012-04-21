using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RadioCollection : Frame {

	public CheckBox DefaultCheckbox; 
	public bool OneMustBeSelected = true;
	private List<CheckBox> radioButtons;
	private CheckBox current;
	
	protected override void AwakeOverride (){
		base.AwakeOverride ();
		radioButtons = new List<CheckBox>();
		updateRadioButtons();
	}
	
	protected override void StartOverride (){
		base.StartOverride ();
	}
	
	private void enableDefaultCheckbox(){
		if(DefaultCheckbox != null){
			current = DefaultCheckbox;
			DefaultCheckbox.Checked = true;
		} else
			EditorDebug.LogError("No DefaultCheckbox found on Element: " + gameObject.name);
	}
	
	protected override void firstUpdate (){
		base.firstUpdate ();
		if(OneMustBeSelected)
			enableDefaultCheckbox();
	}
	
	private void updateRadioButtons(){
		var checkBoxChildren = gameObject.GetComponentsInChildren<CheckBox>() as CheckBox[];
		foreach(CheckBox cb in radioButtons){
			cb.CheckboxStatusChanged -= OnCheckboxStatusChanged;
		}
		radioButtons.Clear();
		foreach(CheckBox cb in checkBoxChildren){
			radioButtons.Add(cb);
			cb.CheckboxStatusChanged += OnCheckboxStatusChanged;
		}
		if(DefaultCheckbox == null && radioButtons.Count > 0)
			DefaultCheckbox = radioButtons[0];
	}
	
	private void OnCheckboxStatusChanged(object sender, CheckBoxEventArgs e){
		if(!created)
			return;
		
		bool flag = e.Checkbox.Checked;
		
		if(current != null && current != e.Checkbox && flag){
			// we need this tmp because the CheckboxChanged is Called immedatly 
			// -> this function is called again before change current, if we dont set old current = false via tmp
			var old = current;
			current = e.Checkbox;
			old.Checked = false;
		} else if(current == null || (!current.Checked && OneMustBeSelected) )
			enableDefaultCheckbox();
		
		
	}
	
	
}
