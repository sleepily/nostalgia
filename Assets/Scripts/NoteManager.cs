﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
	public GameManager gm;

  public Note notePrefab;

  public List<Note> noteList;
  public Note nextNote;
	public List<long> offsetHistory = new List<long>();
	public int offsetCapacity = 10;

	/*
	 * player stats: how many perfect, great, good, bad hits and misses
	 */
	public int[] stats = new int[5];

	/*
	 * For Normal Gameplay
	 * a t 1224
	 * d t -382
	 * spe  200
	 */
	/*
	 * For Visualization
	 * a t 1094
	 * d t -522
	 * spe  222
	 */

	public int approachTime = 300;
  public int despawnTime = -100;
  public float speed = 100f;

	void Start()
  {
		// judgement testing
		despawnTime = -Judgement.BAD / 2;

		noteList = new List<Note>();
		offsetHistory.Capacity = offsetCapacity;
  }

  void Update()
  {
		
  }

	void FixedUpdate()
	{
		gm.ui.urText.text = "UR: " + CalculateUnstableRate();
	}

	public long CalculateUnstableRate()
	{
		long sum = 0;

		foreach(long offset in offsetHistory)
		{
			if (offset.Equals(null))
				break;
			
			sum += offset;
		}

		if (offsetHistory.Count < 1)
			return 0;

		return sum / offsetHistory.Count;
	}

	public void CreateNote(long time, int x, int width, float speed)
	{
    Note n = Instantiate(notePrefab, transform);
    n.gm = gm;
		n.time = time;
		n.x = x;

		noteList.Add(n);
	}
}
