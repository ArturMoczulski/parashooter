using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public abstract class GUIManager : MonoBehaviour {
	
	public GUISkin skin;
	
	public void Awake() {
		guiLayers = new List<GameObject>();
		// initialize();
	}
	
	public void Reset() {
		initialize();
	}
	
	public void OnGUI() {
		renderGUI();
	}

	public static GUIManager Instance {
	
		get {
		
			if (instance == null) {
				instance = (GUIManager)UnityEngine.Object.FindObjectOfType(MethodBase.GetCurrentMethod().DeclaringType);
			}	
			return instance;	
		
		}
				
	}
	
	public void initialize() {
		guiLayers = new List<GameObject>();
		
		foreach(GameObject guiLayer in guiLayers) {
			DestroyImmediate(guiLayer);
		}		
		
		AddLayer();
	}
	
	public void renderGUI() {
		
		foreach(GameObject guiLayer in guiLayers) {
			GUIPart guiPart = guiLayer.GetComponent<GUIPart>();
			if(guiPart)
				guiPart.render();
		}
		
	}
	
	public void AddLayer() {
		GameObject guiLayer = new GameObject();
		guiLayer.transform.parent = transform;
		guiLayer.name = "GUI Layer #" + (transform.childCount);
		guiLayer.AddComponent(typeof(GUIPart));
		guiLayer.GetComponent<GUIPart>().Position = new Vector2(0,0);
		guiLayer.GetComponent<GUIPart>().Visible = true;
		
		guiLayers.Add(guiLayer);
	}
	
	protected static GUIManager instance;
	protected List<GameObject> guiLayers;
	
}
