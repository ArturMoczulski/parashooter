using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParatrooperMainMenu : ListMenu {
	
	public List<GameObject> menuElementsPrefabs;
	public List<Vector3> menuElementsPositions;
	
	public Texture background;
	
	public float notSelectedScale = 1.0f;
	public float selectedSizeScale = 1.5f;
	
	protected override void createMenuElements() {
		
		Screen.orientation = ScreenOrientation.Landscape;
		
		menuElements = new ArrayList();
		
		if (menuElementsPrefabs.Count == 0)
			return;
		
		foreach(GameObject menuElementPrefab in menuElementsPrefabs) {
			GameObject menuElement = (GameObject)Object.Instantiate(menuElementPrefab);
			menuElement.transform.parent = this.transform;
			
			menuElement.name = menuElementPrefab.name + " Menu Element";
			menuElement.transform.localPosition = new Vector3(0,0,0);
			menuElement.AddComponent(typeof(MenuElement));
			menuElements.Add(menuElement);
		}
		
		positionMenuElements();
		
	}
	
	protected override void positionMenuElements ()
	{
		
		int i = 0;
		
		foreach (GameObject menuElement in menuElements)
		{
			if( i < menuElementsPositions.Count ) {
				menuElement.transform.Translate(menuElementsPositions[i], Space.World);
				i++;			
			}
		}
	
	}
	
	protected override void showSelected() {
		
		foreach (GameObject menuElement in menuElements)
		{
			
			if (menuElement == menuElements[currentlySelected] ) {
				menuElement.gameObject.transform.localScale = new Vector3(
					0.001f * selectedSizeScale,
					0.001f * selectedSizeScale,
					0.001f * selectedSizeScale);
			} else {
				menuElement.gameObject.transform.localScale = new Vector3(
					0.001f * notSelectedScale,
					0.001f * notSelectedScale,
					0.001f * notSelectedScale);
			}
			
		}
		
	}
	
}
