using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public abstract class PopUpWindow : GameStateEventHandler
{
	
	public string title = "";
	
	protected bool show = false;
	protected Rect window = new Rect(0,0,0,0);
	protected int windowId;
	
	protected void Awake() {
		
		window = new Rect(
					Screen.width/2 - Screen.width/4, 
					Screen.height/2 - Screen.width/4,
					Screen.width/2,Screen.width/2);
		
	}
	
	protected void OnGUI() {
		if( show ) {
			if( windowId == 0 )
				windowId  = WindowManager.Instance.registerWindow();
			window = GUILayout.Window (
				windowId, 
				window, 
				drawWindow, 
				title);
		} else {
			if( windowId != 0 ) {
				WindowManager.Instance.unregisterWindow(windowId);
				windowId = 0;
			}
		}
	}
	
	protected abstract void drawWindow(int windowId);
	
}


