using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class PulseRing : MonoBehaviour
{
	public List<AudioClip> sounds;
	public Transform displayParent;
	public float minDistance = 2f;
	public float interval = 3.0f;
	
	public float range = -1f;
	
	float lastPulseTime;
		
	public Transform playerTransform;
	
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
				//displayParent.transform.localPosition = Vector3.zero;
				//displayParent.transform.LookAt( playerTransform );
			}
		}
		return playerTransform != null;
	}
	
	bool IsInRange()
	{
		if( range <= 0f ) return true;
		
		float d = Vector3.Distance( transform.position, playerTransform.position );
		d -= playerTransform.gameObject.GetComponent<ThirdPersonPlayer>().totalHearingRadius;
		return d < range;
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
			//displayParent.SetActive( true );
			
			float lerpAmt = ( Time.time - lastPulseTime ) / interval;
			
			if( lerpAmt > 1f )
				Fire();
			//Debug.Log(transform.position.x, transform.position.y, transform.position.z);
			Transform tempObjet = Instantiate(displayParent,  new Vector3(0,0,0), Quaternion.identity) as Transform;
			//Debug.Log(transform.Scale);
			//displayParent.transform.LookAt( playerTransform );
			//displayParent.transform.position = Vector3.Lerp( transform.position, playerTransform.position, lerpAmt );
		}
		else
		{
			//displayParent.SetActive( false );	
		}
	}
}
