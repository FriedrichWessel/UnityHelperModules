using System;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceManager
{
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
		UnityEngine.Object[] objects = Resources.LoadAll(path);
		List<string> loadedResources = new List<string>();
		foreach(UnityEngine.Object obj in objects){
			string resourcePath = path + "/" + obj.name;
			_resources.Add(resourcePath, obj);
			loadedResources.Add(obj.name);
		}
		return loadedResources;
	}
	
	public static void LoadResource(string key)
	{
		if (IsResourceLoaded(key)) {
			var message = string.Format("Resource {0} already loaded, skipping.", key);
			Debug.Log(message);
			return;
		}
		var r = UnityEngine.Resources.Load(key);
		if (r == null) {
			Debug.Log("resouce not found" + key + " while loading");
		}
		_resources.Add(key, r);
	}
	
	public static T GetResource<T>(string key) where T : UnityEngine.Object
	{
		if (!IsResourceLoaded(key)) {
			Debug.LogWarning(key + "not found");
		}
		return (T) _resources[key];
	}
	
	public static T CreateInstance<T>(string key) where T : UnityEngine.Object { 
		if (!IsResourceLoaded(key)) {
			Debug.LogWarning(key + "not found");
		}
		var res = _resources[key];
		return (T) GameObject.Instantiate(res, Vector3.zero, Quaternion.identity);
	}
}


