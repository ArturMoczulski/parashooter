using UnityEngine;
using System.Collections;

public class SceneChangerMenuCallback : MenuElementCallback {
	
	override public void onChosen() {
		if (parameter != null) {
			StringClass sceneName = (StringClass)parameter;
			Application.LoadLevel(sceneName.stringValue);
		}
	}
	
}
