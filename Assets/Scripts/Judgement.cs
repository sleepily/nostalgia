using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgement
{
    /*
	 *                       O                 <- hitobject time (eg. 550 ms)
	 *                 -5 |-----| +5           <- perfect hit (10 ms window)
	 *             -12 |------------| +12      <- great hit (24 ms window)
	 * TIME  ............X.................... <- example user input (542 ms)
	 * 
	 * RESULT: GREAT HIT
	 */

    public static int PERFECT = 10;
    public static int GREAT = 24;
    public static int GOOD = 46;
    public static int BAD = 68;
    public static int MISS = 100;

    public static string GetJudgementString(int judgementindex)
    {
        switch (judgementindex)
        {
            case -1:
                return "";
            case 0:
                return "PERFECT";
            case 1:
                return "GREAT";
            case 2:
                return "GOOD";
            case 3:
                return "BAD";
            default:
                return "";
        }
    }
}
