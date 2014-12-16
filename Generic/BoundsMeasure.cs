using UnityEngine;
using System.Collections;

public class BoundsMeasure : MonoBehaviour {

	public Bounds getBounds() {
		
		Bounds bounds = gameObject.collider != null ? gameObject.collider.bounds : new Bounds();
		foreach( Collider childCollider in GetComponentsInChildren(typeof(Collider)) ) {
			bounds.Encapsulate(childCollider.bounds);
		}
		return bounds;
		
	}
	
}
