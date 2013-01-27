using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour 
{
	public AudioClip sound;
	public UILabel text;
	public float fadeInTime = 3f;
	public float endSize = 50f;
	
	void Awake()
	{
		text.alpha = 0f;
	}
	
	public void Go()
	{
		audio.clip = sound;
		audio.Play();
		StartCoroutine( DoThings() );
	}
	
	IEnumerator DoThings()
	{
		float startSize = 20f;
		
		float elapsed = 0f;
		
		while( elapsed < fadeInTime )
		{
			elapsed += Time.deltaTime;
			float lerpTime = elapsed / fadeInTime;
			text.alpha = lerpTime;
			/*
			float lerpSize = Mathf.Lerp( startSize, endSize, lerpTime );
			text.transform.localScale = new Vector3( lerpSize, lerpSize, 1f );
			*/
			yield return null;
		}
		
		while( audio.isPlaying )
		{
			yield return null;	
		}
		
		text.alpha = 0f;
		
		Game.FinishCurrentLevel();
	}
}
