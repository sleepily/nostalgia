using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;

public class SongManager : MonoBehaviour
{
  public GameManager gm;

	/*
	 * @TODO: put this into a SongInfo.cs class
	 */
  public string beatmapFilePath;
  public string title;
  public string artist;
  public string creator;
  public string audioFilename;
  public string audioLeadIn;

	/*
	 * @TODO: rework this into an array representing the actual line structure
	 */
	public float noteSize;
	public float noteSpeed;
	public float noteDifficulty;

	string[] beatmapLines;

	/*
	 * indices for skipping to certain lines
	 * @TODO: rework this into a list/array that stores these values and make them accessible using indexarray[indexOfXY]
	 */
	int indexGeneral, indexMetadata, indexDifficulty, indexTimingPoints, indexHitObjects;

  public void LoadSong(string folder, string diff, string audio)
  {
    string beatmapFolderPath = Application.dataPath + "/Resources/Songs/" + folder + "/";
    string beatmapAudioPath = folder + "/" + audio; // without file extension. resource loader doesn't need it
    beatmapFilePath = beatmapFolderPath + diff + ".osu";

    LoadBeatmapIntoArray();
    GetBeatmapFileIndices();

    ConvertBeatmapInfo();
		ConvertDifficultyInfo();
    ConvertHitObjects();

    gm.audioManager.LoadSongAudio(beatmapAudioPath);
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

      switch(line)
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

			gm.noteManager.CreateNote(time, x, (int)noteSize, noteSpeed);
		}
	}
}
