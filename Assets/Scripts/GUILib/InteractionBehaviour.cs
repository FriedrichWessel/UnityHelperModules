using UnityEngine;
using System.Collections;

public class InteractionBehaviour  : MonoBehaviour{

	public virtual void Click(){}
	public virtual void Hover(){}
	public virtual void Down(){}
	public virtual void Up(){}
	public virtual void Move(){}
	public virtual void Swipe(float degrees){}
	
}
