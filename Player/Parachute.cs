using System;
using UnityEngine;

public class Parachute : MonoBehaviour
{
	
	void Start()
	{
		this.gameObject.SetActive(false);
	}
	
	void Update()
	{
		if( this.Opened && !this.FullyOpened ) {
			this.inflate();
		}
	}
	
	/**
	 * how fast the parachute opens
	 */
	public float inflationSpeed = 0.001f;
	
	/**
	 * set to true to trigger opening the parachute
	 */
	public bool Opened { 
		get { return this.InflationProgress > 0; }
		set {
			if( !this.Opened && value ) {
				this.gameObject.SetActive(true);
				inflate();
			}
			if( value == false ) {
				throw new Exception("Closing the parachute is not supported.");
			}
		}
	}
	
	public bool FullyOpened { get { return InflationProgress >= 1.0f; } }
	public float InflationProgress { get { return inflationProgress; } }
	
	/**
	 * drag caused by the parachute when fully opened
	 */
	public float drag;
	
	/**
	 * current drag caused by the parachute
	 */
	public float CurrentDrag {
		get 
		{
			return drag * inflationProgress;
		}
	}
	
	/**
	 * progress opening of the parachute
	 */
	public void inflate()
	{
		if( InflationProgress == 0.0f ) {
			adjustPositioning();
		}
		
		if( InflationProgress < 1.0f ) {
			inflationProgress += inflationSpeed * Time.deltaTime;
		}
		if( InflationProgress > 1.0f) {
			inflationProgress = 1.0f;
		}
	
	}
	
	/**
	 * adjust positioning of the human character and the
	 * parachute; needs to be done when the parachute
	 * is first opened
	 */
	private void adjustPositioning()
	{
		/**
		 * move the pivot to the center of the parachute;
		 * this is accomplished by swapping relative
		 * position of the human model and the parachute
		 * model inside the parent container
		 */
		PlayerController player = ((PlayerController)UnityEngine.Object.FindObjectOfType(typeof(PlayerController)));
		
		float yDifference = transform.position.y - player.humanTransform.position.y;
			
		player.modelTransform.Translate(0, yDifference, 0);
		
		player.transform.Translate(0, -yDifference, 0);
		
		// adjust camera
		player.cameraTransform.Translate(new Vector3(0, -5, -4));
	}
	
	/**
	 * range 0 - 1
	 */
	private float inflationProgress = 0.0f;
	
}

