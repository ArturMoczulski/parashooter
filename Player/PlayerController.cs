using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{	
	/**
	 * Weird shortcut to current object's transform
	 * introduced by Rob
	 */
	public Transform playerTransform;
	
	/**
	 * Grouping GameObject for applying player's pitch
	 */
	public GameObject pitchController;
	/**
	 * Grouping GameObject for applying player's roll
	 */
	public GameObject rollController;
	
	/**
	 * GameObject containing all the elements of the
	 * model, including pivot containers, etc.
	 */
	public Transform modelTransform;
	/**
	 * GameObject containing the 3D model of the human
	 */
	public Transform humanTransform;
	private Human human;
	/**
	 * GameObject containing the 3D model of the parachute
	 */
	public Transform parachuteTransform;
	private Parachute parachute;
	
	public Transform cameraTransform;
	
	/**
	 * GameObjects for controlling player's rotation
	 */
	public GameObject TargetX { get { return targetX; } }
	public GameObject TargetZ { get { return targetZ; } }
	private GameObject targetX;
	private GameObject targetZ;
	
	// maximum horizontal (x,z) speed
	public float maxTilt = 10;
	
	// acceleration of direction change
	public float tiltSpeed = 0.25f;
	
	// general horizontal (x,z) speed
	public float horizontalSpeed = 0.25f;
	
	public float turnSpeed = 1f;
	
	public float tiltSmoothing = 8;
	public GameObject targetPrefab;
	
	/**
	 * Calculated on runtime based on target objects
	 */
	private float maxAngle = 0;
	
	public float getMaximumRoll() { return maxAngle; }
	public float getMaximumPitch() { return maxAngle; }
	
	protected void handleUserInput() {
		
		if (!GameState.Instance.LevelStarted || GameState.Instance.LevelFinished)
			return;
		
		if( Input.GetKeyDown(KeyCode.Pause) ) {
			Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
		}
		
		float verticalTilt = 0;
		float horizontalTilt = 0;
		float turn = 0;
		
		/**
		 * Smartphone based control
		 */
		
		horizontalTilt = Input.acceleration.x*maxTilt;
		verticalTilt = Input.acceleration.y*maxTilt;
		// turn = Input.acceleration.z*10;
		
		// roll
		targetX.transform.localPosition = new Vector3(horizontalTilt,0,0);
		
		// pitch
		targetZ.transform.localPosition = new Vector3(0,0,verticalTilt);
		
		// HTC Desire Z used for testing does not have a gyroscope required for turning
		// yaw
		// transform.localRotation = Quaternion.Euler(new Vector3(0,turn,0));
		
		/**
		 * Keyboard based control
		 *
		if (Input.GetKey(KeyCode.Q)) 
			horizontalTilt = -1;
		else if (Input.GetKey(KeyCode.E))
			horizontalTilt = 1;
		
		if (Input.GetKey(KeyCode.W) )
			verticalTilt = 1;
		else if (Input.GetKey(KeyCode.S) )
			verticalTilt = -1;
		
		if (Input.GetKey(KeyCode.A) )
			turn = -1;
		else if (Input.GetKey(KeyCode.D) )
			turn = 1;
		
		Vector3 deltaTranslation = new Vector3(
			horizontalTilt*tiltSpeed*Time.deltaTime*50,
			0,
			verticalTilt*tiltSpeed*Time.deltaTime*50);
		
		if (Mathf.Abs(targetX.transform.localPosition.x + deltaTranslation.x) < maxTilt ) {
			targetX.transform.Translate(deltaTranslation.x,0,0);
		}
		if (Mathf.Abs(targetZ.transform.localPosition.z + deltaTranslation.z) < maxTilt ) {
			targetZ.transform.Translate(0,0,deltaTranslation.z);
		}
		
		// turning
		float deltaRotation = turn*turnSpeed*Time.deltaTime*50;
		transform.Rotate(new Vector3(0,deltaRotation,0));
		*/
	}
	
	void Update() {
		
		handleUserInput();
		
		moveHorizontally();	
		
		faceDirection();
		
		if( this.parachute && this.parachute.Opened ) {
			rigidbody.drag = parachute.CurrentDrag < human.drag ? human.drag : parachute.CurrentDrag;
		}
		
	}
	
	void moveHorizontally() {
		
		Vector3 translationVector = new Vector3(0,0,0);
		
		// relating the speed of horizontal movement to the speed of falling, and of course roll and pitch
		translationVector.x = 
			-rigidbody.velocity.y/100 *
			horizontalSpeed * 
			targetX.transform.localPosition.x *
			(rigidbody.drag*8);
		
		translationVector.y = 0;
		
		translationVector.z = 
			-rigidbody.velocity.y/100 * 
			horizontalSpeed * 
			targetZ.transform.localPosition.z * 
			(rigidbody.drag*4);
		
		playerTransform.Translate(translationVector*Time.deltaTime);
		
	}
	
	void faceDirection() {	
		
		Vector3 oldRotation = new Vector3(
			pitchController.transform.eulerAngles.x,
			0,
			rollController.transform.eulerAngles.z);
		
		pitchController.transform.eulerAngles = new Vector3(
			targetZ.transform.localPosition.z/maxTilt * maxAngle,
			pitchController.transform.eulerAngles.y,
			pitchController.transform.eulerAngles.z);
		
		rollController.transform.eulerAngles = new Vector3(
			rollController.transform.eulerAngles.x,
			rollController.transform.eulerAngles.y,
			-targetX.transform.localPosition.x/maxTilt * maxAngle);
		
	}
	
	void Start() {
		
		this.parachute = parachuteTransform.GetComponent<Parachute>();
		this.human = humanTransform.GetComponent<Human>();
		
		createTargets();
		
		maxAngle = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(targetX.transform.localPosition.y) / maxTilt);
		
		/**
		 * enable animation
		 */
		if( modelTransform.animation ) {
			modelTransform.animation.wrapMode =  WrapMode.Loop;
		}
		
		/**
		 * rotate the player model
		 */
		humanTransform.RotateAround(humanTransform.collider.bounds.center, Vector3.right, 90);
		
		/**
		 * reset drag
		 */
		rigidbody.drag = humanTransform.GetComponent<Human>().drag;
	}
	
	/**
	 * Controller targets
	 */
	void createTargets() {
		
		if (targetPrefab == null) {
			targetX = new GameObject();
			targetZ = new GameObject();
		} else {
			targetX = (GameObject)Instantiate(targetPrefab);
			targetZ = (GameObject)Instantiate(targetPrefab);
		}
		
		targetX.name = "Target X transform";
		targetX.transform.parent = transform;
		targetX.transform.localPosition = new Vector3(0,0,0);
		targetX.transform.Translate(new Vector3(0,-20,0)); 
		
		targetZ.name = "Target Z transform";
		targetZ.transform.parent = transform;
		targetZ.transform.localPosition = new Vector3(0,0,0);
		targetZ.transform.Translate(new Vector3(0,-20,0)); 
	}
	
	void OnCollisionEnter(Collision collision) {
		
		if( collision.gameObject.GetComponent<Terrain>() ) {
			
			GameState.Instance.LevelFinished = true;
			
		}
		
	}
	
}


