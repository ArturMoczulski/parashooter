using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GameStateCriteriaSetTemplate {
	
	public float score;
	public bool landingZoneHit;
	public bool levelFinished = true;
	
	public GameStateCriteriaSet createSet() {
		
		GameStateCriteriaSet criteriaSet = new GameStateCriteriaSet();
		
		LandingZoneHitCriteria landingZoneHitCriteria = new LandingZoneHitCriteria(landingZoneHit);
		criteriaSet.Add(landingZoneHitCriteria);
		
		LevelFinishedCriteria levelFinishedCriteria = new LevelFinishedCriteria(levelFinished);
		criteriaSet.Add(levelFinishedCriteria);
		
		if( score > 0 ) {
			ScoreCriteria scoreCriteria = new ScoreCriteria(score);
			criteriaSet.Add(scoreCriteria);
		}
		
		return criteriaSet;
		
	}
	
}
