using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LevelManagerWithGenerator : LevelManager {
	
	protected GameObject currentLevel;
	protected LevelGenerator levelGenerator;
	public string levelGeneratorTypeName;
	
	public LevelGenerator LevelGenerator { get { return this.levelGenerator; } }
	
	public GameObject CurrentLevel{ get {return currentLevel;}}
	public void Awake() {
		
		if( levelGeneratorTypeName != null )
			levelGenerator = (LevelGenerator)System.Activator.CreateInstance(Type.GetType(levelGeneratorTypeName));
		
	}
	
	public override void onLevelChanged(LevelChangedEvent ev) { 
		reloadCurrentLevel(); 
	}
	
	public override void reloadCurrentLevel() {
	
		if (currentLevel != null)
			Destroy(currentLevel);
		
		if (levelGenerator != null) {
			currentLevel = levelGenerator.generateLevel(GameState.Instance.CurrentLevelNumber);
			// Unloading the terrain data from the previous mission preview for performance
			Resources.UnloadUnusedAssets();
		} else
			throw new UnityException("No level generator set up.");
		
	}
	
	public static List<Type> GetAvailableGenerators() {
		List<Type> availableGenerators = new List<Type>();
		
		foreach(Type subclass in Assembly.GetAssembly(typeof(LevelGenerator)).GetTypes().Where(
			t => t.IsSubclassOf(typeof(LevelGenerator))
			)) {
			availableGenerators.Add (subclass);
		}
		
		return availableGenerators;
		
	}
	
}
