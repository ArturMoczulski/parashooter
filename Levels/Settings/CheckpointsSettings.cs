using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CheckpointsSettings : LevelSettings {
	
	public GameObject checkpointPrefab;
	public GameObject landingZonePrefab;
	
	public int checkpointsNumber;
	public Vector2 checkpointsSpreadRange;
	
}
