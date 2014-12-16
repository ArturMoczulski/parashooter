using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {
	
	protected GameStateCriteriaSet levelPassedCriteriaSet;
	protected GameObject player;
	protected GameObject terrainObject;
	
	void Awake() {
		GameStateCriteriaSet levelPassedCriteriaSet = new GameStateCriteriaSet();
	}
	
	public GameStateCriteriaSet LevelPassedCriteriaSet { get { return levelPassedCriteriaSet; } set { levelPassedCriteriaSet = value; } }
	
	public GameObject Player { get
		{
		
			return player;
		
		} 
		set {
		
			player = value; 
			player.transform.parent = gameObject.transform;
		
		}
	}
	
	public GameObject TerrainObject { get { return terrainObject; } set { terrainObject = value; } }
	
		
	
}
