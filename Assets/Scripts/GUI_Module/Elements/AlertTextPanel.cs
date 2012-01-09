using UnityEngine;
using System.Collections;
using System;
public class AlertTextPanel : TextPanel {
	
	public  int ShowTimeInSeconds = 8;
	
	public string help_alert = "ONLY IN EditorDebugMODUS:";
	public bool ShowAlways; 
	
	protected bool textShow = false;
	public Timer HidingTimer{
		get{
			return localTimer;
		}
		private set{
			localTimer = value;
		}
		
	}
	
	private Timer localTimer;
	
	void Awake(){
		AwakeOverride();
	}
	
	void Start(){
		StartOverride();
	}
	
	protected override void AwakeOverride(){
		base.AwakeOverride();
		initTimer();
		
		
	}
	
	private void initTimer(){
		if(HidingTimer == null)
			HidingTimer = new Timer(ShowTimeInSeconds);
		HidingTimer.TimerFinished += OnTimerFinished;
	}

	
	void Update(){
		UpdateOverride();
	}
	
	protected override void UpdateOverride(){
		base.UpdateOverride();
		Visibility = textShow;
#if UNITY_EDITOR
		Visibility |= ShowAlways;
#endif
	}
	
	void OnGUI(){
		OnGUIOverride();
	}
	
	protected override void OnGUIOverride(){
		if(textShow || ShowAlways){
			base.OnGUIOverride();	
		}
	}
	
	public void ShowText(string value){
		ShowText(value, ShowTimeInSeconds);
	}
	
	public void ShowText(string value, float time){
		initTimer();
		textShow = true;
		Text = value;
		HidingTimer.StartTimer(time);
	}
	public void HideText(){
		textShow = false;
	}
	
	private void OnTimerFinished(object sender, EventArgs e){
		textShow = false;
	}
}
