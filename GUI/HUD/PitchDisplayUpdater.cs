using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PitchDisplayUpdater : GameStateEventHandler {
 
	public Texture background;
	public Texture horizon;
	public Texture indicator;
	public Texture step;
	
	public Vector2 displayGUIPosition = new Vector2(50,50);
	public Vector2 backgroundSizeGUI;
	public Vector2 horizonSizeGUI;
	public Vector2 indicatorSizeGUI;
	public Vector2 stepSizeGUI;
	
	public int steps = 2;
	
	protected GameObject player;
	
	public void onLevelStarted(LevelStartedEvent gsEvent) {
		if( ((ParashooterLevelManager)LevelManager.Instance).CurrentLevel == null )
			throw new UnityException("Level has not been generated yet. Level generation sequence problem.");
		player = ((ParashooterLevelManager)LevelManager.Instance).CurrentLevel.GetComponent<ParashooterLevel>().Player;
		if( player == null )
			throw new UnityException("Player object not found.");
	}
	
	void OnGUI() 
	{
		
	 	GUI.DrawTexture(new Rect(
			displayGUIPosition.x-backgroundSizeGUI.x/2,
			displayGUIPosition.y-backgroundSizeGUI.y/2,
			backgroundSizeGUI.x,
			backgroundSizeGUI.y),background);

		drawHorizon();
		drawIndicator();
		drawSteps();
 
	}
	
	void drawHorizon() {
		
		int textureOffset = calculateHorizonOffset();
		
		// clipping area
		GUI.BeginGroup(new Rect(
			displayGUIPosition.x-horizonSizeGUI.x/2,
			displayGUIPosition.y-horizonSizeGUI.y/2,
			horizonSizeGUI.x,
			horizonSizeGUI.y));
			
		// horizon texture
		GUI.DrawTexture(new Rect(
			0, 
			backgroundSizeGUI.y/2 - textureOffset, 
			horizonSizeGUI.x, horizonSizeGUI.y),horizon);
		
		GUI.EndGroup();
	}
	
	protected int calculateHorizonOffset()
	{
		PlayerController playerController = player.GetComponent<PlayerController>();
		
		float normalizedAngle = playerController.pitchController.transform.eulerAngles.x;
		
		normalizedAngle = normalizedAngle > 180 ? -(360 - normalizedAngle) : normalizedAngle;
		
		// Angle limits
		normalizedAngle = normalizedAngle > playerController.getMaximumPitch() ? 
			playerController.getMaximumPitch() : 
			normalizedAngle;
		normalizedAngle = normalizedAngle < -playerController.getMaximumRoll() ?
			- playerController.getMaximumPitch() :
			normalizedAngle;
		
		int textureOffset = (int)(normalizedAngle / playerController.getMaximumPitch() * (backgroundSizeGUI.y/2) );
		
		return textureOffset;
	}
	
	void drawIndicator() {
		GUI.DrawTexture(new Rect(
			displayGUIPosition.x + backgroundSizeGUI.x/2 - indicatorSizeGUI.x,
			displayGUIPosition.y,
			indicatorSizeGUI.x,
			indicatorSizeGUI.y),indicator);
	}
	
	void drawSteps() {
		
		PlayerController playerController = player.GetComponent<PlayerController>();
		
		for(int i = 1; i <= steps; i++) {
			
			// forward leaning steps
			GUI.DrawTexture(new Rect(
				displayGUIPosition.x + backgroundSizeGUI.x/2 - stepSizeGUI.x,
				displayGUIPosition.y - ( horizonSizeGUI.y/2 / (steps+1) )*i,
				stepSizeGUI.x,
				stepSizeGUI.y),
			step);
			
			// backwards leaning steps
			GUI.DrawTexture(new Rect(
				displayGUIPosition.x + backgroundSizeGUI.x/2 - stepSizeGUI.x,
				displayGUIPosition.y + ( horizonSizeGUI.y/2 / (steps+1) )*i,
				stepSizeGUI.x,
				stepSizeGUI.y),
			step);
		}		
		
	}
 
}

