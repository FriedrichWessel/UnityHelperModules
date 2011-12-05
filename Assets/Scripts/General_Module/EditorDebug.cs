using UnityEngine;
using System.Collections;

public static class EditorDebug  {
	
	public static bool ShowDebugOutsideEditor;
	public static void Log(string message){
#if UNITY_EDITOR
		Debug.Log(message);
#endif
		if(ShowDebugOutsideEditor)
			Debug.Log(message);
	}
	
	public static void LogWarning(string message){
#if UNITY_EDITOR
		Debug.LogWarning(message);
#endif
		if(ShowDebugOutsideEditor)
			Debug.LogWarning(message);
	}
	
	public static void LogError(string message){
#if UNITY_EDITOR
		Debug.LogError(message);
#endif
		if(ShowDebugOutsideEditor)
			Debug.LogError(message);
	}
	
	public static void DrawRay(Vector3 start, Vector3 dir){
#if UNITY_EDITOR
		Debug.DrawRay(start, dir);
#endif
	}
}
