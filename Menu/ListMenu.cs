using UnityEngine;
using System.Collections;

public class ListMenu : SequentionalMenu {
	
	public float step;
	public float notSelectedCharacterSize;
	public float selectedCharacterSize;
	
	void Start () {
		
		createMenuElements();
	
	}
	
	void Update () {
		base.Update();
		showSelected();
	}
	
	protected override void createMenuElements()
	{
		
		base.createMenuElements();
		
		foreach (GameObject menuElement in menuElements)
		{
			menuElement.GetComponent<TextMesh>().characterSize = notSelectedCharacterSize;			
		}
		
	}
	
	protected override void positionMenuElements ()
	{
		
		int i = 0;
		
		foreach (GameObject menuElement in menuElements)
		{
			menuElement.transform.Translate((Vector3.down * i)*step);
			i++;			
		}
	
	}
	
	protected virtual void showSelected()
	{
		
		
		foreach (GameObject menuElement in menuElements)
		{
			
			if (menuElement == menuElements[currentlySelected] )
			{
				menuElement.GetComponent<TextMesh>().characterSize = selectedCharacterSize;
			}
			else menuElement.GetComponent<TextMesh>().characterSize = notSelectedCharacterSize;
			
		}
		
	}
}
