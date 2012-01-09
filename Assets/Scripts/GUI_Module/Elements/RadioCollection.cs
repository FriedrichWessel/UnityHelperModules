using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RadioCollection : Frame {

	public CheckBox DefaultCheckbox; 
	
	private List<CheckBox> radioButtons;
	
	protected override void AwakeOverride (){
		base.AwakeOverride ();
		radioButtons = new List<CheckBox>();
		updateRadioButtons();
	}
	
	protected override void StartOverride (){
		base.StartOverride ();
		enableDefaultCheckbox();
	}
	
	private void enableDefaultCheckbox(){
		if(DefaultCheckbox != null)
			DefaultCheckbox.Checked = true;
		else
			EditorDebug.LogError("No DefaultCheckbox found on Element: " + gameObject.name);
	}
	
	protected override void firstUpdate (){
		base.firstUpdate ();
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
		EditorDebug.Log("CheckBoxes found: " + radioButtons);
		if(DefaultCheckbox == null && radioButtons.Count > 0)
			DefaultCheckbox = radioButtons[0];
	}
	
	private void OnCheckboxStatusChanged(object sender, CheckBoxEventArgs e){
		bool flag = e.Checkbox.Checked;
		foreach(CheckBox cb in radioButtons){
			if(cb.Checked)
				cb.Checked = false;
		}
		if(!flag)
			enableDefaultCheckbox();
		else
			e.Checkbox.Checked = true;
	}
	
	
}
