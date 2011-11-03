using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Frame : MonoBehaviour {

	
	protected List<Box> directChildren;
	private delegate void InteractionEvent(InteractionBehaviour ib);
	private delegate void ActionEvent(Box b);
		
	// Use this for initialization
	protected void Awake () {
		Debug.Log("Frame Awake");
		initDirectChildren();
	}
	
	protected void Start(){
		
	}
	// Update is called once per frame
	void Update () {
	
	}
	
	/**
	 * This Function is called by Parent to force the child to arrange them selves 
	 **/
	public virtual void LayoutElement(){
		foreach(Box b in directChildren)
			b.LayoutElement();
		
		//do positioning etc. for this class here
	}
	
	
	public virtual void OnClick(object sender, MouseEventArgs e){
		callHandler(ib=>{ib.Click();}, action=>{action.OnClick(sender, e);});
	}
	
	public virtual void OnHover(object sender, MouseEventArgs e){
		callHandler(ib=>{ib.Hover();}, action=>{action.OnHover(sender, e);});
	}
	
	public virtual void OnDown(object sender, MouseEventArgs e){
		callHandler(ib=>{ib.Down();}, action=>{action.OnDown(sender, e);});
	}
	
	public virtual void OnUp(object sender, MouseEventArgs e){
		callHandler(ib=>{ib.Up();}, action=>{action.OnUp(sender, e);});
	}
	
	public virtual void OnMove(object sender, MouseEventArgs e){
		callHandler(ib=>{ib.Move();}, action=>{action.OnMove(sender, e);});
	}
	
	public virtual void OnSwipe(object sender, SwipeEventArgs e){
		callHandler(ib=>{ib.Swipe(e.Degree);}, action=>{action.OnSwipe(sender, e);});
	}
	
	
	private void callHandler(InteractionEvent interaction, ActionEvent action){
		foreach(Box b in directChildren){
			if(b.checkMouseOverElement()){
				action(b);
				InteractionBehaviour[] behaviours = b.GetComponents<InteractionBehaviour>() as InteractionBehaviour[];
				foreach(InteractionBehaviour ib in behaviours)
					interaction(ib);
			}
				
		}
	}
	

	
	private void initDirectChildren(){
		directChildren = new List<Box>();
		Box[] components = gameObject.GetComponentsInChildren<Box>();
		foreach(Box b in components){
			if(b.gameObject.transform.parent == this.gameObject)
				directChildren.Add(b);
		}
		
	}
}
