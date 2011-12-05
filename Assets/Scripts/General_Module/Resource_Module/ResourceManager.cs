using System;
using System.Collections.Generic;
using UnityEngine;

namespace asdf.Resources{
	

public static class ResourceManager
{
	public static IEnumerable<string> Resource {
		get{
			return _resources.Keys;			
		}
	}
	private static readonly Dictionary<string, UnityEngine.Object> _resources;
	
	static ResourceManager() {
		_resources = new Dictionary<string, UnityEngine.Object>();
	}
	
	public static bool IsResourceLoaded(string key)
	{
		return _resources.ContainsKey(key);
	}
	
	public static bool UnloadResource(string key) {
		if (_resources.ContainsKey(key)) {
			_resources.Remove(key);
			return true;
		}
		return false;
	}
	
	public static List<string> LoadAllResources(string path){
		UnityEngine.Object[] objects = UnityEngine.Resources.LoadAll(path);
		List<string> loadedResources = new List<string>();
		foreach(UnityEngine.Object obj in objects){
			string resourcePath = path + "/" + obj.name;
			try{
				_resources.Add(resourcePath, obj);
			} catch(Exception e){
					EditorDebug.Log(e.Message);
			}
			loadedResources.Add(obj.name);
		}
		if(loadedResources.Count == 0)
				EditorDebug.LogWarning("No Resources found to load on: " + path);
		return loadedResources;
	}
	
	public static void LoadResource(string key)
	{
		if (IsResourceLoaded(key)) {
			var message = string.Format("Resource {0} already loaded, skipping.", key);
				EditorDebug.Log(message);
			return;
		}
		var r = UnityEngine.Resources.Load(key);
		if (r == null) {
			EditorDebug.Log("resouce not found" + key + " while loading");
		}
		_resources.Add(key, r);
	}
	
	public static T GetResource<T>(string key) where T : UnityEngine.Object
	{
		if (!IsResourceLoaded(key)) {
			EditorDebug.LogWarning("Key : " + key + " not found - Getresource");
			return null;
		}
		return (T) _resources[key];
	}
	
	public static T CreateInstance<T>(string key) where T : UnityEngine.Object { 
		if (!IsResourceLoaded(key)) {
			EditorDebug.LogWarning("Key: " + key + " not found - GetInstance");
		}
		var res = _resources[key];
		return (T) GameObject.Instantiate(res, Vector3.zero, Quaternion.identity);
	}
	

}
	
}
