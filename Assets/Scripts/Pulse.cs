using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class Pulse : MonoBehaviour
{
	public List<AudioClip> sounds;
	public GameObject displayParent;
	public float minDistance = 2f;
	public float interval = 3.0f;
	
	public float range = -1f;
	
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
	
	bool IsInRange()
	{
		return ( range <= 0f ) 
			|| Vector3.Distance( transform.position, playerTransform.position ) < range;	
	}
	
	void Fire()
	{
		if( !HavePlayer() || !IsInRange() ) return;
		
		lastPulseTime = Time.time;
		
		if( sounds != null && sounds.Count > 0 )
		{
			audio.clip = sounds[ Random.Range( 0, sounds.Count - 1 ) ];
			audio.Play();
		}
	}
	
	void Update()
	{
		if( !HavePlayer() ) return;
		
		float distance = Vector3.Distance( transform.parent.position, playerTransform.position );
		if( distance > minDistance && IsInRange() )
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
