using UnityEngine;
using System.Collections;

public class SpeedDisplayUpdater : GameStateEventHandler {
	
	protected GameObject player = null;
	
	public void onLevelStarted(LevelStartedEvent gsEvent) {
		player = ((ParashooterLevel)((ParashooterLevelManager)ParashooterLevelManager.Instance).CurrentLevel.GetComponent<ParashooterLevel>()).Player;
	}
	
	void Update() {
		updateText();
	}
	
	private void updateText() {
		
		if( player != null ) {
			int mph = Mathf.CeilToInt(Mathf.Abs(player.rigidbody.velocity.y) * 2.2369f);
			this.GetComponent<GUIText>().text = mph.ToString() + " m/h";
		}
		
	}
	
}
