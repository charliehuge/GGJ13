using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour
{
	public GameObject prefab;
	public float interval = 3.0f;
	
	GameObject pulseObject;
	float lastPulseTime;
	
	Vector3 startPos, targetPos;
	
	Transform playerTransform;
	
	void Start()
	{
		if( pulseObject == null )
			pulseObject = GameObject.Instantiate( prefab ) as GameObject;
		if( playerTransform == null )
			playerTransform = EntityController.GetInstance().playerEntity.transform;
		
		Fire();
	}
	
	void Fire()
	{
		lastPulseTime = Time.time;
		startPos = transform.parent.position;
		targetPos = playerTransform.position;
		transform.LookAt( playerTransform );
	}
	
	void Update()
	{
		float lerpAmt = ( Time.time - lastPulseTime ) / interval;
		
		if( lerpAmt > 1f )
			Fire();
		
		transform.position = Vector3.Lerp( startPos, targetPos, lerpAmt );
	}
}
