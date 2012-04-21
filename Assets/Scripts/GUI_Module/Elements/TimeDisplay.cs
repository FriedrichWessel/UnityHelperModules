using UnityEngine;
using System.Collections;
using System;

public class TimeDisplay : Text {
	 
	#region public Members - Inspector
	public bool ShowHours = false;
	public bool ShowMinutes = true;
	public bool ShowSeconds = true;
	public bool ShowMilliseconds = false;
	public bool ShowUnits = true;
	// this give the possibility to Work with the same time on many Timers
	public bool SlavedTimer = false;
	public TimeDisplay[] Slaves;
	#endregion
	
	#region public propertys
	public float time{
		get{
			return currentTime;
		}
		set{
			currentTime = value;	
			UpdateElement();
		}
	}
	public bool isRunning{
		get;
		private set;
	}
	
	#endregion
	
	#region private member
	private float currentTime = 0;
	
	
	#endregion
	
	
	#region init
	void Start(){
	  StartOverride();  
	}
	
	protected override void StartOverride (){
		base.StartOverride ();
		time = 0;
		formatTime();
		isRunning = false;
	}
	void Awake(){
		AwakeOverride();
	}
	#endregion init
	
	void Update(){
		UpdateOverride();
	}
	
	protected override void UpdateOverride (){
		base.UpdateOverride ();
		if(isRunning && !SlavedTimer){
			time += Time.deltaTime;
		}
		foreach(TimeDisplay slave in Slaves)
			slave.time = time;
		
	}
	
	public override void UpdateElement(bool updateChildren = true){
		base.UpdateElement(updateChildren);
		formatTime();
	}
	
	private void formatTime(){
		TimeSpan t = TimeSpan.FromSeconds( currentTime );

		var hours = string.Format("{0:D2}",t.Hours);
		var minutes = string.Format("{0:D2}",t.Minutes);
		var seconds = string.Format("{0:D2}",t.Seconds);
		var milliseconds = string.Format("{0:D3}",t.Milliseconds);
		
		string text = string.Empty;
		if(ShowHours){
			text += hours;
			if(ShowUnits)
				text += "h";
			if(ShowMinutes)
				text+= ":";
		}	
		if(ShowMinutes){
			text += minutes;
			if(ShowUnits)
				text += "m";
			if(ShowSeconds)
				text += ":";
		}
			
		if(ShowSeconds){
			text += seconds;
			if(ShowUnits)
				text += "m";
			if(ShowMilliseconds)
				text += ":";
		}
			
		if(ShowMilliseconds){
			text += milliseconds;
			if(ShowUnits)
				text += "s";
		}
			
		
		TextValue = text;
		
	}
	
	#region Clockcontrol
	public void StartTime(){
		isRunning = true;
		foreach(TimeDisplay slave in Slaves)
				slave.StartTime();
		
	}
	
	public void PauseTime(){
		isRunning = false;
		foreach(TimeDisplay slave in Slaves)
				slave.PauseTime();
		
	}
	
	public void StopTime(){
		isRunning = false;
		currentTime = 0;
		foreach(TimeDisplay slave in Slaves)
				slave.StopTime();
		
		
	}
	#endregion
}

