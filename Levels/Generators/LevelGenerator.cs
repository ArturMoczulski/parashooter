using UnityEngine;
using System.Collections;

public abstract class LevelGenerator : object {
	
	public virtual GameObject generateLevel(int levelNumber) {
		return createLevelGameObject(levelNumber);
	}
	
	protected virtual GameObject createLevelGameObject(int levelNumber) {
		GameObject newLevelObject = new GameObject();
		newLevelObject.name = "Level "+levelNumber;
		newLevelObject.AddComponent(typeof(Level));
		return newLevelObject;
	}
	
}
