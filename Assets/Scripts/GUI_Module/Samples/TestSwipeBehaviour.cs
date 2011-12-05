using UnityEngine;
using System.Collections;

public class TestSwipeBehaviour : InteractionBehaviour {

	
	
	public override void Swipe(MouseEventArgs mouse){
		EditorDebug.Log("Swipe Element: " + gameObject.name);
	}
}
