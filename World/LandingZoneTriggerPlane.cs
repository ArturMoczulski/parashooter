using System;
using System.Collections;
using UnityEngine;


public class LandingZoneTriggerPlane : CheckpointTriggerPlane {
	
	public void OnTriggerEnter(Collider other ) {
		
		if( other.gameObject.tag == "Player" ) {
			LandingZone checkpoint = gameObject.transform.parent.GetComponent<LandingZone>();
			if( Vector3.Distance(checkpoint.gameObject.transform.position, other.gameObject.transform.position) > checkpoint.circleRadius )
				GameState.Instance.CurrentLevelCheckpointsMissed++;
			else {
				GameState.Instance.CurrentLevelCheckpointsChecked++;
				GameState.Instance.CurrentLevelLandingZoneHit = true;
			}
		}
		
	}


}


