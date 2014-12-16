using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotatableMenu : SequentionalMenu {
	
	public enum RotationAxis {X, Y, Z};
	
	public int rotationSpeed = 5;
	public int rotationAccelaration = 100;
	public RotationAxis rotationAxis = RotationAxis.Y;
	
	public float sphereSize = 70;
	
	protected void Awake() {
		base.Awake();
		if (rotationSpeed == 0) rotationSpeed = 1;
	}
	
	void Start () {		
		
		createMenuElements();
		
	}
	
	protected void Update () {
	
		base.Update();
		
		if (menuElements.Count == 0) 
			return;
		
		showSelected();
		
	}
	
	private void showSelected() {
		
		float proximityFactor = Mathf.Abs(transform.eulerAngles.y - (360 / menuElements.Count)*currentlySelected) / 10000 * rotationAccelaration;
		float rotationDelta = (10*Time.deltaTime*10*rotationSpeed*proximityFactor);
		
		switch (rotationAxis) {
			case RotationAxis.X:
				handleXAxisRotation(rotationDelta);
				break;
			case RotationAxis.Y:
				handleYAxisRotation(rotationDelta);
				break;
			case RotationAxis.Z:
				handleZAxisRotation(rotationDelta);
				break;
		}
	}
	
	private void handleXAxisRotation(float rotation) {
		
		if ( transform.eulerAngles.x < (360 / menuElements.Count)*currentlySelected ) {
			
			if ( transform.eulerAngles.x + rotation > (360 / menuElements.Count)*currentlySelected ) 
				transform.rotation = Quaternion.Euler(new Vector3((360 / menuElements.Count)*currentlySelected,0,0));
			else
				transform.Rotate(new Vector3(rotation,0,0));
			
		} else if ( transform.eulerAngles.x > (360 / menuElements.Count)*currentlySelected ) {
		
			rotation = -rotation;
			
			if ( transform.eulerAngles.x + rotation < (360 / menuElements.Count)*currentlySelected ) 
				transform.rotation = Quaternion.Euler(new Vector3((360 / menuElements.Count)*currentlySelected,0,0));
			else
				transform.Rotate(new Vector3(rotation,0,0));
		}
		
	}
	
	private void handleYAxisRotation(float rotation) {
		
		if ( transform.eulerAngles.y < (360 / menuElements.Count)*currentlySelected ) {
			
			if ( transform.eulerAngles.y + rotation > (360 / menuElements.Count)*currentlySelected ) 
				transform.rotation = Quaternion.Euler(new Vector3(0,(360 / menuElements.Count)*currentlySelected,0));
			else
				transform.Rotate(new Vector3(0,rotation,0));
			
		} else if ( transform.eulerAngles.y > (360 / menuElements.Count)*currentlySelected ) {
		
			rotation = -rotation;
			
			if ( transform.eulerAngles.y + rotation < (360 / menuElements.Count)*currentlySelected ) 
				transform.rotation = Quaternion.Euler(new Vector3(0,(360 / menuElements.Count)*currentlySelected,0));
			else
				transform.Rotate(new Vector3(0,rotation,0));
		}
		
	}
	
	private void handleZAxisRotation(float rotation) {
		
		if ( transform.eulerAngles.z < (360 / menuElements.Count)*currentlySelected ) {
			
			if ( transform.eulerAngles.z + rotation > (360 / menuElements.Count)*currentlySelected ) 
				transform.rotation = Quaternion.Euler(new Vector3(0,0,(360 / menuElements.Count)*currentlySelected));
			else
				transform.Rotate(new Vector3(0,0,rotation));
			
		} else if ( transform.eulerAngles.z > (360 / menuElements.Count)*currentlySelected ) {
		
			rotation = -rotation;
			
			if ( transform.eulerAngles.z + rotation < (360 / menuElements.Count)*currentlySelected ) 
				transform.rotation = Quaternion.Euler(new Vector3(0,0,(360 / menuElements.Count)*currentlySelected));
			else
				transform.Rotate(new Vector3(0,0,rotation));
		}
		
	}
	
	protected override void positionMenuElements() {
		
		float distributionAngle = 360 / menuElements.Count;
		
		int i=0;
		foreach(GameObject menuElement in menuElements) {
			Vector3 rotation = new Vector3(0,0,0);
			
			if (rotationAxis == RotationAxis.X) rotation.x = -distributionAngle*i;
			if (rotationAxis == RotationAxis.Y) rotation.y = -distributionAngle*i;
			if (rotationAxis == RotationAxis.Z) rotation.z = -distributionAngle*i;
			menuElement.transform.Rotate(rotation);
			
			menuElement.transform.Translate(Vector3.back*sphereSize);
			i++;
		}
		
	}
	
}
