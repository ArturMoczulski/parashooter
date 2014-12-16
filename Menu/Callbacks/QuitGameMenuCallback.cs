using UnityEngine;
using System.Collections;

public class QuitGameMenuCallback : MenuElementCallback {
	
	override public void onChosen() {
		if (((BoolClass)(parameter)).boolValue)
			Application.Quit();
	}
	
}
