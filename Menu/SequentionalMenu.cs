using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SequentionalMenu : GameStateEventHandler {
	
	public List<string> menuElementsLabels;
	
	public KeyCode previousKey = KeyCode.LeftArrow;
	public KeyCode nextKey = KeyCode.RightArrow;
	public KeyCode choiceKey = KeyCode.Return;
	
	public Font font;
	public float fontSize = 150;
	public Material material;

	protected int currentlySelected;
	protected ArrayList menuElements;
	
	public void Reset() {
		font = (Font)Font.FindObjectOfType(typeof(Font));
	}
	
	public ArrayList getMenuElements() {
		return this.menuElements;
	}
	
	protected void Awake() {
		currentlySelected = 0;
		menuElements = new ArrayList();
	}
	
	protected void Update() {
		handleUserInput();
	}
	
	protected virtual void createMenuElements() {
		
		menuElements = new ArrayList();
		
		if (menuElementsLabels.Count == 0)
			return;
		
		foreach(string menuElementLabel in menuElementsLabels) {
			GameObject menuElement = new GameObject();
			menuElement.transform.parent = this.transform;
			menuElement.name = menuElementLabel + " Menu Element";
			menuElement.transform.localPosition = new Vector3(0,0,0);
			
			menuElement.AddComponent(typeof(TextMesh));
			menuElement.GetComponent<TextMesh>().text = menuElementLabel;
			menuElement.GetComponent<TextMesh>().fontSize = (int)fontSize;
			menuElement.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
			menuElement.GetComponent<TextMesh>().font = font;
			
			menuElement.AddComponent(typeof(MeshRenderer));
			menuElement.renderer.material = material;
			
			menuElement.AddComponent(typeof(MenuElement));
						
			menuElements.Add(menuElement);
		}
		
		positionMenuElements();
	}
	
	protected void handleUserInput() {
		if (Input.GetKeyDown(previousKey))
			currentlySelected--;
		if (Input.GetKeyDown(nextKey))
			currentlySelected++;
		
		if (currentlySelected < 0)
			currentlySelected = menuElements.Count-1;
		
		if (currentlySelected >= menuElements.Count)
			currentlySelected = 0;
		if (Input.GetKeyDown(choiceKey)) {
			
			if (currentlySelected  >= menuElements.Count) 
				return;
			
			GameObject selectedMenuElement = (GameObject)menuElements[currentlySelected];
			
			selectedMenuElement.GetComponent<MenuElement>().onChosen();
			
		}
	}
	
	protected abstract void positionMenuElements();

}
