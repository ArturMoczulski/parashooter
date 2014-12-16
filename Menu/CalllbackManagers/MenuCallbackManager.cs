using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class MenuCallbackManager : MonoBehaviour {
	
	public string callbackName;
	
	protected List<object> callbackParameters;
	
	public MenuCallbackManager() {
		callbackParameters = new List<object>();
	}
	
	void Start () {
		int i = 0;
		ArrayList menuElements = this.gameObject.GetComponent<SequentionalMenu>().getMenuElements();
		if( callbackParameters.Count != menuElements.Count ) {
			throw new UnityException("Callback parameters not set up for all menu elements or invalid script execution order set (the menu's start method has to be run prior to callback manager's start method).");
		}
		
		foreach(object menuElementCallbackParameter in this.callbackParameters) {
			
			if ( Type.GetType(callbackName) == null || 
				 i >= menuElements.Count ) {
				continue;
			}
		
			MenuElementCallback menuElementCallback = (MenuElementCallback)System.Activator.CreateInstance(System.Type.GetType(callbackName));
			menuElementCallback.parameter = menuElementCallbackParameter;
			GameObject menuElement = (GameObject)menuElements[i];
			menuElement.GetComponent<MenuElement>().addCallbackComponent(menuElementCallback);
			
			i++;
		}
	}
	
	public List<object> getCallbackParameters() {
		return callbackParameters;
	}
	
	public void Reset() {
		callbackName = this.GetType().Name.Replace("MenuCallbackManager","")+"MenuCallback";
	}
	
}
