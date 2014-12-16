using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectMissionMenuCallbackManager : MenuCallbackManager {
	
	public List<SelectMissionMenuCallback.Direction> selectMissionElements;
	
	public void Awake() {
		
		foreach(SelectMissionMenuCallback.Direction direction in selectMissionElements) {
			
			callbackParameters.Add(direction);
			
		}
		
	}
	
}
