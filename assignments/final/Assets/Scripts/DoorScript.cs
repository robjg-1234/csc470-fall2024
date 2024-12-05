using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animation openAnimation;
    GameManager manager;
    public Collider doorCheck;
    bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.instance;
        manager.levelCompleted += openDoor;
        manager.pausedGame += setPausedState;
    }
    IEnumerator OpeningDoor()
    {
        openAnimation.Play();
        while (openAnimation.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !paused)
            {
                paused = true;
                while (paused)
                {
                    openAnimation["Open Door"].speed = 0;
                    yield return null;
                }
            }
            openAnimation["Open Door"].speed = 1;
            yield return null;
        }
        doorCheck.isTrigger = true;
    }
    void openDoor()
    {
        if (manager.puzzleID == 100)
        {
            StartCoroutine(OpeningDoor());
        }
    }
    void setPausedState()
    {
        paused=false;
    }
}
