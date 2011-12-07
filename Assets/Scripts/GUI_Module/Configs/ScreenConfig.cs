using UnityEngine;
using System.Collections;

public class ScreenConfig : MonoBehaviour {

	public int TargetScreenWidth = 960;
	public int TargetScreenHeight = 640;
	public double SwipeMinTime = 0.01;
	public float SwipeMinDistance = 150;
	
	public float ScreenAspect{
		get;
		private set;
	} 
	public int[] FontSizes;
	public Font[] Fonts;
	
	public static ScreenConfig Instance{
		get;
		private set;
	}
	
	void Awake(){
		Instance = this;
		ScreenAspect = (float)TargetScreenWidth / (float)TargetScreenHeight;
	}
	
	
	
	
	
}
