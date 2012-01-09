using UnityEngine;
using System.Collections;

public class NumberScrollField : Control {

	
	public TextPanel NumbersText;
	public int Digits = 0;
	public float ScrollSpeed = 1;
	public bool DisableUnlockedScrolling = false;
	
	public float DefaultValue;
	public float MinValue;
	public float MaxValue;
	public Slider.ElementOrientation Orientation;
	private bool down = false;
	
	public float Value{
		get{
			return currentValue;
		}
		set{
			if(value > MaxValue){
				EditorDebug.LogWarning("Value: " + value + " out of Range on Element: " + gameObject.name + " value is set to Max");
				currentValue = MaxValue;
			} else if(value < MinValue){
				EditorDebug.LogWarning("Value: " + value + " out of Range on Element: " + gameObject.name + " value is set to Min");
				value = MaxValue;
			} else
				currentValue = value;
			
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
	private Timer timer;
	
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
		timer = new Timer();
	}
	
	public override void UpdateElement(){
		base.UpdateElement ();
		updateText();
	}
	
	private void updateText(){
		
		if(NumbersText != null){
			NumbersText.Text = ((int)currentValue).ToString("D"+Digits.ToString());
			NumbersText.UpdateElement();	
		}
		
	}
	
	
	
	public override void OnUp (object sender, MouseEventArgs e){
		base.OnUp (sender, e);
		down = false;
	}
	public override void OnMove (object sender, MouseEventArgs e){
		base.OnMove (sender, e);
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
		if(!DisableUnlockedScrolling){
			// TODO: Do some unlocked Scrolling (like unlocked Mousewheel)	
		}
		
	}
	
	public override void OnDown (object sender, MouseEventArgs e){
		base.OnDown (sender, e);
		down = true;
		// TODO: Stop scrolling Numbers && stop changing over Time
	}
	
	public override bool checkMouseOverElement (){
		return ( base.checkMouseOverElement () || down ) ;
	}
	
}
