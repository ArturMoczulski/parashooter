#pragma strict
var Rotation 		:float;
var EarthTransform 	:Transform;


function Start () {
	EarthTransform = transform;
}

function Update () {
	EarthTransform.Rotate ( 0, Rotation*Time.deltaTime, 0); 
}