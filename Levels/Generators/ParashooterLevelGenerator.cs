using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParashooterLevelGenerator : LevelGenerator {
	
	private ParashooterLevelsSettings levelsSettings;
	public ParashooterLevelsSettings LevelsSettings { get { return levelsSettings; } set { levelsSettings = value; } }
	
	public GameObject cloudsPrefab;
	
	void Awake() {
		levelsSettings = new ParashooterLevelsSettings();
	}

	public override GameObject generateLevel(int levelNumber) {
		
		// 1. Create the level game object
		GameObject newLevelObject = base.generateLevel(levelNumber);
		ParashooterLevel newLevel = (ParashooterLevel)newLevelObject.GetComponent<ParashooterLevel>();
		
		// 2. Generate terrain
		newLevel.TerrainObject = generateTerrain(newLevelObject, levelNumber);
		
		// 3. Calculate the level's starting X,Z position
		newLevel.PlayersStartingHorizontalPosition = calculatePlayersStartingHorizontalPosition(newLevel.TerrainObject.GetComponent<Terrain>());
		
		// 4. Create the player
		GameObject newPlayer = generatePlayer(levelNumber, newLevel);
		newLevel.Player = newPlayer;
		
		// 5. Generate checkpoints
		newLevel.Checkpoints = generateCheckpoints(newLevel,levelNumber);
		if( newLevel.Checkpoints.Count > 0 )
			newLevel.LandingZone = newLevel.Checkpoints[0];		
		
		return newLevelObject;
		
	}
	
	protected override GameObject createLevelGameObject(int levelNumber) {
		GameObject newLevelObject = base.createLevelGameObject(levelNumber);
		Object.Destroy(newLevelObject.GetComponent<ParashooterLevel>());
		ParashooterLevel newLevel = (ParashooterLevel)newLevelObject.AddComponent(typeof(ParashooterLevel));
		newLevel.LevelPassedCriteriaSet = levelsSettings.getLevelSettings(levelNumber).levelPassedCriteria.createSet();
		newLevel.JumpHeight = levelsSettings.getLevelSettings(levelNumber).jumpHeight;
		return newLevelObject;
	}
	
	public GameObject generateTerrain(GameObject newLevel, int levelNumber) {
		
		// Loading the terrain data on demand for performance
		TerrainData terrainDataPrefab = (TerrainData)Resources.Load (levelsSettings.getLevelSettings(levelNumber).terrainDataPath, typeof(TerrainData));
		
		if( terrainDataPrefab == null )
			throw new UnityEngine.UnityException("There is no terrain prefab set up for level "+levelNumber.ToString());
		
		// TerrainData terrainData = (TerrainData)Object.Instantiate(terrainDataPrefab);
		GameObject terrain = Terrain.CreateTerrainGameObject(terrainDataPrefab);
		terrain.name = terrainDataPrefab.name;
		if( newLevel != null ) terrain.transform.parent = newLevel.transform;
		terrain.transform.localPosition = new Vector3(-terrainDataPrefab.size.x/2,0,-terrainDataPrefab.size.z/2);
		
		return terrain;
		
	}
	
	protected Vector2 calculatePlayersStartingHorizontalPosition(Terrain terrain) {
		return new Vector2(
			0 + Random.Range(-terrain.terrainData.size.x/2,terrain.terrainData.size.x/2),
			0 + Random.Range(-terrain.terrainData.size.y/2,terrain.terrainData.size.y/2));
	}
	
	public List<GameObject> generateCheckpoints(ParashooterLevel newLevel, int levelNumber) {
		
		List<GameObject> checkpoints = new List<GameObject>();
		
		if( levelsSettings.getLevelSettings(levelNumber).checkpointsSettings.landingZonePrefab == null )
			throw new UnityException("Landing zone prefab not set for level " + levelNumber);
		
		if( levelsSettings.getLevelSettings(levelNumber).checkpointsSettings.checkpointPrefab == null )
			throw new UnityException("Checkpoint prefab not set for for level " + levelNumber);
		
		// calculate positions
		List<Vector3> checkpointsPositions = calculateCheckpointsPositions(newLevel, levelsSettings.getLevelSettings(levelNumber));
		
		// create the game objects
			
		int checkpointsNumber = levelsSettings.getLevelSettings(levelNumber).checkpointsSettings.checkpointsNumber; // perform the count check only once per generation
		for( int i = 0; i < checkpointsNumber; i++) {
			
			if( i >= checkpointsPositions.Count )
				throw new UnityException("Not enough checkpoints positions have been calculated.");
			else {
				
				GameObject checkpoint = null;
				string checkpointName = "Checkpoint "+(i);
				GameObject prefab = levelsSettings.getLevelSettings(levelNumber).checkpointsSettings.checkpointPrefab;
				Vector3 checkpointPosition = checkpointsPositions[i];
				
				if( i == 0 ) { // handling landing zone generation
					prefab = levelsSettings.getLevelSettings(levelNumber).checkpointsSettings.landingZonePrefab;
					checkpointName = "Landing Zone";
					// Don't flatten the ground for the landing zone as the prefab
					// takes into account uneven ground level.
					// flattenTerrainForLandingZone(prefab, checkpointPosition, newLevel.TerrainObject.GetComponent<Terrain>());
				}
					
				checkpoint = generateCheckpoint(prefab, checkpointPosition);
				
				if( checkpoint != null ) {
					checkpoint.transform.parent = newLevel.gameObject.transform;
					checkpoint.name = checkpointName;
					checkpoints.Add (checkpoint);
				} else
					throw new UnityException("Could not create checkpoint "+(i));
				
			}
			
		}
		
		return checkpoints;
	}
	
	protected List<Vector3> calculateCheckpointsPositions(ParashooterLevel newLevel, ParashooterLevelSettings levelSettings) {
		
		List<Vector3> positions = new List<Vector3>();
		
		// it's important to know the y increments, as the x,z distances between checkpoints depend on it
		float yIncrement = newLevel.JumpHeight / (float)levelSettings.checkpointsSettings.checkpointsNumber;
		
		PlayerController player = newLevel.Player.GetComponent<PlayerController>();
		
		/** 
		 * horizontals; positions are being generated in a ring-shaped space
		 * around the last checkpoint.
		 */
		Vector2 startingHorizontalPosition = newLevel.PlayersStartingHorizontalPosition;
		Vector3 lastCheckpointPosition = new Vector3(startingHorizontalPosition.x,0,startingHorizontalPosition.y);
		
		for( int i = 0; i < levelSettings.checkpointsSettings.checkpointsNumber; i++ ) {
			
			Vector3 checkpointPosition = lastCheckpointPosition;
			Vector2 spreadRange = levelSettings.checkpointsSettings.checkpointsSpreadRange;
			if( spreadRange.x > spreadRange.y ) throw new UnityException("Minimum spread range value must be less or equal to the maximum spread range value.");
			
			GameObject dummy = new GameObject();
			dummy.transform.position = lastCheckpointPosition;
			dummy.transform.Rotate(new Vector3(0,Random.Range(0f,360f),0));					
			
			/**
			 *
			 * to calculate maximum achievable spread we need to
			 * figure out how long it will take the player to
			 * get to the next checkpoint vertically; based on
			 * time and horizontal speed we are able to say
			 * what's the furthest achievable distance
			 * 
			 * in below air resistance is not considered
			 * 
			 * h = g*t^2 - 2*v0*t =>
			 * g*t^2 -2*v0*t -2*h = 0 =>
			 * (from quadratic equation) see below
			 *
			 */
			
			float g = -Physics.gravity.y;
			
			// time from the top to the currently generated checkpoint
			float hLeft = yIncrement * (levelSettings.checkpointsSettings.checkpointsNumber - i);
			float totalTime = (Mathf.Sqrt( -4.0f*g * (-2.0f*hLeft) ) ) / (2.0f*g);
			float v0 = g * totalTime > 50 ? 50 : g * totalTime; // max falling speed is 50 m/s
			
			// time between the last generated checkpoint 
			// and currently generated one
			
			float time = (2.0f*v0 + Mathf.Sqrt( Mathf.Pow((-2.0f*v0),2.0f) -4.0f*g * (-2.0f*yIncrement) ) ) / (2.0f*g); 
			
			float maxXZDistance =
					player.horizontalSpeed *
					time;
			
			float xzDistance = maxXZDistance; //Random.Range(spreadRange.x, spreadRange.y < maxXZDistance ? spreadRange.y : maxXZDistance);
			
			dummy.transform.Translate(Vector3.forward * xzDistance);
			checkpointPosition = dummy.transform.position;
			Object.Destroy(dummy);
			
			positions.Add(checkpointPosition);
			lastCheckpointPosition = checkpointPosition;
		}
		
		// verticals (heights)
		if( positions.Count > 0 ) {
			Terrain terrain = newLevel.TerrainObject.GetComponent<Terrain>();
			
			float landingZoneY = calculateLandingVerticalPosition(
				positions[0],
				levelSettings.checkpointsSettings.landingZonePrefab.GetComponent<LandingZone>(), 
				terrain);
			
			for( int i = positions.Count-1; i >= 0; i-- ) {
				Vector3 position = positions[i];
				position.y = landingZoneY + yIncrement*i;
				positions[i] = position;
			}
		}
		
		return positions;
	}
	
	protected float calculateLandingVerticalPosition(Vector3 horizontalPosition, LandingZone landingZone, Terrain terrain) {
		
		Vector3 prefabSize = new Vector3(landingZone.circleRadius*2,0,landingZone.circleRadius*2);
		
		float landingZoneY = terrain.SampleHeight(horizontalPosition);
		for( int x = 0; x <= prefabSize.x; x++ ) {
			for( int z = 0; z <= prefabSize.z; z++ ) {
				float sampleHeight = terrain.SampleHeight(new Vector3(
					horizontalPosition.x - prefabSize.x/2 + x,
					0,
					horizontalPosition.z - prefabSize.z/2 + z));
				if( landingZoneY < sampleHeight ) landingZoneY = sampleHeight;
			}
		}
		return landingZoneY+5;
	}
	
	public GameObject generatePlayer(int levelNumber, ParashooterLevel level){
		
		if( levelsSettings.getLevelSettings(levelNumber).playerPrefab == null )
			throw new UnityException("Player prefab not set up for level "+levelNumber);
		
		GameObject player = (GameObject)UnityEngine.Object.Instantiate(levelsSettings.getLevelSettings(levelNumber).playerPrefab);
		player.name = "Player";
		player.transform.position = calculatePlayersPosition(levelNumber, level);
		return player;
	}
	
	protected Vector3 calculatePlayersPosition(int levelNumber, ParashooterLevel newLevel) {
		
		if (levelsSettings.getLevelSettings(levelNumber).playerPrefab == null)
			throw new UnityException("PlayerPrefab  is not linked with the level generator.");
		
		// horizontal
		Vector3 playersPosition = new Vector3( 
			newLevel.PlayersStartingHorizontalPosition.x,
			0,
			newLevel.PlayersStartingHorizontalPosition.y);
					
		// vertical
		Terrain terrain = (Terrain)newLevel.TerrainObject.GetComponent<Terrain>();
		
		playersPosition.y = newLevel.LandingZone != null ? 
			newLevel.LandingZone.transform.position.y :
			terrain.SampleHeight(playersPosition);
		playersPosition.y += newLevel.JumpHeight;
		
		return playersPosition;
		
	}
	
	public GameObject generateCheckpoint(GameObject prefab, Vector3 position) {
		if (prefab == null) throw new UnityException("Checkpoint prefab not set");
		GameObject checkpoint = (GameObject)UnityEngine.Object.Instantiate(prefab);
		checkpoint.transform.position = position;
		checkpoint.name = "Checkpoint";
		return checkpoint;
	}
	
	protected void flattenTerrainForLandingZone(
			GameObject landingZonePrefab,
			Vector3 landingZonePosition,
			Terrain terrain
		) {
		
		// flatten the ground for the landing zone
		Vector2 size = new Vector2(
			Mathf.Ceil(landingZonePrefab.GetComponent<LandingZone>().circleRadius / terrain.terrainData.size.x),
			Mathf.Ceil(landingZonePrefab.GetComponent<LandingZone>().circleRadius / terrain.terrainData.size.z)
		);
		size.x = size.x>4 ? size.x : 4;
		size.y = size.y>4 ? size.y : 4;
		// TODO: the minimum size should be really dependant on the heightmap resolution
				
		Vector3 flattenPosition = new Vector3(
			(landingZonePosition.x - terrain.gameObject.transform.position.x) / terrain.terrainData.size.x,
			(landingZonePosition.y - terrain.gameObject.transform.position.y) / terrain.terrainData.size.y,
			(landingZonePosition.z - terrain.gameObject.transform.position.z) / terrain.terrainData.size.z
		);
				
		flattenPosition.x *= terrain.terrainData.heightmapWidth;
		flattenPosition.z *= terrain.terrainData.heightmapHeight;
				
		float[,] heightsToFlatten = terrain.terrainData.GetHeights(
			(int)(flattenPosition.x - size.x/2),
			(int)(flattenPosition.z - size.y/2),
			(int)size.x, (int)size.y
		);
				
		for(int x=0;x<size.x;x++) {
			for(int y=0;y<size.y;y++) {
				heightsToFlatten[x,y] = flattenPosition.y;
			}
		}
				
		terrain.terrainData.SetHeights(
			(int)(flattenPosition.x-size.x/2),
			(int)(flattenPosition.z-size.y/2),
			heightsToFlatten
		);
		
	}
	
}
