using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RollDisplayUpdater : GameStateEventHandler {
 
	public Texture background;
	public Texture horizon;
	public Texture pointer;
	public Texture indicator;
	public Texture step;
	public Texture limit;
	
	public Vector2 displayGUIPosition = new Vector2(50,50);
	public float backgroundSizeGUI = 64;
	public Vector2 horizonSizeGUI;
	public Vector2 pointerSizeGUI;
	public Vector2 indicatorSizeGUI;
	public float indicatorOffset = 10f;
	public Vector2 stepSizeGUI;
	public Vector2 limitSizeGUI;
	public float stepOffset = 0f;
	public float limitOffset = 0f;
	
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
			displayGUIPosition.x-backgroundSizeGUI,
			displayGUIPosition.y-backgroundSizeGUI,
			backgroundSizeGUI*2,
			backgroundSizeGUI*2),background);

		drawHorizon();
		drawPointer();
		drawIndicator();
		drawSteps();
 
	}
	
	void drawHorizon() {
		
		PlayerController playerController = player.GetComponent<PlayerController>();
		
		float textureAngle = -playerController.rollController.transform.eulerAngles.z;
		
		Matrix4x4 originalMatrix = GUI.matrix;
		GUIUtility.RotateAroundPivot(textureAngle,new Vector2(displayGUIPosition.x, displayGUIPosition.y));
		GUI.DrawTexture(new Rect(
			displayGUIPosition.x-horizonSizeGUI.x/2,
			displayGUIPosition.y,
			horizonSizeGUI.x,
			horizonSizeGUI.y),horizon);
		GUI.matrix = originalMatrix;
	}
	
	void drawPointer() {
		GUI.DrawTexture(new Rect(
			displayGUIPosition.x-pointerSizeGUI.x/2,
			displayGUIPosition.y,
			pointerSizeGUI.x,
			pointerSizeGUI.y),pointer);
	}
	
	void drawIndicator() {
		GUI.DrawTexture(new Rect(
			displayGUIPosition.x-indicatorSizeGUI.x/2,
			displayGUIPosition.y-(horizonSizeGUI.y - indicatorOffset),
			indicatorSizeGUI.x,
			indicatorSizeGUI.y),indicator);
	}
	
	void drawSteps() {
		
		PlayerController playerController = player.GetComponent<PlayerController>();
		
		float currentRoll = playerController.rollController.transform.eulerAngles.z;
		
		float maximumAngle = playerController.getMaximumRoll();
		
		drawStep(step, stepSizeGUI, stepOffset, currentRoll + 0f);
		
		for(int i = 1; i <= steps+1; i++) {
			if( i == steps ) {
				drawStep(limit, limitSizeGUI, limitOffset, currentRoll + maximumAngle);
				drawStep(limit, limitSizeGUI, limitOffset, currentRoll - maximumAngle);
			} else {
				drawStep(step, stepSizeGUI, stepOffset, currentRoll + maximumAngle/i);
				drawStep(step, stepSizeGUI, stepOffset, currentRoll - maximumAngle/i);
			}
		}		
		
	}
	
	void drawStep(Texture texture, Vector2 textureGUISize, float offset, float angle) {
		Matrix4x4 originalMatrix = GUI.matrix;
		GUIUtility.RotateAroundPivot(angle,new Vector2(displayGUIPosition.x, displayGUIPosition.y));
		GUI.DrawTexture(new Rect(
			displayGUIPosition.x-textureGUISize.x/2,
			displayGUIPosition.y-(horizonSizeGUI.y-offset),
			textureGUISize.x,
			textureGUISize.y),texture);
		GUI.matrix = originalMatrix;
	}
 
}

