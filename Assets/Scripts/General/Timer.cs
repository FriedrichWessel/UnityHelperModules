using UnityEngine;
using System.Collections;
using System;

public class Timer {

	private float currentTime = 0;
	private float maxTime;
	private bool run = false;
	
	public event EventHandler TimerFinished;
	
	public Timer(){
		TimeBehaviour.Instance.AddTimer(this);
	}
	
	~Timer(){
		TimeBehaviour.Instance.RemoveTimer(this);
	}
	
	// Update is called once per frame
	public void Update () {
		if(run){
			currentTime += Time.deltaTime;
			if(currentTime >= maxTime){
				InvokeTimerFinished();
				StopTimer();
			}
		}
			
		
	}
	
	public void StartTimer(float timeInSeconds){
		StopTimer();
		maxTime = timeInSeconds;
		run = true;
	}
	
	public void StopTimer(){
		currentTime = 0;
		run = false;
	}
	
	private void InvokeTimerFinished(){
		var handler = TimerFinished;
		if (handler == null) {
			return;
		}
		handler(this, null);
	}
}
