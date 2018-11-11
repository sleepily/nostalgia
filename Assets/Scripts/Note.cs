using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
  public GameManager gm;

  public long time = 1000;
  public int x = 0;
  public long timeUntilnote = 0;
  public float width = 3f;
  public float size = .5f;
	public float speed = 200f;

	/*
	 * 0 = normal note
	 * 1 = hold note
	 * 2 = long note (?)
	 * 3 = triller note (?)
	 */
	public int noteMode = 0;

  private Vector3 rotationPoint;
  private Vector3 endPosition;

  public GameObject noteModel;

	void Start ()
  {
    noteModel.SetActive(false);

		//speed = gm.noteManager.speed;

    noteModel.transform.localScale = new Vector3(width, size, size);
    rotationPoint = Vector3.right * x;
    endPosition = new Vector3(0, 0, -10);
    endPosition += rotationPoint;
  }
	
	void Update ()
  {
    timeUntilnote = CalculateTimeUntilNote();
    RotateNoteModel();

    if (timeUntilnote < gm.noteManager.approachTime)
      noteModel.SetActive(true);

    if (timeUntilnote < gm.noteManager.despawnTime)
      MissNote();
	}

	void MissNote()
	{
		gm.judgementManager.GetJudgement(gm.noteManager.despawnTime * 2);
		RemoveNote();
	}

  public void Tap(long tapTime)
  {
    long offset = tapTime - time;

		gm.ui.offsetText.text = offset.ToString();

		gm.noteManager.offsetHistory.Insert(0, offset);

		gm.judgementManager.GetJudgement(offset);

    RemoveNote();
  }

  void RemoveNote()
  {
    gm.noteManager.noteList.RemoveAt(0);
    Destroy(gameObject);
  }

  void RotateNoteModel()
  {
    noteModel.transform.localPosition = endPosition;
    noteModel.transform.RotateAround(Vector3.zero, Vector3.left, timeUntilnote * (-gm.noteManager.speed) / 1000);
		noteModel.transform.rotation = Quaternion.identity;
  }

  long CalculateTimeUntilNote()
  {
    return time - gm.audioManager.position;
  }
}
