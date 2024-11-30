using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public Action levelCompleted;
    public int puzzleID;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void userBeatAPuzzle()
    {
        levelCompleted?.Invoke();
    }
}
