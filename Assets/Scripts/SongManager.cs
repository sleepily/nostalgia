using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using System.Linq;

public class SongManager : MonoBehaviour
{
    public GameManager gm;

    public bool isLoaded = false;

    /*
	 * @TODO: put this into a SongInfo.cs class
	 */
    public string beatmapFilePath;
    public string title;
    public string artist;
    public string creator;
    public string audioFilename;
    public string audioLeadIn;
    public float beatLength;

    /*
	 * @TODO: rework this into an array representing the actual line structure
	 */
    public float noteSize;
    public float noteSpeed;
    public float noteDifficulty;
    public float sliderMultiplier;
    public float sliderTickRate;

    string[] beatmapLines;

    /*
	 * indices for skipping to certain lines
	 * @TODO: rework this into a list/array that stores these values and make them accessible using indexarray[indexOfXY]
	 */
    int indexGeneral, indexMetadata, indexDifficulty, indexTimingPoints, indexHitObjects;

    public void LoadSong(string folder, string diff)
    {
        isLoaded = false;

        string beatmapFolderPath = Application.dataPath + "/Resources/Songs/" + folder + "/";
        beatmapFilePath = beatmapFolderPath + diff + ".osu";

        LoadBeatmapIntoArray();
        GetBeatmapFileIndices();

        ConvertBeatmapInfo();
        ConvertDifficultyInfo();
        ConvertTimingInfo();
        ConvertHitObjects();

        string beatmapAudioPath = folder + "/" + audioFilename; // without file extension. resource loader doesn't need it

        gm.audioManager.LoadSongAudio(beatmapAudioPath);

        isLoaded = true;
    }

    void LoadBeatmapIntoArray()
    {
        beatmapLines = File.ReadAllLines(beatmapFilePath);
    }

    void GetBeatmapFileIndices()
    {
        /*
         * index represents line numbers, so that when accessed in beatmapLines,
         * the actual header line will be skipped
         */
        int index = 0;

        foreach (string line in beatmapLines)
        {
            index++;

            switch (line)
            {
                case "[General]":
                    indexGeneral = index;
                    break;

                case "[Metadata]":
                    indexMetadata = index;
                    break;

                case "[Difficulty]":
                    indexDifficulty = index;
                    break;

                case "[TimingPoints]":
                    indexTimingPoints = index;
                    break;

                case "[HitObjects]":
                    indexHitObjects = index;
                    return; // no need to continue further since [HitObjects] is the file's last section

                default:
                    break;
            }
        }
    }

    void ConvertDifficultyInfo()
    {
        int index = 0;

        foreach (string line in beatmapLines)
        {
            if (line == "\r|\n|\rn")
                return;

            if (index < indexDifficulty)
            {
                index++;
                continue;
            }

            string[] property = Regex.Split(line, ":"); // get first word from current line

            switch (property[0])
            {
                case "CircleSize":
                    noteSize = float.Parse(property[1]); // @TODO: read everything after first colon (remove indedx 0 from array and convert array into datapoint)
                    break;

                case "ApproachRate":
                    noteSpeed = float.Parse(property[1]);
                    break;

                case "SliderMultiplier":
                    sliderMultiplier = float.Parse(property[1]);
                    break;

                case "SliderTickRate":
                    sliderTickRate = float.Parse(property[1]);
                    break;

                case "OverallDifficulty":
                    noteDifficulty = float.Parse(property[1]);
                    break;

                default:
                    break;
            }
        }
    }

    void ConvertBeatmapInfo()
    {
        int index = 0;

        foreach (string line in beatmapLines)
        {
            if (index < indexGeneral)
            {
                index++;
                continue;
            }

            string[] property = Regex.Split(line, ": "); // get first word from current line

            switch (property[0])
            {
                case "AudioFilename":
                    audioFilename = property[1];
                    break;

                case "AudioLeadIn":
                    audioLeadIn = property[1];
                    break;

                default:
                    return;
            }
        }
    }

    void ConvertTimingInfo()
    {
        int index = 0;

        foreach (string line in beatmapLines)
        {
            if (index < indexTimingPoints)
            {
                index++;
                continue;
            }

            string[] property = Regex.Split(line, ","); // get first word from current line

            gm.audioManager.beatLength = float.Parse(property[1]);
            beatLength = float.Parse(property[1]);
            return;
        }
    }

    void ConvertHitObjects()
    {
        int index = 0;

        foreach (string line in beatmapLines)
        {
            if (index < indexHitObjects)
            {
                index++;
                continue;
            }

            string[] dataPoints = Regex.Split(line, ",");

            long time = long.Parse(dataPoints[2]);
            int x = int.Parse(dataPoints[0]) * 3 / 64;

            // if hitobject == note
            if (dataPoints[5] == "0:0:0:0:")
            {
                gm.noteManager.CreateNote(time, x, (int)noteSize, noteSpeed);
                continue;
            }

            // if hitobject == holdnote (slider tick note equivalent)
            string dataPoint = dataPoints[5];

            string[] sliderPoints = Regex.Split(dataPoint, "\\|");

            //sliderPoints = sliderPoints.Skip(1).ToArray(); // skip letter
            string sliderEndPoint = sliderPoints.Last();

            int sliderEndPointX = int.Parse(Regex.Split(sliderEndPoint, ":")[0]);

            float pixelLength = 0f;

            if (dataPoints.Length > 7)
                pixelLength = float.Parse(dataPoints[7]);

            float sliderDuration = pixelLength / (100.0f * sliderMultiplier) * gm.audioManager.beatLength;
            float sliderTickFrequency = beatLength / sliderTickRate;
            int sliderTicks = Mathf.FloorToInt(sliderDuration / sliderTickFrequency);

            gm.noteManager.CreateNote(time, x, (int)noteSize, noteSpeed);
            gm.noteManager.CreateNote(time + (long)sliderDuration, x, (int)noteSize, noteSpeed - noteSpeed / 5);

            /*
               * HOLD NOTE IMPLEMENTATION PROCESS
               * 
               * createHoldNote(x, time, sliderEndPointX, sliderTicks, sliderTickFrequency)
               * 
               * calculate their x positions (x + (sliderEndPointX - x) * currentSliderTick) and times (startNote.time + (beatLength / sliderTickRate) * currentSliderTick)
               * @TODO: account for rounding errors and take them into consideration later
               */

            // if hitobject == long note (slider equivalent)


            /*
               * LONG NOTE IMPLEMENTATION PROCESS
               * 
               * createLongNote(x, time, sliderDuration)
               * how to display long note body? what length? (> scrollspeed, sliderDuration)
               */
        }
    }
}
