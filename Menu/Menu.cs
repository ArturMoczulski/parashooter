using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	public string rendererClassName;
	public MenuData data;
	public GUISkin style;
	
	void OnGUI() {
		renderer.render(data, style);
	}
	
	void Awake() {
		if( rendererClassName == "" || System.Type.GetType(rendererClassName) == null ) {
			throw new UnityException("No menu renderer set.");
		}
		renderer = (MenuRenderer)System.Activator.CreateInstance(System.Type.GetType(rendererClassName));
	}
	
	protected MenuRenderer renderer;
	
}
