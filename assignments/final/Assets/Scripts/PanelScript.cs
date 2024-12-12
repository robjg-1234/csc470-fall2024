using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScript : MonoBehaviour
{
    public bool isSelected;
    bool isUnlocked;
    public PuzzleScript uniquePuzzle;
    public int remainingPuzzles = 0;
    public int PuzzleID = 100;
    GameManager manager;
    private void Start()
    {
        manager = GameManager.instance;
        if (remainingPuzzles == 0)
        {
            isUnlocked = true;
        }
        else
        {
            isUnlocked = false;
        }
        manager.levelCompleted += nextPuzzle;
    }
    void nextPuzzle()
    {
        if (manager != null)
        {
            if (manager.puzzleID == PuzzleID)
            {
                remainingPuzzles--;
                if (remainingPuzzles <= 0)
                {
                    if (!isUnlocked)
                    {
                        isUnlocked = true;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (uniquePuzzle != null)
        {
            if (isUnlocked)
            {
                uniquePuzzle.gameObject.SetActive(true);
                if (isSelected)
                {
                    uniquePuzzle.isLoaded = true;
                }
                else
                {
                    uniquePuzzle.isLoaded = false;
                }
                
            }
            else
            {
                uniquePuzzle.gameObject.SetActive(false);
            }
            
        }
    }
}
