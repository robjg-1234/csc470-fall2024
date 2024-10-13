using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class PuzzleGame : MonoBehaviour
{
    //Puzzle Test

    public TMP_Text notification;
    public TMP_Text instructions;
    public GameObject box;
    patternBoxScript[,] playerBoard = new patternBoxScript[5, 5];
    patternBoxScript[,] targetBoard = new patternBoxScript[5, 5];
    float spacing = 1.25f;
    int expectedOscillations = 1;
    int currentOscilations = 1;
    bool comparingArrays = false;
    float step = 1f;
    bool isReset = true;
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                Vector3 pos = new Vector3(x * spacing, 0, y * spacing);
                GameObject newBox = Instantiate(box, pos, Quaternion.identity);
                playerBoard[x, y] = newBox.GetComponent<patternBoxScript>();
                playerBoard[x, y].isAlive = false;
                playerBoard[x, y].isTarget = false;
            }
        }
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                Vector3 pos = new Vector3(x * spacing + 10, 0, y * spacing);
                GameObject newBox = Instantiate(box, pos, Quaternion.identity);
                targetBoard[x, y] = newBox.GetComponent<patternBoxScript>();
                targetBoard[x, y].isAlive = false;
                targetBoard[x, y].isTarget = true;
            }
        }
        setLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (expectedOscillations < 6)
        {

            if (comparingArrays)
            {
                if (currentOscilations > 0)
                {
                    if (step > 0)
                    {
                        step -= Time.deltaTime;
                    }
                    else
                    {
                        nextStep();
                        currentOscilations -= 1;
                        step = 1f;
                    }

                }
                else
                {
                    if (compareArrays())
                    {
                        expectedOscillations += 1;
                        currentOscilations = expectedOscillations;
                        notification.text = "Correct! Press Space to continue.";
                        comparingArrays = false;
                        isReset = false;
                    }
                    else
                    {
                        expectedOscillations = 1;
                        notification.text = "That's the wrong pattern! Press Space to try again.";
                        comparingArrays = false;
                        isReset = false;
                    }
                }
            }
            else
            {
                if (!isReset)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        instructionSet();
                        resetBoard();
                        setLevel();
                        isReset = true;
                        notification.text = "";
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        comparingArrays = true;
                    }
                }

            }
        }
        else
        {
            instructions.text = "You Win!";
            notification.text = "";
        }
    }
    bool compareArrays()
    {
        bool areEqual = true;
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                if (targetBoard[x, y].isAlive != playerBoard[x, y].isAlive)
                {
                    areEqual = false; break;
                }
            }
        }
        return areEqual;
    }

    void nextStep()
    {
        bool[,] nextState = new bool[5, 5];
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                int neighbors = countNeighbors(x, y);
                if (playerBoard[x, y].isAlive && neighbors < 2)
                {
                    nextState[x, y] = false;
                }
                else if (playerBoard[x, y].isAlive && (neighbors == 2 || neighbors == 3))
                {
                    nextState[x, y] = true;
                }
                else if (playerBoard[x, y].isAlive && neighbors > 3)
                {
                    nextState[x, y] = false;
                }
                else if (!playerBoard[x, y].isAlive && neighbors == 3)
                {
                    nextState[x, y] = true;
                }
            }
        }
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                playerBoard[x, y].isAlive = nextState[x, y];
                playerBoard[x, y].setColor();
            }
        }
    }

    void setLevel()
    {
        if (expectedOscillations == 1)
        {
            for (int x = 1; x < 4; x++)
            {
                targetBoard[x, 2].becomeAlive();
            }

        }
        else if (expectedOscillations == 2)
        {
            targetBoard[0, 2].becomeAlive();
            targetBoard[1, 3].becomeAlive();
            targetBoard[1, 1].becomeAlive();
            targetBoard[2, 4].becomeAlive();
            targetBoard[2, 0].becomeAlive();
            targetBoard[3, 3].becomeAlive();
            targetBoard[3, 1].becomeAlive();
            targetBoard[4, 2].becomeAlive();
        }
        else if (expectedOscillations == 3)
        {
            targetBoard[2, 3].becomeAlive();
            for (int x = 0; x < 5; x++)
            {
                for (int y = 1; y < 3; y++)
                {
                    if (x != 2)
                    {
                        targetBoard[x, y].becomeAlive();
                    }
                }
            }
        }
        else if (expectedOscillations == 4)
        {
            targetBoard[3, 2].becomeAlive();
            targetBoard[2, 3].becomeAlive();
            for (int x = 1; x < 4; x++)
            {
                targetBoard[x, 1].becomeAlive();
            }
        }
        else if (expectedOscillations == 5)
        {
            targetBoard[1, 3].becomeAlive();
            targetBoard[2, 3].becomeAlive();
            targetBoard[0, 2].becomeAlive();
            targetBoard[3, 2].becomeAlive();
            for (int x = 0; x < 5; x++)
            {
                if (x != 2)
                {
                    targetBoard[x, 1].becomeAlive();
                }
            }
            targetBoard[2, 0].becomeAlive();
        }
    }
    void resetBoard()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                targetBoard[x, y].isAlive = false;
                targetBoard[x, y].setColor();
                playerBoard[x, y].isAlive = false;
                playerBoard[x, y].setColor();
            }
        }
    }

    void instructionSet()
    {
        if (expectedOscillations == 1)
        {
            instructions.text = "Create a pattern that will be equal to the one on the right in 1 step\r\n(Press space to try)";
        }
        else
        {
            instructions.text = "Create a pattern that will be equal to the one on the right in " + expectedOscillations + " steps\r\n(Press space to try)";
        }
    }
    public int countNeighbors(int xIndex, int yIndex)
    {
        int count = 0;
        for (int x = (xIndex - 1); x <= (xIndex + 1); x++)
        {
            for (int y = (yIndex - 1); y <= (yIndex + 1); y++)
            {
                if (x >= 0 && x < 5 && y >= 0 && y < 5)
                {
                    if (!(x == xIndex && y == yIndex))
                    {
                        patternBoxScript State = playerBoard[x, y].GetComponent<patternBoxScript>();
                        if (State.isAlive)
                        {
                            count++;
                        }
                    }
                }
            }
        }
        return count;
    }

}
