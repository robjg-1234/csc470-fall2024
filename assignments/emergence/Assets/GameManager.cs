using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public GameObject box;
    GameObject[,] gridFloor = new GameObject[12, 12];
    public GameObject player;
    float spacing = 2f;
    float GlobalTimer = 4f;
    float resetTimer = 10f;
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 12; x++)
        {
            for (int y = 0; y < 12; y++)
            {
                Vector3 pos = new Vector3(x * spacing, 0, y * spacing);
                gridFloor[x, y] = Instantiate(box, pos, Quaternion.identity);
                BoxScript boxScript = gridFloor[x, y].GetComponent<BoxScript>();
                boxScript.isAlive = (Random.value > 0.5);
            }
        }

    }
    public int countNeighbors(int xIndex, int yIndex)
    {
        int count = 0;
        for (int x = (xIndex - 1); x <= (xIndex + 1); x++)
        {
            for (int y = (yIndex - 1); y <= (yIndex + 1); y++)
            {
                if (x >= 0 && x < 12 && y >= 0 && y < 12)
                {
                    if (!(x == xIndex && y == yIndex))
                    {
                        BoxScript State = gridFloor[x, y].GetComponent<BoxScript>();
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

    // Update is called once per frame
    void Update()
    {
        if (GlobalTimer < 0)
        {
            changeState();
            GlobalTimer = 4f;
            //Debug.Log(resetTimer);
            if (resetTimer < 1)
            {
                resetTimer = 10f;
                remixFloor();
            }
            else
            {
                resetTimer -= 1;
            }
        }
        else
        {
            GlobalTimer -= Time.deltaTime;
        }
    }
    void remixFloor()
    {
        for (int x = 0; x < 12; x++)
        {
            for (int y = 0; y < 12; y++)
            {
                BoxScript boxScript = gridFloor[x, y].GetComponent<BoxScript>();
                boxScript.isAlive = (Random.value > 0.5);
            }
        }
    }
    void changeState()
    {
        bool[,] nextState = new bool[12, 12];
        for (int x = 0; x < 12; x++)
        {
            for (int y = 0; y < 12; y++)
            {
                int neighbors = countNeighbors(x, y);
                BoxScript currentState = gridFloor[x, y].GetComponent<BoxScript>();
                if (currentState.isAlive && neighbors < 2)
                {
                    nextState[x, y] = false;
                }
                else if (currentState.isAlive && (neighbors == 2 || neighbors == 3))
                {
                    nextState[x, y] = true;
                }
                else if (currentState.isAlive && neighbors > 3)
                {
                    nextState[x, y] = false;
                }
                else if (!currentState.isAlive && neighbors == 3)
                {
                    nextState[x, y] = true;
                }
            }
        }
        for (int x = 0; x < 12; x++)
        {
            for (int y = 0; y < 12; y++)
            {
                BoxScript newState = gridFloor[x, y].GetComponent<BoxScript>();
                newState.isAlive = nextState[x, y];
                newState.setColor();
            }
        }
    }

}
