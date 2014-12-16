using UnityEngine;
using System.Collections;

public class ParatrooperMissionSelectionMenuNew : ScreenCenteredMenu {
	
	public GameObject terrainContainer;
	
	public Texture mapPreviewBG;
	protected Texture mapPreview;
	public GUIStyle mapPreviewStyle;
	
	public GUIText continentNameText;
	public GUIText countryNameText;
	public GUIText missionNumberText;
	
	public void Start() {
		
		base.Start();
		GameState.Instance.SelectedLevelNumber = 1;
		UpdateMenu();
		
	}
	
	protected override void drawMenu() {
		
		GUIStyle mainMenuButtonsStyle = new GUIStyle(style);
		mainMenuButtonsStyle.fontSize = (int)(Screen.height * fontSize);
		mainMenuButtonsStyle.font = font;
		
		// Buttons
		GUILayout.BeginArea(new Rect(0, (float)(Screen.height - Screen.height/5.3), Screen.width, Screen.height));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		
		foreach( GameObject menuElement in menuElements ) {
		
			GUIStyle buttonStyle = new GUIStyle(mainMenuButtonsStyle);
			
			if( menuElement.name == "<" || menuElement.name == ">" ) {
				buttonStyle.padding.right = buttonStyle.padding.left = Screen.width/12;
			}
			
			if( GUILayout.Button(menuElement.name, buttonStyle) ) {
				menuElement.GetComponent<MenuElement>().onChosen();
			}
		
		}
		
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		
		// Map
		GUI.Box(new Rect(Screen.width/4 * 3 - mapPreview.width/2,Screen.height/17,mapPreview.width,mapPreview.height), mapPreview, mapPreviewStyle);
		
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
		
		// Unloading the terrain data from the previous mission preview for performance
		Resources.UnloadUnusedAssets();
		
	}
	
	protected void UpdateMapPreview() {
		
		try {
			mapPreview = ((ParashooterLevelManager)ParashooterLevelManager.Instance).levelsSettings.getLevelSettings(GameState.Instance.SelectedLevelNumber).mapPreview;
		} catch( UnityException exception ) {
			mapPreview = null;
		}
			
	}
	
}
