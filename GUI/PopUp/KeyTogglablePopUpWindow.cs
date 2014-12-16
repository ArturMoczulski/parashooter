using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public abstract class KeyToggablePopUpWindow : PopUpWindow 
{
	
	public KeyCode toggleKey;
	
	protected void Update() {
		if( Input.GetKeyDown(toggleKey) ) {
			show = !show;
		}
	}
	
	protected void Awake() {
		base.Awake();
		validateSetUp();
	}
	
	protected bool validateSetUp() {
		bool result = true;
		
		if( toggleKey == null ) {
			result = false;
			throw new UnityException("Toggle key has not been set up");
		}
		
		return result;
	}
	
}


