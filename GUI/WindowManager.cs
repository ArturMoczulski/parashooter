using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class WindowManager {
	
	protected static WindowManager instance;
	protected Stack<int> recycledWindowIds;
	protected int lastUsedWindowId = 1;
	
	public static WindowManager Instance {
		get { 
			if( instance == null )
				instance = new WindowManager();
			return instance;
		}
	}
	
	public int registerWindow() {
		if( recycledWindowIds.Count > 0 )
			return recycledWindowIds.Pop();
		else {
			lastUsedWindowId++;
			return lastUsedWindowId;
		}
	}
	
	public void unregisterWindow(int id) {
		if( !recycledWindowIds.Contains(id) )
			recycledWindowIds.Push(id);
	}
	
	protected WindowManager() {
		recycledWindowIds = new Stack<int>();
	}
	
}
