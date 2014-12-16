using UnityEngine;
using System.Collections;

public class ParatrooperMissionSelectionMenu : ParatrooperMainMenu {
	public GameObject terrainContainer;
	public GUITexture mapPreview;
	public Texture defaultMapPreviewTexture;
	public GUIText continentNameText;
	public GUIText countryNameText;
	public GUIText missionNumberText;
	
	public void Start() {
		
		createMenuElements();
		GameState.Instance.SelectedLevelNumber = 1;
		UpdateMenu();
		
	}
	
	public void onSelectedMissionChanged(SelectedMissionChangedEvent gsEvent) {
		UpdateMenu();
	}
	
	protected void UpdateMenu() {
		UpdateBackgroundTerrain();	
		UpdateMapPreview();
		UpdateMissionTitle();
	}
	
	protected void UpdateMissionTitle() {
		int selectedMission = GameState.Instance.SelectedLevelNumber;
		string continent = ((ParashooterLevelManager)ParashooterLevelManager.Instance).levelsSettings.getLevelSettings(selectedMission).continent;
		string country = ((ParashooterLevelManager)ParashooterLevelManager.Instance).levelsSettings.getLevelSettings(selectedMission).country;
		string mission = ((ParashooterLevelManager)ParashooterLevelManager.Instance).levelsSettings.getLevelSettings(selectedMission).name;
		
		continentNameText.text = continent;
		countryNameText.text = country;
		missionNumberText.text = mission;
	}
	
	protected void UpdateBackgroundTerrain() {
		
		if( terrainContainer == null )
			throw new UnityException("Terrain container not set.");
		
		ParashooterLevelGenerator levelGenerator = (ParashooterLevelGenerator)(((ParashooterLevelManager)ParashooterLevelManager.Instance).LevelGenerator);
		
		Destroy(Terrain.activeTerrain.gameObject);
		
		Terrain terrain = levelGenerator.generateTerrain(terrainContainer, GameState.Instance.SelectedLevelNumber).GetComponent<Terrain>();
		
		SplatPrototype[] splats = new SplatPrototype[0];
		terrain.terrainData.splatPrototypes = splats;
		
	}
	
	protected void UpdateMapPreview() {
		
		Texture mapPreviewTexture = new Texture();
		
		try {
			mapPreviewTexture = ((ParashooterLevelManager)ParashooterLevelManager.Instance).levelsSettings.getLevelSettings(GameState.Instance.SelectedLevelNumber).mapPreview;
		} catch( UnityException exception ) {
			mapPreviewTexture = defaultMapPreviewTexture;
		}
		
		if( mapPreviewTexture == null )
			mapPreviewTexture = defaultMapPreviewTexture;
		
		mapPreview.texture = mapPreviewTexture;
			
	}
	
}
