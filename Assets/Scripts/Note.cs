using UnityEngine;

public abstract class Note : MonoBehaviour
{
    public GameManager gm;

    public long time = 1000;
    public int x = 0;
    public long timeUntilnote = 0;
    public float width = 3f;
    public float size = .5f;
    public float speed = 200f;

    private Vector3 rotationPoint;
    private Vector3 endPosition;

    public GameObject noteModel;

    void Start()
    {
        noteModel.SetActive(false);

        noteModel.transform.localScale = new Vector3(width, size, size);
        rotationPoint = Vector3.right * x;
        endPosition = new Vector3(0, 0, -10);
        endPosition += rotationPoint;
    }

    void Update()
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

        int judgement = gm.judgementManager.GetJudgement(offset);

        // if the note is hit way too early, do not remove or count as hit
        if (judgement != -1)
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
