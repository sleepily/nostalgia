using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public long position = 0;
	public long positionWithoutOffset = 0;
	public AudioClip clip;
	public AudioSource source;

	public long offset = 0;

	void Start()
	{

	}

	void Update()
	{
		if (source.isPlaying)
			positionWithoutOffset = (long)(source.time * 1000);

		position = positionWithoutOffset + offset;
	}

  public void Play()
  {
    if (source.isPlaying)
      return;

    if (source.clip.loadState != AudioDataLoadState.Loaded)
      return;

    source.Play();
  }

  public void LoadSongAudio(string path)
  {
    Debug.Log("Loading Song Audio: " + "Songs/" + path);
    clip = Resources.Load<AudioClip>("Songs/" + path);

    source.clip = clip;
    source.clip.LoadAudioData();
  }
}
