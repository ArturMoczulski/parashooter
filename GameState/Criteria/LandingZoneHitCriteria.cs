using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandingZoneHitCriteria : GameStateCriteria {
	
	protected bool requiredHit;
	
	public LandingZoneHitCriteria(bool expectedValue) : base() {
		requiredHit = expectedValue;
	}
	
	public override bool satisfied(Level level) {
		if( requiredHit )
			return GameState.Instance.CurrentLevelLandingZoneHit == true;
		else
			return true;
	}
	
	public override string ToString() {
		return base.ToString()+": "+requiredHit;
	}
	
}
