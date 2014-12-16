using System;
using UnityEngine;

public class EndLevelPopUpDisplay : GameStateEventHandler
{
	
	public GUISkin skin;
	
	public GUIStyle headerStyle;
	public float fontSize;
	
	public Texture overlay;
	
	public int countdownLength = 3;
	
	protected float countdownStart = 0;
	protected bool counting = false;
	
	public void onLevelPassed(GameStateEvent gsEvent) {
		countdownStart = Time.realtimeSinceStartup;
		counting = true;
	}
	
	void Update() {
		
		if( counting && getCountdownLeft() <= 0 ) {
			
			counting = false;
			GameState.Instance.CurrentLevelNumber++;
			GameState.Instance.LevelStarted = true;
			
		}
		
	}
			
	protected int getCountdownLeft() {
		return Mathf.CeilToInt(countdownLength - (Time.realtimeSinceStartup - countdownStart) );
	}

	void OnGUI() 
	{
		GUI.skin = skin;
		if (GameState.Instance.LevelFinished || GameState.Instance.LevelPassed )
		{
			
			// Draw background
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), overlay);
		
			// Header
			drawHeader();			
			
			// Draw buttons
			drawButtons();
		
		}
	
	}
	
	protected void drawHeader() {
		
		string resultMessage = GameState.Instance.LevelPassed ? "Mission accomplished" : "Mission failed";
		
		GUILayout.BeginArea(new Rect(0,0, Screen.width, Screen.height/4 * 3)); GUILayout.BeginVertical(); GUILayout.FlexibleSpace();
		
		// Header
		GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
	
		headerStyle.fontSize = Screen.height/10;
		GUILayout.Label(resultMessage, headerStyle);
		GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
		
		if( counting ) {
			GUIStyle countdownStyle = new GUIStyle(headerStyle);
			countdownStyle.fontSize = (int)(headerStyle.fontSize*0.5);
			GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
			GUILayout.Label("next level starts in "+getCountdownLeft(), countdownStyle);
			GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
		}
		
		GUILayout.FlexibleSpace(); GUILayout.EndVertical(); GUILayout.EndArea();
		
	}
			
	protected void drawButtons() {
		
		GUIStyle buttonStyle = skin.button;
		buttonStyle.fontSize = (int)(Screen.height * fontSize);
		
		GUILayout.BeginArea(new Rect(0, Screen.height/4 * 3, Screen.width, Screen.height/4)); GUILayout.BeginVertical(); GUILayout.FlexibleSpace(); GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
		
		if( !GameState.Instance.LevelPassed ) {
			
			if( GUILayout.Button("Restart", buttonStyle) ) {
				GameState.Instance.CurrentLevelNumber = GameState.Instance.CurrentLevelNumber;
				GameState.Instance.LevelStarted = true;
			}
			
		}
			
		if( GUILayout.Button("Select mission", buttonStyle) ) {
			Application.LoadLevel("MissionSelectionScene");
		}
		
		GUILayout.FlexibleSpace(); GUILayout.EndHorizontal(); GUILayout.FlexibleSpace(); GUILayout.EndVertical(); GUILayout.EndArea();
			
	}

}


