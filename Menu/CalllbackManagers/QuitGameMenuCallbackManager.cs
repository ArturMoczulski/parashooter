using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuitGameMenuCallbackManager : MenuCallbackManager {
	
	public List<bool> quitElements;
	
	public void Awake() {
		
		foreach(bool quitElement in quitElements) {
			
			BoolClass quitElementObj = new BoolClass();
			quitElementObj.boolValue = quitElement;
			
			callbackParameters.Add(quitElementObj);
			
		}
		
	}
	
}
