using System;
using System.Collections.Generic;
using UnityEngine;




public class ResourceLoadingBehaviour : MonoBehaviour
{
	public string ResourcesPath; 
	public bool LoadAllResourcesInPath;
	public string[] ResourceNames;
	
	private List<string> loadedResources;
	
	protected ResourceLoadingBehaviour() {
	}

	private void Awake() {
		if(ResourcesPath != "")
				ResourcesPath += "/";
		OnResourcesLoading(EventArgs.Empty);
		LoadResources();
		OnResourcesLoaded(EventArgs.Empty);
	}
	
	protected virtual void OnResourcesLoading(EventArgs e) { }
	protected virtual void OnResourcesLoaded(EventArgs e) { }

	protected virtual void LoadResources(){
		if(LoadAllResourcesInPath){
			loadedResources = ResourceManager.LoadAllResources(ResourcesPath);
			
		} else{
			foreach(string resource in ResourceNames){
				ResourceManager.LoadResource(ResourcesPath  + resource);	
			}
		}
	}
	
	protected virtual void UnloadResources(){
		if(LoadAllResourcesInPath){
			ResourceManager.LoadAllResources(ResourcesPath);
		} else{
			foreach(string resource in loadedResources){
				ResourceManager.UnloadResource(ResourcesPath  + resource);	
			}
		}
	}
}


