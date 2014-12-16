using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ParashooterLevelManager : LevelManagerWithGenerator {
	
	public ParashooterLevelsSettings levelsSettings;
	
	public bool isNextLevel(int currentLevel) {
		return currentLevel < levelsSettings.levels.Count && currentLevel > -1;
	}
	
	public override void onLevelStarted(LevelStartedEvent ev) {
		
		Time.timeScale = 1;
		
	}
	
	public override void onLevelFinished(LevelFinishedEvent ev) {
		
		Time.timeScale = 0;
		
	}
	
	public override void onLevelChanged(LevelChangedEvent gsEvent) {
		if( GameState.Instance.CurrentLevelNumber > levelsSettings.levels.Count ) {
			GameState.Instance.CurrentLevelNumber = levelsSettings.levels.Count;
		} else {
			base.onLevelChanged(gsEvent);
		}
	}
	
	public new void Awake() {
		
		base.Awake();
		ParashooterLevelGenerator levelGenerator = (ParashooterLevelGenerator)this.levelGenerator;
		levelGenerator.LevelsSettings = levelsSettings;
		
	}
		
	//public int getLevelsNumber() { return levelsSettings.
	
	void Reset() {
		levelGeneratorTypeName = typeof(ParashooterLevelGenerator).Name;
	}
	
}
