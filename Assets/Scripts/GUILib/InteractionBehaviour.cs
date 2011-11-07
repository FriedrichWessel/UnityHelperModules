using UnityEngine;
using System.Collections;

public class InteractionBehaviour  : MonoBehaviour{

	public virtual void Click(MouseEventArgs mouse){}
	public virtual void Hover(MouseEventArgs mouse){}
	public virtual void Down(MouseEventArgs mouse){}
	public virtual void Up(MouseEventArgs mouse){}
	public virtual void Move(MouseEventArgs mouse){}
	public virtual void Swipe(MouseEventArgs mouse){}
	
}
