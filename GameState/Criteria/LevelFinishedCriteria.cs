using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelFinishedCriteria : GameStateCriteria {
	
	protected bool requiredValue;
	
	public LevelFinishedCriteria(bool expectedValue) : base() {
		requiredValue = expectedValue;
	}
	
	public override bool satisfied(Level level) {
		if( requiredValue )
			return GameState.Instance.LevelFinished == true;
		else
			return true;
	}
	
	public override string ToString() {
		return base.ToString()+": "+requiredValue;
	}
	
}
