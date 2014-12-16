using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementScreen : MonoBehaviour {
	
	public Vector3 iconSize;
	public Vector2 arrayDimensions;
	public Vector2 spaceBetweenIcons;
	public Vector3 spaceBetweenArrays;
	public Font font;
	public int fontSize;
	public Material material;
	public Mesh mesh;
	
	public KeyCode previousKey = KeyCode.LeftArrow;
	public KeyCode nextKey = KeyCode.RightArrow;
	
	private List<GameObject> achievementElements;
	private int currentlySelected = 0;
	
	public int CurrentlySelected {
		get { return currentlySelected; }
		set {
//			achievementElements[currentlySelected].transform.localScale = Vector3.one;
			currentlySelected = value;
//			achievementElements[currentlySelected].transform.localScale = new Vector3(1.5f,1.5f,1f);
		}
	}
	
	

	void Start () {

		currentlySelected = 0;
		createAchievementElements();
		positionAchievementElements();
		
	}
	
	void Update() {
		handleUserInput();
	}
	
	protected void createAchievementElements()
	{
		
		achievementElements = new List<GameObject>();
		
		if (AchievementManager.Instance.GetAchievements.Length == 0)
			return;
		
		foreach(Achievement achievement in AchievementManager.Instance.GetAchievements)
		{
			GameObject achievementElement = new GameObject();
			achievementElement.transform.parent = this.transform;
			achievementElement.name = achievement.Name + " AchievementElement";
			achievementElement.transform.localPosition = Vector3.zero;
			
			achievementElement.AddComponent(typeof(MeshRenderer));
			achievementElement.AddComponent(typeof(MeshFilter));
			achievementElement.GetComponent<MeshFilter>().mesh = mesh;
			achievementElement.transform.Rotate(new Vector3());
			achievementElement.GetComponent<MeshFilter>().renderer.material = achievement.GetIcon;
			achievementElement.transform.localScale = iconSize;
			
			achievementElements.Add(achievementElement);

		}
	}
	
	protected void positionAchievementElements()
	{
		int arraySize = Mathf.CeilToInt(arrayDimensions.x * arrayDimensions.y);
		int arraysCount = Mathf.CeilToInt(achievementElements.Count / arraySize);
		
		int x = 0,y = 0,t = 0;
		
		foreach( GameObject achievement in achievementElements ) {
			
			Vector3 translation = new Vector3(
				x * spaceBetweenIcons.x + spaceBetweenArrays.x * t,
				0,
				(y * spaceBetweenIcons.y + spaceBetweenArrays.y * t) * (-1)
			);
			translation += spaceBetweenArrays * t;
			
			achievement.transform.Translate(translation);
			
			x++;
			
			if( x >= arrayDimensions.x ) {
				x = 0;
				y++;
			}
			
			if( y >= arrayDimensions.y ) {
				y = 0;
				t++;
			}
			
		}
		
		
		
	}
	
	protected void handleUserInput()
	{
			if (Input.GetKeyDown(previousKey))
				CurrentlySelected--;
			if (Input.GetKeyDown(nextKey))
				CurrentlySelected++;
			
			if (CurrentlySelected < 0)
				CurrentlySelected = achievementElements.Count-1;
			
			if (CurrentlySelected >= achievementElements.Count)
				CurrentlySelected = 0;
	}
		
}
