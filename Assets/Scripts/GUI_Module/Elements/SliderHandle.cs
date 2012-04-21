using UnityEngine;
using System.Collections;

public class SliderHandle : Button {

	public Slider SliderElement{
		private get;
		set;
	}
	
	
	public override void OnMove (object sender, MouseEventArgs mouse){
		base.OnMove (sender, mouse);
		if(down){
			float mouseOffsetToElement = getMouseOffsetToElement(); 
			
			bool flag = SliderElement.SliderMaxPosition - SliderElement.SliderMinPosition < 0;
			if(flag)
				mouseOffsetToElement *= -1;
			
			var value = (mouseOffsetToElement*SliderElement.sliderValueRange) / SliderElement.sliderLength;
			SliderElement.SliderValue = value;
			//EditorDebug.Log("Slider Value: " + value + " Object: " + gameObject.name);
			
				
			
		}
	}
	
	private float getMouseOffsetToElement(){
		Vector2 realMouseOffset = new Vector2(0,0);
		realMouseOffset.x = Input.mousePosition.x - SliderElement.RealRegionOnScreen.x;
		realMouseOffset.y = CameraScreen.NormalizePhysicalMousePosition(Input.mousePosition).y - SliderElement.RealRegionOnScreen.y;
		
		float mouseOffsetToElement;
		if(SliderElement.Orientation == ElementOrientation.horizontal){	
			mouseOffsetToElement =  CameraScreen.PhysicalToVirtualScreenPosition(realMouseOffset).x;	
		}
		else{
			mouseOffsetToElement =  CameraScreen.PhysicalToVirtualScreenPosition(realMouseOffset).y;	
		}
		
		mouseOffsetToElement -= SliderElement.SliderMinPosition;
		return mouseOffsetToElement;
	}
	
	public override bool CheckMouseOverElement (){
		return ( ( base.CheckMouseOverElement () || down ) && !ReadOnly ) ;
	}
}
