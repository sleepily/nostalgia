using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
  public GameManager gm;
  
	void Start ()
  {
		
	}
	
	void Update ()
  {
    if (!gm.audioManager.source.isPlaying)
      return;

    if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C))
      Tap();
	}

  void Tap()
  {
    Note n = gm.noteManager.noteList[0];
    n.Tap(gm.audioManager.position);
  }
}
