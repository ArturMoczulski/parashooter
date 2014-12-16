using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class GameStateDebugPopUp : ObjectDebugPopUp
{
	
	protected override object setObjectUnderDebug() {
		return GameState.Instance;
	}
	
	protected override string[] getIngoredPropertiesNames() {
		return new string[] {
			"Instance"
		};
	}
	
	protected void selectedLevelNumberProperty(MethodInfo getter, MethodInfo setter) {
		rangeIntegerPropertyWrite(
			getter,
			setter,
			1,
			((ParashooterLevelManager)ParashooterLevelManager.Instance).levelsSettings.levels.Count);
	}
	
	protected void currentLevelNumberProperty(MethodInfo getter, MethodInfo setter) {
		rangeIntegerPropertyWrite(
			getter,
			setter,
			1,
			((ParashooterLevelManager)ParashooterLevelManager.Instance).levelsSettings.levels.Count);
	}
	
	protected void afterCurrentLevelNumberSet() {
		GameState.Instance.LevelStarted = true;
	}
	
}