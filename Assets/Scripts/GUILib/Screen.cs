using UnityEngine;
using System.Collections;

public class Screen : MonoBehaviour {

	public Camera ScreenCamera;
	
	void Start(){
		InputEvents.Instance.ClickEvent += OnClick;	
	}
	
	void OnGUI(){
		
	}
	
	private void OnClick(object sender, ClickEventArgs e){
		
	}
}
