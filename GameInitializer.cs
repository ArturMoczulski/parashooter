using UnityEngine;
using System.Collections;

public class GameInitializer : MonoBehaviour {
	
	public int debug = 0;
	
	void Start() {
		
		// start the game
		Screen.orientation = ScreenOrientation.Landscape;
		GameState.Instance.CurrentLevelNumber = GameState.Instance.SelectedLevelNumber;
		GameState.Instance.LevelStarted = true;
		
	}
	
}
