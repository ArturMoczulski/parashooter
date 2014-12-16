using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GUIPart : MonoBehaviour {
	
	void Awake() {
		position = new Vector2(0,0);
	}
	
	public void render() {
		
		if (!visible)
			return;
		
		foreach(GameObject guiPart in transform) {
			guiPart.GetComponent<GUIPart>().render();
		}
		
	}
	
	public bool Visible { get { return visible; } set { visible = value; } }
	public Vector2 Position { get { return position; } set { position = value; } }
	
	protected bool visible = true;
	protected Vector2 position;

}
