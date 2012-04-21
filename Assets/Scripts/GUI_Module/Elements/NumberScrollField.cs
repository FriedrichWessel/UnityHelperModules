using System;
using UnityEngine;
using System.Collections;
using System.Globalization;

public class NumberScrollField : Button {


	public Text NumbersText;
	public int Digits = 0;
	public int FractionalDigits = 0;
	public float ScrollSpeed = 1;
	public bool DisableUnlockedScrolling = false;

	public float DefaultValue;
	public float MinValue;
	public float MaxValue;
	public Slider.ElementOrientation Orientation;


	public event EventHandler<SliderEventArgs> ValueChanged;

	public float Value{
		get{
			return currentValue;
		}
		set{
			var old = currentValue;
			if(value > MaxValue){
				EditorDebug.LogWarning("Value: " + value + " out of Range on Element: " + gameObject.name + " value is set to Max");
				currentValue = MaxValue;
			} else if(value < MinValue){
				EditorDebug.LogWarning("Value: " + value + " out of Range on Element: " + gameObject.name + " value is set to Min");
				value = MaxValue;
			} else
				currentValue = value;

			invokeValueChangedEvent(old);
			UpdateElement();
		}
	}

	public float TargetValue{
		get;
		set;
	}

	public float ChangeTime{
		get;
		set;
	}


	private float currentValue;

	void Awake(){AwakeOverride();}

	protected override void AwakeOverride (){
		base.AwakeOverride ();
		init();

	}

	private void init(){
		if(NumbersText == null)
			EditorDebug.LogError("TextPanel for Numbers is not set on Element " + gameObject.name);
		TargetValue = DefaultValue;
		Value = DefaultValue;
	}

	public override void UpdateElement(bool updateChildren = true){
		base.UpdateElement(updateChildren);
		updateText();
	}

	protected override void firstUpdate (){
		base.firstUpdate ();
		//Label.targetFontSize = 22;
		//Label.VirtualRegionOnScreen.x += 1;
	}

	private void updateText(){

		if(NumbersText != null){
			var value = NumberFormat.NumberToString(currentValue, Digits, FractionalDigits);
			NumbersText.TextValue = value;
			NumbersText.UpdateElement();

		}

	}

	public override bool CheckMouseOverElement (){
		return ( base.CheckMouseOverElement () || down ) ;
	}

	#region Event Handling

	public override void OnDown (object sender, MouseEventArgs e){
		if(ReadOnly){
			base.OnDown (sender, e);
			return;
		} else {
			ConstantActive = true;
			base.OnDown (sender, e);
		}


		// TODO: Stop scrolling Numbers && stop changing over Time
	}

	public override void OnUp(object sender, MouseEventArgs e){
		if(ReadOnly){
			base.OnUp(sender,e);
			return;
		} else {
			if(down){
				ConstantActive = false;
			}
			base.OnUp(sender,e);
		}

	}


	private void invokeValueChangedEvent(float oldValue) {
		var handler = ValueChanged;
		if (handler == null) {
			return;
		}
		var e = new SliderEventArgs(oldValue, Value);
		handler(this, e);
	}

	public override void OnMove (object sender, MouseEventArgs e){
		base.OnMove (sender, e);
		if(ReadOnly)
			return;

		if(down){
			var scrollLength = new Vector2(0,0);
			float changeValue = 0;
			if(Orientation == ElementOrientation.horizontal){
				scrollLength.x = e.MoveDirection.x;
				changeValue = scrollLength.magnitude;
				if(e.MoveDirection.x < 0)
					changeValue *= -1;

			} else {
				scrollLength.y = e.MoveDirection.y;
				changeValue = scrollLength.magnitude;
				if(e.MoveDirection.y > 0)
					changeValue *= -1;
			}
			Value += changeValue * ScrollSpeed;

		}

	}

	public override void OnSwipe (object sender, MouseEventArgs e){
		base.OnSwipe (sender, e);
		if(ReadOnly)
			return;
		if(!DisableUnlockedScrolling){
			// TODO: Do some unlocked Scrolling (like unlocked Mousewheel)
		}

	}

	#endregion EventHandling





}
