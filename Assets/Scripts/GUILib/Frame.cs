using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Frame : MonoBehaviour {

	
	protected List<Box> directChildren;
	private delegate void InteractionEvent(InteractionBehaviour ib);
	private delegate void ActionEvent(Box b);
		
	// DONT USE THIS!
	void Awake () {
		AwakeOverride();
	}
	
	// Use this for initialization
	protected virtual void AwakeOverride(){
		initDirectChildren();
	}
	
	void Start(){
		
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
		callHandler(ib=>{ib.Click(e);}, action=>{action.OnClick(sender, e);});
	}
	
	public virtual void OnHover(object sender, MouseEventArgs e){
		callHandler(ib=>{ib.Hover(e);}, action=>{action.OnHover(sender, e);});
	}
	
	public virtual void OnDown(object sender, MouseEventArgs e){
		callHandler(ib=>{ib.Down(e);}, action=>{action.OnDown(sender, e);});
	}
	
	public virtual void OnUp(object sender, MouseEventArgs e){
		callHandler(ib=>{ib.Up(e);}, action=>{action.OnUp(sender, e);});
	}
	
	public virtual void OnMove(object sender, MouseEventArgs e){
		callHandler(ib=>{ib.Move(e);}, action=>{action.OnMove(sender, e);});
	}
	
	public virtual void OnSwipe(object sender, MouseEventArgs e){
		callHandler(ib=>{ib.Swipe(e);}, action=>{action.OnSwipe(sender, e);});
	}
	
	
	private void callHandler(InteractionEvent interaction, ActionEvent action){
		foreach(Box b in directChildren){
			if(b.checkMouseOverElement()){
				action(b);
				InteractionBehaviour[] behaviours = b.GetComponents<InteractionBehaviour>() as InteractionBehaviour[];
				foreach(InteractionBehaviour ib in behaviours){
					interaction(ib);
				}
			} else {
				b.resetElement();
			}
				
		}
	}
	

	
	private void initDirectChildren(){
		directChildren = new List<Box>();
		foreach(Transform child in transform){
			Box b = child.GetComponent<Box>();
			if(b != null)
				directChildren.Add(b);
		}
		
		
		
	}
}
