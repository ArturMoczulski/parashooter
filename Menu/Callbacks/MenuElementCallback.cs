using UnityEngine;
using System.Collections;

public abstract class MenuElementCallback : object {
	
	public object parameter;
	
	public abstract void onChosen();
	
}
