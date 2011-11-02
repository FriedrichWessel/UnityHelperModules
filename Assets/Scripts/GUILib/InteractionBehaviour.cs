using UnityEngine;
using System.Collections;

public abstract class InteractionBehaviour  : MonoBehaviour{

	public abstract void Click();
	public abstract void Hover();
	public abstract void Down();
	public abstract void Up();
	public abstract void Move();
	public abstract void Swipe(float degrees);
	
}
