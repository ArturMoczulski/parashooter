using System;
using UnityEngine;

public class LevelChangedEvent : GameStateEvent
{
	
	private int levelNumberDelta;
	
	public int getLevelNumberBefore() {
		return GameState.Instance.CurrentLevelNumber + levelNumberDelta;
	}
	
	public LevelChangedEvent(int oldLevelNumber) {
		levelNumberDelta = Mathf.Abs(oldLevelNumber - GameState.Instance.CurrentLevelNumber);
	}
	
}
