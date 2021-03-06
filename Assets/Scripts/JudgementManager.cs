﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementManager : MonoBehaviour
{
	public GameManager gm;
	
	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}

	/*
	 * @TODO: implement long/hold note judgement
	 */
	public void GetJudgement(long offset)
	{
		int judgement = CalculateJudgement(offset);
		string judgementString = "";

		judgementString =
			(judgement == 0) ? "PERFECT" :
			(judgement == 1) ? "GREAT" :
			(judgement == 2) ? "GOOD" :
			(judgement == 3) ? "BAD" : "MISS";

		gm.ui.judgementText.text = judgementString;
		gm.noteManager.stats[judgement] += 1;

		string statsString = "";

		foreach (int i in gm.noteManager.stats)
			statsString += i + " ";

		gm.ui.statsText.text = statsString;
	}

	int CalculateJudgement(long offset)
	{
		float abs = Mathf.Abs(offset);

		if (abs <= Judgement.PERFECT / 2)
			return 0;
		if (abs <= Judgement.GREAT / 2)
			return 1;
		if (abs <= Judgement.GOOD / 2)
			return 2;
		if (abs <= Judgement.BAD / 2)
			return 3;

		return 4;
	}
}
