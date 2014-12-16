using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {
	
	public Vector3 rotationDirection;
	public float rotationSpeed;
	public Transform around;
	public Vector3 relativeDistance;
	
	public void Reset() {
		positionCamera();
	}
	
	public void Start() {
		positionCamera();
	}
	
	protected void positionCamera() {
		if( relativeDistance != null ) {
			
			if( around.GetComponent<BoundsMeasure>() == null )
				throw new UnityException("The orbit center object needs the BoundsMeasure component attached.");
			
			Bounds aroundBounds = around.GetComponent<BoundsMeasure>().getBounds();
			
			Vector3 aroundSize = aroundBounds.size;
			
			transform.Translate( new Vector3(
				-transform.localPosition.x + around.transform.localPosition.x +	(aroundSize.x * relativeDistance.x),
				-transform.localPosition.y + around.transform.localPosition.y + (aroundSize.y * relativeDistance.y),
				-transform.localPosition.z + around.transform.localPosition.z + (aroundSize.z * relativeDistance.z)
				), Space.World );
			
		}
	}
	
	public void Update() {
		if( rotationDirection != null ) {
			if( around == null ) {
				transform.Rotate(new Vector3(
					rotationDirection.x * Time.deltaTime,
					rotationDirection.y * Time.deltaTime,
					rotationDirection.z * Time.deltaTime
				));
			} else {
				transform.RotateAround(around.position, rotationDirection, rotationSpeed);
				transform.LookAt(around);
			}
		}
	}
	
}
