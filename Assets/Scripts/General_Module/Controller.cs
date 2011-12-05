using System;
using UnityEngine;

public abstract class Controller<T> where T : MonoBehaviour
{
	protected Controller () {
		// imediately active
		Trigger = () => true;
	}
	
	public Func<bool> Trigger {
		get;
		set;
	}
	
	public bool IsTriggered {
		get;
		private set;
	}
	
	public bool IsFinished {
		get;
		private set;
	}
	
	public virtual void Update(T entity)
	{
		if (Trigger == null) {
			return;
		}
		
		if (IsTriggered) {
			UpdateOverride(entity);
			return;
		}
		
		IsTriggered = Trigger();
	}
	
	protected abstract void UpdateOverride(T entity);
	
	public event EventHandler<BehaviourEventArgs<T>> ControllerFinished;
	protected void InvokeControllerFinished (T entity) {
		var handler = ControllerFinished;
		if (handler == null) {
			return;
		}
		
		var e = new BehaviourEventArgs<T> (entity);
		ControllerFinished (this, e);
		IsFinished = true;
	}
}


