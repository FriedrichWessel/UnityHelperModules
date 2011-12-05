using System;
using UnityEngine;

public class BehaviourEventArgs<T> : EventArgs where T : MonoBehaviour
{
	public BehaviourEventArgs(T behaviour) {
		Behaviour = behaviour;
	}
	
	public T Behaviour {
		get;
		private set;
	}
}


