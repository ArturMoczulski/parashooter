using UnityEngine;
using System.Collections;
using System.Reflection;

public class HUDElementRelativePositioner : MonoBehaviour {
	
	public Vector2 position;
	
	void Start () {
		
		if (position.x > 1 || position.y > 1)
			throw new UnityException("HUD element's position is out of the screen.");
		
		Vector2 newPixelOffset = new Vector2();
		newPixelOffset.x = position.x * Screen.width;
		newPixelOffset.y = position.y * Screen.height;
		
		if( this.GetComponent<GUIText>() ) {
			newPixelOffset.x = position.x * Screen.width - Screen.width/2;
			newPixelOffset.y = position.y * Screen.height - Screen.height/2;
			guiText.pixelOffset = newPixelOffset;
		} else if( this.GetComponent<RadarDisplayUpdater>() ) {
			this.GetComponent<RadarDisplayUpdater>().displayGUIPosition = newPixelOffset;
		} else if( this.GetComponent<RollDisplayUpdater>() ) {
			this.GetComponent<RollDisplayUpdater>().displayGUIPosition = newPixelOffset;
		}
		
	}
	
}
