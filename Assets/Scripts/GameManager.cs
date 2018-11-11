﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public SongManager songManager;
  public AudioManager audioManager;
  public NoteManager noteManager;
	public UICanvas ui;
    
	void Start ()
  {
  }

	void Update ()
  {
		if (Input.GetKeyDown(KeyCode.Return))
			songManager.LoadSong("158023", "UNDEAD CORPORATION - Everything will freeze (Ekoro) [Hard]", "12 - Everything will freeze");
	  //songManager.LoadSong("622946", "KOAN Sound & Asa - fuego (sakuraburst remix) (Couil) [HD]", "audio");
		//songManager.LoadSong("348381", "[deetz' Deception]", "ripdeetz");

		if (Input.GetKeyDown(KeyCode.Space))
      audioManager.Play();
	}
}
