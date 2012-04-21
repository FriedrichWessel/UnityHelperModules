using UnityEngine;
using System.Collections;

public class InteractionBehaviour  : MonoBehaviour{

	private bool firstUpdateFlag = true;
	// DONT USE THIS
	void Awake(){
		AwakeOverride();
	}
	
	// USE THIS FOR INITIALISATION
	protected virtual void AwakeOverride(){
		
	}
	
	void Start(){
		StartOverride();
	}
	
	protected virtual void StartOverride(){
		
	}
	
	private void Update(){
		UpdateOverride();
	}
	
	protected virtual void UpdateOverride(){
		if(firstUpdateFlag){
			firstUpdateFlag = false;
			firstUpdate();
		}
	}
	
	protected virtual void firstUpdate(){
		// nothing there
	}
	
	public virtual void Click(MouseEventArgs mouse){}
	public virtual void Hover(MouseEventArgs mouse){}
	public virtual void Down(MouseEventArgs mouse){}
	public virtual void Up(MouseEventArgs mouse){}
	public virtual void Move(MouseEventArgs mouse){}
	public virtual void Swipe(MouseEventArgs mouse){}
	public virtual void TextChanged(string newText){}
	
}
