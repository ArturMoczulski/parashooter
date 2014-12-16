using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneChangerMenuCallbackManager : MenuCallbackManager {
	
	public List<string> sceneNames;
	
	public void Awake() {
		
		foreach(string sceneName in sceneNames) {
			
			StringClass sceneNameObj = new StringClass();
			
			if (sceneName == "")
				sceneNameObj = null;
			else
				sceneNameObj.stringValue = sceneName;
			
			callbackParameters.Add(sceneNameObj);
			
		}
		
	}
	
}
