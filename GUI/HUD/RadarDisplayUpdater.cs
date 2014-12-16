using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarDisplayUpdater : GameStateEventHandler {
 
	public Texture blipTexture;
	public Texture nextBlipDirectionTexture;
	public Texture playerBlipTexture;
	public Texture radarBGTexture;
 
	private GameObject centerObject;
	public List<string> tagFilters;
	public float mapScale = 0.3f;
	public float horizontalRange = 2000;
	public float verticalRange = 100;
	
	public Vector2 displayGUIPosition = new Vector2(50,50);
	public float radiusGUI = 64;
	public float backgroundSizeGUI = 64;
	public int blipSizeGUI = 10;
	public int playerBlipSizeGUI = 15;
	
	void Reset() {
		tagFilters.Add("Checkpoint");
		tagFilters.Add("LandingZone");
	}
	
	public override void onLevelStarted(LevelStartedEvent gsEvent) {
		if( ((ParashooterLevelManager)LevelManager.Instance).CurrentLevel == null )
			throw new UnityException("Level has not been generated yet. Level generation sequence problem.");
		centerObject = ((ParashooterLevelManager)LevelManager.Instance).CurrentLevel.GetComponent<ParashooterLevel>().Player;
		if( centerObject == null )
			throw new UnityException("Player object not found.");
	}
 
	void OnGUI() 
	{
		
	 	GUI.DrawTexture(new Rect(displayGUIPosition.x-backgroundSizeGUI,displayGUIPosition.y-backgroundSizeGUI,backgroundSizeGUI*2,backgroundSizeGUI*2),radarBGTexture);
		
		foreach( string tagFilter in tagFilters) {
			DrawBlipsFor(tagFilter);
		}
		
		drawBlip(centerObject, playerBlipTexture, playerBlipSizeGUI);
 
	}
 
	private void DrawBlipsFor(string tagName)
	{
 
	    GameObject[] objects = GameObject.FindGameObjectsWithTag(tagName); 
 
	    foreach (GameObject gameObject in objects)  
	    { 
			drawBlip(gameObject,blipTexture,blipSizeGUI, nextBlipDirectionTexture);
	    }
	}
 
	private void drawBlip(GameObject gameObject,Texture texture, float textureSize, Texture nextBlipDirectionTexture = null)
	{
		
		if( centerObject == null )
			throw new UnityException("Player object not found.");
		
		Vector3 centerPosition = centerObject.transform.position;
		Vector3 objectPosition = gameObject.transform.position;
		Vector3 difference = centerPosition - objectPosition;
		
		float horizontalDistance=Vector2.Distance(
			new Vector2(centerPosition.x, centerPosition.z),
			new Vector2(objectPosition.x, objectPosition.z)
		);
		
		float verticalDistance = centerPosition.y - objectPosition.y;
		
		Vector2 radarDifference = new Vector2();
		radarDifference.x = -difference.x * mapScale;
		radarDifference.y = difference.z * mapScale;
		
		Vector2 objectRadarPosition = displayGUIPosition + radarDifference;
		
		if( horizontalDistance <= horizontalRange && 
			verticalDistance >= 0 &&
			Vector3.Distance(Vector3.zero,radarDifference) < radiusGUI )
		{
			// The checkpoint is in the range of the radar; draw the blip
			
			float blipSize = textureSize - verticalDistance / verticalRange;
			blipSize = blipSize < 0 ? 0 : blipSize;
			
			Rect blip = new Rect(
				objectRadarPosition.x - blipSize/2,
				objectRadarPosition.y - blipSize/2,
				blipSize,
				blipSize);
			
			GUI.DrawTexture(blip,texture);
			
		} else {
			// The checkpoint is outside of the radar; draw the arrow
			
			if( nextBlipDirectionTexture != null ) {
				
				Vector2 horizontalDifference = (
					(new Vector2(centerPosition.x, centerPosition.z)) -
					(new Vector2(objectPosition.x, objectPosition.z))).normalized;
			
				float directionAngle = Vector2.Angle(Vector2.right, horizontalDifference);
			
				if( Vector3.Cross(Vector2.right, horizontalDifference).z < 0 ) 
					directionAngle = 360 - directionAngle;
			
				Vector2 directionIndicatorPosition = new Vector2(
					displayGUIPosition.x + radiusGUI * Mathf.Sin((270 + directionAngle) * Mathf.Deg2Rad),
					displayGUIPosition.y + radiusGUI * Mathf.Cos((270 + directionAngle) * Mathf.Deg2Rad));
				
				Rect indicator = new Rect(
					directionIndicatorPosition.x - nextBlipDirectionTexture.width/2,
					directionIndicatorPosition.y - nextBlipDirectionTexture.height/2,
					nextBlipDirectionTexture.width,
					nextBlipDirectionTexture.height);
				
				Matrix4x4 tempMatrix = GUI.matrix;
				GUIUtility.RotateAroundPivot(270 - directionAngle, new Vector2(indicator.x + indicator.width/2, indicator.y + indicator.height/2));
				GUI.DrawTexture(indicator, nextBlipDirectionTexture);
				GUI.matrix = tempMatrix;			
				
			}
			
		}
 
	}	
	
}
