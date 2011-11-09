using UnityEngine;
using System.Collections;

public class InteractionBehaviour  : MonoBehaviour{

	// DONT USE THIS
	void Awake(){
		AwakeOverride();
	}
	
	// USE THIS FOR INITIALISATION
	protected virtual void AwakeOverride(){
		
	}
	
	public virtual void Click(MouseEventArgs mouse){}
	public virtual void Hover(MouseEventArgs mouse){}
	public virtual void Down(MouseEventArgs mouse){}
	public virtual void Up(MouseEventArgs mouse){}
	public virtual void Move(MouseEventArgs mouse){}
	public virtual void Swipe(MouseEventArgs mouse){}
	
}
