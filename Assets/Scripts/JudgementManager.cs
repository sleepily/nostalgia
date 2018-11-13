using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementManager : MonoBehaviour
{
    public GameManager gm;

    /*
	 * @TODO: implement long/hold note judgement
	 */
    public int GetJudgement(long offset)
    {
        int judgement = CalculateJudgement(offset);

        DisplayJudgement(judgement);
        RenderStats();

        return judgement;
    }

    void DisplayJudgement(int judgementIndex)
    {
        string judgementString = Judgement.GetJudgementString(judgementIndex);

        if (judgementString != "")
        {
            gm.ui.judgementText.text = judgementString;
            gm.noteManager.stats[judgementIndex] += 1;
        }
    }

    void RenderStats()
    {
        string statsString = "";

        foreach (int i in gm.noteManager.stats)
            statsString += i + " ";

        gm.ui.statsText.text = statsString;
    }

    int CalculateJudgement(long offset)
    {
        if (offset <= Judgement.MISS / 2)
            return -1;

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
