using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform targetTransform;
	public Transform cameraTransform;
	public Vector3 distance;
	
	void Reset() {
		distance = new Vector3( 0, 5, -2 );		
	}
	
	
	void Start() {
		cameraTransform = transform;	
	}
		
	
	void Update() {
		
		cameraTransform.position = targetTransform.position + distance; 
		cameraTransform.LookAt(targetTransform);
		
	} 
}


