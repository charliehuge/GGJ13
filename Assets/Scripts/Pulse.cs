using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Pulse : MonoBehaviour
{
	public AudioClip sound;
	public GameObject displayParent;
	public float minDistance = 2f;
	public float interval = 3.0f;
	
	float lastPulseTime;
		
	Transform playerTransform;
	
	void Start()
	{		
		Fire();
	}
	
	bool HavePlayer()
	{
		if( playerTransform == null )
		{
			Entity player = EntityController.GetInstance().playerEntity;
			if( player )
			{
				playerTransform = player.transform;	
				displayParent.transform.localPosition = Vector3.zero;
				displayParent.transform.LookAt( playerTransform );
			}
		}
		return playerTransform != null;
	}
	
	void Fire()
	{
		if( !HavePlayer() ) return;
		
		lastPulseTime = Time.time;
		
		if( sound )
		{
			audio.clip = sound;
			audio.Play();
		}
	}
	
	void Update()
	{
		if( !HavePlayer() ) return;
		
		float distance = Vector3.Distance( transform.parent.position, playerTransform.position );
		if( distance > 2f )
		{
			displayParent.SetActive( true );
			
			float lerpAmt = ( Time.time - lastPulseTime ) / interval;
			
			if( lerpAmt > 1f )
				Fire();
			
			displayParent.transform.LookAt( playerTransform );
			displayParent.transform.position = Vector3.Lerp( transform.position, playerTransform.position, lerpAmt );
		}
		else
		{
			displayParent.SetActive( false );	
		}
	}
}
