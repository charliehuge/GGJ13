#pragma strict

var objectLink : Transform;
var target : Transform;

internal var startRotation : Quaternion;

function Start () {
	startRotation = transform.rotation;
	moveObject();
	lookAtCamera();
}

function Update () {
	moveObject();
	lookAtCamera();
}

function lookAtCamera () {
    //transform.LookAt(target);

    var relativePos : Vector3 = target.position - transform.position;
    //print("ROTATION: " + Quaternion.LookRotation(relativePos).eulerAngles.y);
    var yRotation : float = Quaternion.LookRotation(relativePos).eulerAngles.y;

    transform.rotation = Quaternion.Euler(Vector3(startRotation.eulerAngles.x + 90, yRotation - 180, startRotation.eulerAngles.z));

}

function moveObject () {
	transform.position = Vector3(objectLink.position.x, objectLink.position.y, objectLink.position.z);
}