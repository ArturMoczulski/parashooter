using System;
using System.Collections;
using UnityEngine;


public class CheckpointTriggerPlane : MonoBehaviour {
	
	public void OnTriggerEnter(Collider other ) {
		
		if( other.gameObject.tag == "Player" ) {
			
			LightsCircle checkpoint = gameObject.transform.parent.GetComponent<LightsCircle>();
			
			if( Vector3.Distance(checkpoint.gameObject.transform.position, other.gameObject.transform.position) > checkpoint.circleRadius )
				GameState.Instance.CurrentLevelCheckpointsMissed++;
			else
				GameState.Instance.CurrentLevelCheckpointsChecked++;
			checkpoint.gameObject.SetActive(false);
		}
		
	}


}


