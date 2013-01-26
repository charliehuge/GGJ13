using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour
{
	public GameObject prefab;
	public float interval = 3.0f;
	
	GameObject pulseObject;
	float lastPulseTime;
	
	Vector3 startPos;
	
	Transform playerTransform;
	
	void Start()
	{
		if( pulseObject == null )
		{
			pulseObject = GameObject.Instantiate( prefab ) as GameObject;
			pulseObject.transform.parent = transform;
			pulseObject.transform.localPosition = prefab.transform.position;
			pulseObject.transform.localRotation = Quaternion.identity;
		}
		if( playerTransform == null )
			playerTransform = EntityController.GetInstance().playerEntity.transform;
		
		Fire();
	}
	
	void Fire()
	{
		lastPulseTime = Time.time;
		startPos = transform.parent.position;
		transform.LookAt( playerTransform );
	}
	
	void Update()
	{
		float distance = Vector3.Distance( transform.parent.position, playerTransform.position );
		if( distance > 2f )
		{
			pulseObject.SetActive( true );
			
			float lerpAmt = ( Time.time - lastPulseTime ) / interval;
			
			if( lerpAmt > 1f )
				Fire();
			
			transform.LookAt( playerTransform );
			transform.position = Vector3.Lerp( startPos, playerTransform.position, lerpAmt );
		}
		else
		{
			pulseObject.SetActive( false );	
		}
	}
}
