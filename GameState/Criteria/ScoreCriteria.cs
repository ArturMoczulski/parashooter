using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreCriteria : GameStateCriteria {
	
	protected float requiredScore;
	
	public ScoreCriteria(float minimumValue) : base() {
		requiredScore = minimumValue;
	}
	
	public override bool satisfied(Level level) {
		return requiredScore <= GameState.Instance.CurrentLevelScore;
	}
	
	public override string ToString() {
		return base.ToString()+": "+requiredScore.ToString();
	}
	
}
