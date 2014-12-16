using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParashooterLevel : Level {
	
	protected List<GameObject> checkpoints;
	protected GameObject landingZone;
	protected float jumpHeight;
	protected Vector2 playersStartingHorizontalPosition;
	
	public float JumpHeight { get { return jumpHeight; } set { jumpHeight = value; } }
	public List<GameObject> Checkpoints { get { return checkpoints; } set { checkpoints = value;} }
	public GameObject LandingZone { get { return landingZone; } set { landingZone = value; } }
	public Vector2 PlayersStartingHorizontalPosition { get { return playersStartingHorizontalPosition; } set { playersStartingHorizontalPosition = value; } }
	
	public ParashooterLevel() : base() {
		checkpoints = new List<GameObject>();
	}
	
}
