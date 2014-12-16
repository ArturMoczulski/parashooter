using System;
using UnityEngine;
using System.Reflection;

public class ParashooterLevelDebugPopUp : ObjectDebugPopUp
{
	
	protected override string[] getIngoredPropertiesNames() {
		return new string[] {
			"LandingZone",
			"Checkpoints"
		};
	}
	
	protected override object setObjectUnderDebug() {
		if( ((ParashooterLevelManager)ParashooterLevelManager.Instance).CurrentLevel != null )
			return ((ParashooterLevelManager)ParashooterLevelManager.Instance).CurrentLevel.GetComponent<ParashooterLevel>();
		else 
			return null;
	}
	
	public override void onLevelStarted(LevelStartedEvent gsEvent) {
		base.onLevelStarted(gsEvent);
		objectUnderDebug = setObjectUnderDebug();
		if( objectUnderDebug != null )
			title = objectUnderDebug.GetType().Name + " Debug";
	}
	
	protected override PropertyInfo[] getPropertiesToDisplay (Type limitingType = null) {
		return base.getPropertiesToDisplay(objectUnderDebug.GetType());
	}
	
	
}


