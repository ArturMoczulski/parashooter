using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

[Serializable]
public class LevelManager : GameStateEventHandler {
	
	protected static LevelManager instance;

	public static LevelManager Instance {
	
		get {
		
			if (instance == null) {
				instance = (LevelManager)UnityEngine.Object.FindObjectOfType(MethodBase.GetCurrentMethod().DeclaringType);
			}	
			return instance;	
		
		}
				
	}
	
	public virtual void reloadCurrentLevel() {}
}