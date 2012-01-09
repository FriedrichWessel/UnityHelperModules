using UnityEngine;
using System.Collections;

public class RolloutPanel : HVPanel {

	
	public float RolloutTime = 1.0f;
	
	public void Show(){
		Show(RolloutTime);
	}
	
	public void Hide(){
		Hide(RolloutTime);
	}
	
	public void Show(float timeInSeconds){
		// TODO: implement rollout
		EditorDebug.LogWarning("RolloutElement Show is only a dummy!");
		this.Visibility = true;	
		UpdateElement();
	}
	
	public void Hide(float timeInSeconds){
		//TODO: implement roll in
		EditorDebug.LogWarning("RolloutElement Hide is only a dummy!");
		this.Visibility = false;
		UpdateElement();
		
	}
}
