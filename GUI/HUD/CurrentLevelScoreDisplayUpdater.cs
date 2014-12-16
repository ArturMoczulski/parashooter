using UnityEngine;
using System.Collections;

public class CurrentLevelScoreDisplayUpdater : GameStateEventHandler {
	
	public override void onLevelStarted(LevelStartedEvent gsEvent) {
		updateText();
	}
	
	public override void onCurrentLevelCheckpointsMissedChanged(CurrentLevelCheckpointsMissedChangedEvent gsEvent) {
		updateText();
	}
	
	public override void onCurrentLevelCheckpointsCheckedChanged(GameStateEvent gsEvent) {
		updateText();
	}
	
	private void updateText() {
		float currentScore = 0;
		try { 
			currentScore = GameState.Instance.CurrentLevelScore;
		} catch( UnityException ex ) {
			currentScore = 0;
		}
		
		this.GetComponent<GUIText>().text = ((int)currentScore).ToString()+"%";
	}
	
}
