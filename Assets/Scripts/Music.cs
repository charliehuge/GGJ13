using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour 
{
	public List<AudioClip> Files;
	
	int currentTrack;
	
	void PlayNext()
	{
		if( audio.clip != null ) currentTrack = ( currentTrack + 1 ) % Files.Count;
		else currentTrack = 0;
		audio.clip = Files[currentTrack];
		audio.Play();
	}
	
	void PlayRandom()
	{
		audio.clip = Files[Random.Range(0,Files.Count - 1)];
		audio.Play();
	}
	
	void Update()
	{
		if( Files == null || Files.Count == 0 ) return;
		if( !audio.isPlaying )
		{
			PlayRandom();
			//PlayNext();	
		}
	}
}
