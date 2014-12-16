using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenCenteredMenu : SequentionalMenu {
	
	public GUIStyle style;
	public Texture background;
	
	protected void Start() {
		Screen.orientation = ScreenOrientation.Landscape;
		createMenuElements();
	}
	
	protected void OnGUI() {
		
		drawBackground();
		drawMenu();		
		
	}
	
	protected virtual void drawBackground() {
		if( background != null )
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), background);
	}
	
	protected virtual void drawMenu() {
		
		GUIStyle mainMenuButtonsStyle = style;
		mainMenuButtonsStyle.fontSize = (int)(Screen.height * fontSize);
		mainMenuButtonsStyle.font = font;
		
		// Buttons
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.Space(Screen.height/4);
		
		foreach( GameObject menuElement in menuElements ) {
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
		
			if( GUILayout.Button(menuElement.name, mainMenuButtonsStyle) ) {
				menuElement.GetComponent<MenuElement>().onChosen();
			}
		
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		
		GUILayout.EndArea();
		
	}
	
	protected virtual void createMenuElements() {
		
		menuElements = new ArrayList();
		
		if (menuElementsLabels.Count == 0)
			return;
		
		foreach(string menuElementLabel in menuElementsLabels) {
			GameObject menuElement = new GameObject();
			menuElement.transform.parent = this.transform;
			menuElement.name = menuElementLabel;
			menuElement.transform.localPosition = new Vector3(0,0,0);
			
			menuElement.AddComponent(typeof(MenuElement));
						
			menuElements.Add(menuElement);
		}
		
	}
	
	protected void showSelected() {}
	protected override void positionMenuElements() {}
	
}

