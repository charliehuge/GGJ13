#pragma strict

//var objectLink : Transform;
//var target : Transform;
var targetLock : boolean;

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

    var relativePos : Vector3 = Camera.main.transform.position - transform.position;
    //print("ROTATION: " + Quaternion.LookRotation(relativePos).eulerAngles.y);
    var yRotation : float = Quaternion.LookRotation(relativePos).eulerAngles.y;

    transform.rotation = Quaternion.Euler(Vector3(startRotation.eulerAngles.x + 110, yRotation - 180, startRotation.eulerAngles.z));

}

function moveObject () {
	if(targetLock) {
		//transform.position = Vector3(objectLink.position.x, objectLink.position.y, objectLink.position.z);
	}
}
