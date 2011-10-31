using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TimeBehaviour : MonoBehaviour {

	public static TimeBehaviour Instance{get;private set;}
	
	void Awake(){
		Instance = this;
	}
	private List<Timer> timerList;
	
	// Use this for initialization
	void Start () {
		timerList = new List<Timer>();
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Timer t in timerList)
			t.Update();
	}
	
	public void AddTimer(Timer timer){
		timerList.Add(timer);
	}
	
	public void RemoveTimer(Timer timer){
		timerList.Remove(timer);
	}
}
