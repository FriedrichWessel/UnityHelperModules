using UnityEngine;
using System.Collections;
using System;

public class Slider : Control {

	public event EventHandler<SliderEventArgs> SliderValueChanged;
	
	public float SliderValue{
		get{
			return currentSliderValue;
		}
		set{
			lastSliderValue = currentSliderValue;
			if(value > MaxValue){
				EditorDebug.LogWarning("Slidervalue: " + value + " is out of Range on Object: " + gameObject.name + " value is set to max");
				currentSliderValue = MaxValue;
			} else if(value < MinValue){
				EditorDebug.LogWarning("Slidervalue: " + value + " is out of Range on Object: " + gameObject.name + " value is set to min");
				currentSliderValue = MinValue;
			}else
				currentSliderValue = value;
			
			UpdateElement();
			
			InvokeSliderValueChanged();
		}
	}
	
	public SliderHandle SliderHandle;
	public ElementOrientation Orientation = ElementOrientation.horizontal;
	
	public float DefaultValue;
	public float MinValue;
	public float MaxValue;
	
	
	public float SliderMinPosition;
	public float SliderMaxPosition;
	
	public float sliderLength{get;private set;}
	public float sliderValueRange{get;private set;}
	public float currentSliderPosition{get;private set;}
	private float currentSliderValue;
	private float lastSliderValue;

	
	protected override void AwakeOverride (){
		base.AwakeOverride ();
		initSliderValues();
	}
	
	private void initSliderValues(){
		sliderLength = Math.Abs(SliderMaxPosition - SliderMinPosition);
		sliderValueRange = Math.Abs(MaxValue - MinValue);
		SliderValue = DefaultValue;
		if(DefaultValue < MinValue)
			EditorDebug.LogWarning("Slider Default Value is smaller than min value on Object: "+gameObject.name+"- set to min Value!");
		if(DefaultValue > MaxValue)
			EditorDebug.LogWarning("Slider Default Value is bigger than max value on Object: "+gameObject.name+" - set to min Value!");		
		if(SliderHandle == null)
			EditorDebug.LogError("Slider Handle not set on: " + gameObject.name);
		else{
			SliderHandle.RemoveFloat();	
			SliderHandle.SliderElement = this;
		}
		
	}
	
	void Start(){
		StartOverride();
	}
	
	void Awake(){
		AwakeOverride();
	}
	
	protected override void StartOverride (){
		base.StartOverride ();
		SliderValue = DefaultValue;		
	}
	
	
	/*public override void OnMove (object sender, MouseEventArgs e){
		base.OnMove (sender, e);
		SliderHandle.OnMove(sender, e);
	}*/
	
	/*public override void OnDown (object sender, MouseEventArgs e){
		base.OnDown (sender, e);
		SliderHandle.OnDown(sender, e);
	}
	
	public override void OnUp (object sender, MouseEventArgs e){
		base.OnDown (sender, e);
		SliderHandle.OnUp(sender, e);
	}*/
	
	public override void UpdateElement (bool updateChildren = true){
		updateSliderPosition();
		base.UpdateElement (updateChildren);
	}
	
	protected override void firstUpdate (){
		base.firstUpdate ();
		
	}
	
	private void updateSliderPosition(){
		var offset = (currentSliderValue*sliderLength) /  sliderValueRange;
		
		var flag = SliderMaxPosition - SliderMinPosition < 0;
		if(flag)
			offset *= -1;
		//EditorDebug.Log("Offset: " + offset);
		currentSliderPosition = SliderMinPosition + offset;
		if(Orientation == ElementOrientation.horizontal){
			var value = currentSliderPosition - SliderHandle.VirtualRegionOnScreen.width/2;
			SliderHandle.VirtualRegionOnScreen = new Rect(value, SliderHandle.VirtualRegionOnScreen.y, SliderHandle.VirtualRegionOnScreen.width, SliderHandle.VirtualRegionOnScreen.height);
		}
		else{
			var value = currentSliderPosition - SliderHandle.VirtualRegionOnScreen.height/2;
			SliderHandle.VirtualRegionOnScreen = new Rect(SliderHandle.VirtualRegionOnScreen.x,value, SliderHandle.VirtualRegionOnScreen.width, SliderHandle.VirtualRegionOnScreen.height);
			//SliderHandle.VirtualRegionOnScreen.y = currentSliderPosition - SliderHandle.VirtualRegionOnScreen.height/2;
		}
			
		SliderHandle.UpdateElement();
	}
	
	private void InvokeSliderValueChanged(){
		var handler = SliderValueChanged;
		if (handler == null) {
			return;
		}

		var e = new SliderEventArgs(lastSliderValue, SliderValue);
		SliderValueChanged(this, e);
	}
}
