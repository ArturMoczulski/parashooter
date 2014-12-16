using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ParashooterLevelSettings : LevelSettings {
	
	public string name;
	public string country;
	public string continent;
	public string terrainDataPath;
	public GameObject playerPrefab;
	public GameStateCriteriaSetTemplate levelPassedCriteria;
	/**
	 * The height of the jump. This is exactly the vertical distance between
	 * the player and the landing zone.
	 */
	public float jumpHeight;
	public CheckpointsSettings checkpointsSettings;
	public Texture mapPreview;
	
}
