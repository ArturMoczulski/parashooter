using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class ParashooterLevelsSettings : LevelsSettings {
	
	public List<ParashooterLevelSettings> levels;
	
	public ParashooterLevelSettings getLevelSettings(int levelNumber) {
		if( levelNumber > levels.Count )
			throw new UnityException("Level number "+levelNumber+" not set up.");
		
		return levels[levelNumber-1];
	}
	
}
