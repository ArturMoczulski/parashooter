using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParachuteOpenButton : GameStateEventHandler {
 
	void OnGUI() 
	{
		
		Vector2 size = new Vector2(Screen.width/8, Screen.height/20);
		
		Vector4 margin = new Vector4(0,Screen.width/20, Screen.height/20, 0);
			
		Rect buttonBounds = new Rect(
			Screen.width-size.x-margin.y, 
			Screen.height-size.y-margin.z, 
			size.x, 
			size.y
		);
			
	 	if( GUI.Button(buttonBounds, "Open parachute") ) {
			ParashooterLevel level = (ParashooterLevel)((ParashooterLevelManager)LevelManager.Instance).CurrentLevel.GetComponent<ParashooterLevel>();
			PlayerController player = level.Player.GetComponent<PlayerController>();
			player.parachuteTransform.GetComponent<Parachute>().Opened = true;
		}
		
	}
	
}
