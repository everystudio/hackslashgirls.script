using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEControl : Singleton<SEControl> {

	public List<AudioSource> audio_source_list = new List<AudioSource>();

	public List<AudioClip> audio_clip_list = new List<AudioClip>();

	/*
	void Start()
	{
		foreach( AudioClip clip in audio_clip_list)
		{
			Debug.Log(clip.name);
		}
	}
	*/

	public void Play(string _strName)
	{
		AudioSource is_playing = audio_source_list.Find(p => p.clip != null && p.clip.name == _strName);

		if( is_playing != null)
		{
			is_playing.Stop();
			is_playing.Play();
		}

		AudioSource empty = audio_source_list.Find(p => p.isPlaying == false);

		AudioClip clip = audio_clip_list.Find(p => p.name == _strName);

		if( empty != null && clip != null)
		{
			empty.clip = clip;
			empty.Play();
		}

	}

}
