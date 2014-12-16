using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandingZone : LightsCircle {
	
	protected override void positionLights() {
		
		base.positionLights();
		
		// Don't position the lights on the ground as the model for the landing zone
		// take under consideration possible uneven ground level.
		/*
		ParashooterLevel currentLevel = ((ParashooterLevelManager)LevelManager.Instance).CurrentLevel.GetComponent<ParashooterLevel>();
		Terrain terrain = (Terrain)currentLevel.TerrainObject.GetComponent<Terrain>();
		foreach(GameObject light in lights) {
			light.transform.Translate(new Vector3(0,terrain.SampleHeight(light.transform.position)-light.transform.position.y,0));
		}
		*/
		
	}
	
	void OnCollisionEnter(Collision collision) {
		if( collision.gameObject.tag == "Player" ) 
			GameState.Instance.LevelFinished = true;
	}
	
	protected override void createTriggerPlane() {
		base.createTriggerPlane();
		Object.Destroy(triggerPlane.GetComponent<CheckpointTriggerPlane>());
		triggerPlane.AddComponent(typeof(LandingZoneTriggerPlane));
	}
	
}
