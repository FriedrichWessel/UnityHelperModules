using UnityEngine;
using System.Collections;
using System;

public class InputEvents  {

	
	private static InputEvents instance;

	private InputEvents(){
	}
	
	public static InputEvents Instance
	{
		get{
			if (instance == null){
			instance = new InputEvents();
			}
			return instance;
		}
	}
	
	
	public event EventHandler<ClickEventArgs> ClickEvent;
	
	private void InvokeClickEvent(){
		var handler = ClickEvent;
		if (handler == null) {
			return;
		}
		var e = new ClickEventArgs();
		handler(this, e);
	}

}
