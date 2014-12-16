using UnityEngine;
using System.Collections;

public class SelectMissionMenuCallback : MenuElementCallback {
	
	public enum Direction { Next, Previous, None };
	
	override public void onChosen() {
		if (parameter != null) {
			
			Direction direction = (Direction)parameter;
			
			int levelsCount = ((ParashooterLevelManager)ParashooterLevelManager.Instance).levelsSettings.levels.Count;
			int newLevelNumber = GameState.Instance.SelectedLevelNumber;
			
			if( direction == Direction.Next ) 
				newLevelNumber = newLevelNumber >= levelsCount ? 1 : newLevelNumber+1;
			else if( direction == Direction.Previous ) 
				newLevelNumber = newLevelNumber <= 1 ? levelsCount : newLevelNumber-1;
			
			GameState.Instance.SelectedLevelNumber = newLevelNumber;
			
		}
	}
	
}
