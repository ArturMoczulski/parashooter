using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public abstract class DebugPopUpWindow : KeyToggablePopUpWindow
{
	
	protected void Awake() {
		
		base.Awake();
		
		if( ((GameInitializer)FindObjectOfType(typeof(GameInitializer))).debug > 0 )
			gameObject.active = true;
		else {
			gameObject.active = false;
			return;
		}
		
	}
	
}


