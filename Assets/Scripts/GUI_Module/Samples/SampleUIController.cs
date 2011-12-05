using System;
using UnityEngine;



public sealed class SampleUIController : Controller<MonoBehaviour>
{
	private Func<float, float> function;
	private TimeSpan elapsedTime = TimeSpan.Zero; 

	public SampleUIController()
		: this((x) => x) { }
	
	public SampleUIController(Func<float, float> function) {
		this.function = function;
	}
	
	public TimeSpan Duration {
		get;
		set;
	}
	
	public Vector2 StartPosition {
		get;
		set;
	}
	
	public Vector2 TargetPosition {
		get;
		set;
	}
	
	protected override void UpdateOverride (MonoBehaviour entity)
	{	
		
		/* UNTESTET
		if (IsFinished || entity == null) {
			return;
		}
		
		elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(Time.deltaTime));
		
		if (elapsedTime >= Duration) {
			elapsedTime = TimeSpan.Zero;
			InvokeControllerFinished(entity);
			return;
		}
		
		var relTime = (float)elapsedTime.TotalMilliseconds / (float)Duration.TotalMilliseconds;
		if (relTime > 1.0f) {
			relTime = 1.0f;
		}
		var relDistance = function(relTime); 
		// need more speed for we do operate on a larger scale than in game 
		
		var vec = (TargetPosition - StartPosition);
		
		//HACK for mono bug, something with trampolines
		var ui = (UIElementBehaviour<GUIManager>) entity;
		ui.Position = StartPosition + (vec * relDistance); */
	}		
}


