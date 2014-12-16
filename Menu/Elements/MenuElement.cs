using UnityEngine;
using System.Collections;

public class MenuElement : MonoBehaviour {
	
	private ArrayList callbackComponents;
	
	public MenuElement() {
		callbackComponents = new ArrayList();
	}
	
	public void addCallbackComponent (MenuElementCallback component) {
		callbackComponents.Add(component);
	}
	
	public void onChosen () {
		foreach(MenuElementCallback callbackComponent in callbackComponents) {
			callbackComponent.onChosen();
		}
	}
	
}
