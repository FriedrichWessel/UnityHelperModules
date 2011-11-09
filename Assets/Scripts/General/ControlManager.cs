using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ControlManager<T> where T : MonoBehaviour
{
	private readonly List<string> keysToBeRemoved;
	private readonly Dictionary<string, Controller<T>> controllers;
	private readonly Dictionary<string, Controller<T>> queuedControllers;

	public ControlManager() {
		queuedControllers = new Dictionary<string, Controller<T>>();
		controllers = new Dictionary<string, Controller<T>>();
		keysToBeRemoved = new List<string>();
	}

	public void ClearControllers() {
		controllers.Clear();
	}
	
	public void Reset()
	{
		controllers.Clear();
		keysToBeRemoved.Clear();
		queuedControllers.Clear();
	}

	public void QueueController(string name, Controller<T> controller) {
		if (queuedControllers.ContainsKey(name)) {
			queuedControllers.Remove(name);
		}
		queuedControllers.Add(name, controller);
	}

	private void AddController(string name, Controller<T> controller) {
		if (controllers.ContainsKey(name)) {
			var message = string.Format("must remove controller with name {0}, before attaching a second with the same name.",name);
			throw new ApplicationException(message);
		}
		
		controller.ControllerFinished += (sender, e) => keysToBeRemoved.Add(name);
		controllers.Add(name, controller);
	}

	public void DequeueController(string name) {
		keysToBeRemoved.Add(name);
	}

	public bool IsControllerAttached(string name) {
		return controllers.ContainsKey(name);
	}

	public void Update(T entity) {
		foreach (var c in this.Controllers) {
			c.Update(entity);
		}
		
		foreach (var oc in keysToBeRemoved) {
			controllers.Remove(oc);
		}
		keysToBeRemoved.Clear();
		
		foreach (var qc in queuedControllers) {
			this.AddController(qc.Key, qc.Value);
		}
		queuedControllers.Clear();
	}

	public IEnumerable<Controller<T>> Controllers {
		get { return controllers.Values; }
	}
}


