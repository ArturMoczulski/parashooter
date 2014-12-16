using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightsCircle : Checkpoint {
	
	public int lightsNumber;
	public float circleRadius;
	public GameObject lightPrefab;
	
	protected ArrayList lights;
	protected GameObject triggerPlane;
	
	void Awake() {
		lights = new ArrayList();
	}
	
	void Start() {
		createLights();
		createTriggerPlane();
	}
	
	protected void createLights() {
		
		lights = new ArrayList();
		
		for(int i = 0; i < lightsNumber; i++) {
			GameObject light = (GameObject)Instantiate(lightPrefab);
			light.transform.parent = this.transform;
			light.name = "Circle Light";
			light.transform.localPosition = new Vector3(0,0,0);
			lights.Add(light);
			
		}
		
		positionLights();
	}
	
	protected virtual void positionLights() {
		
		float distributionAngle = 360 / lights.Count;
		
		int i=0;
		foreach(GameObject light in lights) {
			Vector3 rotation = new Vector3(0,0,0);
			
			rotation.y = -distributionAngle*i;
			light.transform.Rotate(rotation);
			
			light.transform.Translate(Vector3.back*circleRadius);
			i++;
		}
		
	}
	
	protected virtual void createTriggerPlane() {
		triggerPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		triggerPlane.transform.parent = this.transform;
		triggerPlane.name = "TriggerPlane";
		
		triggerPlane.transform.position = Terrain.activeTerrain.transform.position;
		
		triggerPlane.transform.localScale = new Vector3(
			Terrain.activeTerrain.terrainData.size.x/10,
			1f,
			Terrain.activeTerrain.terrainData.size.z/10);
		
		triggerPlane.transform.Translate(new Vector3(
			Terrain.activeTerrain.terrainData.size.x/2,
			transform.position.y,
			Terrain.activeTerrain.terrainData.size.z/2), Space.World);
		
		triggerPlane.renderer.enabled = false;
		triggerPlane.collider.isTrigger = true;
		
		triggerPlane.AddComponent(typeof(CheckpointTriggerPlane));
	}
	
}
