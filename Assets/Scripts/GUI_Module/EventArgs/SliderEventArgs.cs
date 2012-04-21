using UnityEngine;
using System.Collections;
using System;

public class SliderEventArgs : EventArgs {
	
	public float NewSliderValue{
		get;
		private set;
	}
	
	public float SliderDifference{
		get;
		private set;
	}
	
	private float oldSliderValue;
	
	public SliderEventArgs(float oldSliderValue, float newSliderValue){
		NewSliderValue = newSliderValue;
		SliderDifference = newSliderValue - oldSliderValue;
	}
}
