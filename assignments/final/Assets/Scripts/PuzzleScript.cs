using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleScript : MonoBehaviour
{
    public bool isLoaded = false;
    public CellScript[,] puzzleCells;
    CellScript currentCell;
    public int yHeight;
    public int xHeight;
    int currentPosX;
    int currentPosY;
    int direction = 0;
    int cellCount = 0;
    bool isSolved = false;
    int mirrorPosX;
    int mirrorPosY;
    GameManager manager;
    CellScript mirrorCell;
    Color defaultMirror;
    Color defaultStarting;
    // Start is called before the first frame update
    private void OnEnable()
    {
        puzzleCells = new CellScript[yHeight, xHeight];
    }
    void Start()
    {
        manager = GameManager.instance;
        for (int i = 0; i < yHeight; i++)
        {
            for (int j = 0; j < xHeight; j++)
            {
                if (puzzleCells[i, j] != null)
                {
                    cellCount++;
                    if (puzzleCells[i, j].cellType == 1)
                    {
                        currentCell = puzzleCells[i, j];
                        currentPosX = j;
                        currentPosY = i;
                        defaultStarting = Color.white;
                    }
                    else if (puzzleCells[i, j].cellType == 6)
                    {
                        mirrorCell = puzzleCells[i, j];
                        mirrorPosX = j;
                        mirrorPosY = i;
                        defaultMirror = new Color(1f, 0.6578f, 0f, 1);
                    }
                }

            }
        }
    }
    /* 
     Direction inputs
    W (up) = 0
    D (right) = 3
    S (down) = 2
    A (left) = 1
     */
    // Update is called once per frame
    void Update()
    {
        if (isLoaded && !isSolved)
        {
            currentPosX = currentCell.cellColumn;
            currentPosY = currentCell.cellRow;
            if (mirrorCell == null)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    direction = 3;
                    if (currentPosX + 1 < xHeight)
                    {
                        if (puzzleCells[currentPosY, currentPosX + 1] != null)
                        {
                            if (puzzleCells[currentPosY, currentPosX + 1].checkAvailability(direction))
                            {
                                CellScript tempCell = currentCell;
                                currentPosX += 1;
                                currentCell = puzzleCells[currentPosY, currentPosX].changingCellLocation(defaultStarting);
                                currentCell.setPrevCell(tempCell);
                                callAction();
                            }
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    direction = 2;
                    if (currentPosY - 1 >= 0)
                    {
                        if (puzzleCells[currentPosY - 1, currentPosX] != null)
                        {
                            if (puzzleCells[currentPosY - 1, currentPosX].checkAvailability(direction))
                            {
                                CellScript tempCell = currentCell;
                                currentPosY -= 1;
                                currentCell = puzzleCells[currentPosY, currentPosX].changingCellLocation(defaultStarting);
                                currentCell.setPrevCell(tempCell);
                                callAction();
                            }
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    direction = 1;
                    if (currentPosX - 1 >= 0)
                    {
                        if (puzzleCells[currentPosY, currentPosX - 1] != null)
                        {
                            if (puzzleCells[currentPosY, currentPosX - 1].checkAvailability(direction))
                            {
                                CellScript tempCell = currentCell;
                                currentPosX -= 1;
                                currentCell = puzzleCells[currentPosY, currentPosX].changingCellLocation(defaultStarting);
                                currentCell.setPrevCell(tempCell);
                                callAction();
                            }
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    direction = 0;
                    if (currentPosY + 1 < yHeight)
                    {
                        if (puzzleCells[currentPosY + 1, currentPosX] != null)
                        {
                            if (puzzleCells[currentPosY + 1, currentPosX].checkAvailability(direction))
                            {
                                CellScript tempCell = currentCell;
                                currentPosY += 1;
                                currentCell = puzzleCells[currentPosY, currentPosX].changingCellLocation(defaultStarting);
                                currentCell.setPrevCell(tempCell);
                                callAction();
                            }
                        }
                    }
                }
            }
            else
            {
                mirrorPosY = mirrorCell.cellRow;
                mirrorPosX = mirrorCell.cellColumn;
                if (Input.GetKeyDown(KeyCode.D))
                {
                    direction = 3;
                    if ((currentPosX + 1 < xHeight) && (mirrorPosX - 1 >= 0))
                    {
                        if (puzzleCells[currentPosY, currentPosX + 1] != null && puzzleCells[mirrorPosY, mirrorPosX - 1] != null)
                        {
                            if (puzzleCells[currentPosY, currentPosX + 1].checkAvailability(direction) && puzzleCells[mirrorPosY, mirrorPosX - 1].checkAvailability(1) &&!(currentPosY == mirrorPosY && currentPosX + 1 == mirrorPosX - 1))
                            {
                                CellScript tempMirrorCell = mirrorCell;
                                CellScript tempCell = currentCell;
                                currentPosX += 1;
                                mirrorPosX -= 1;
                                currentCell = puzzleCells[currentPosY, currentPosX].changingCellLocation(defaultStarting);
                                mirrorCell = puzzleCells[mirrorPosY, mirrorPosX].changingCellLocation(defaultMirror);
                                if (mirrorCell == currentCell)
                                {
                                    currentCell.undoCell();
                                    mirrorCell = tempMirrorCell;
                                    currentCell = tempCell;
                                }
                                else
                                {
                                    mirrorCell.setPrevCell(tempMirrorCell);
                                    currentCell.setPrevCell(tempCell);
                                    callAction();
                                }
                            }
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    direction = 2;
                    if (currentPosY - 1 >= 0 && (mirrorPosY + 1 < yHeight))
                    {
                        if (puzzleCells[currentPosY - 1, currentPosX] != null && puzzleCells[mirrorPosY + 1, mirrorPosX] != null)
                        {
                            if (puzzleCells[currentPosY - 1, currentPosX].checkAvailability(direction) && puzzleCells[mirrorPosY + 1, mirrorPosX].checkAvailability(0) && !(currentPosY -1 == mirrorPosY +1 && currentPosX == mirrorPosX))
                            {
                                CellScript tempMirrorCell = mirrorCell;
                                CellScript tempCell = currentCell;
                                currentPosY -= 1;
                                mirrorPosY += 1;
                                mirrorCell = puzzleCells[mirrorPosY, mirrorPosX].changingCellLocation(defaultMirror);
                                currentCell = puzzleCells[currentPosY, currentPosX].changingCellLocation(defaultStarting);
                                if (mirrorCell == currentCell)
                                {
                                    currentCell.undoCell();
                                    mirrorCell = tempMirrorCell;
                                    currentCell = tempCell;
                                }
                                else
                                {
                                    mirrorCell.setPrevCell(tempMirrorCell);
                                    currentCell.setPrevCell(tempCell);
                                    callAction();
                                }
                            }
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    direction = 1;
                    if ((mirrorPosX + 1 < xHeight) && (currentPosX - 1 >= 0))
                    {
                        if (puzzleCells[currentPosY, currentPosX - 1] != null && puzzleCells[mirrorPosY, mirrorPosX + 1] != null)
                        {
                            if (puzzleCells[currentPosY, currentPosX - 1].checkAvailability(direction) && puzzleCells[mirrorPosY, mirrorPosX + 1].checkAvailability(3) && !(currentPosY == mirrorPosY && currentPosX - 1 == mirrorPosX + 1))
                            {
                                CellScript tempMirrorCell = mirrorCell;
                                CellScript tempCell = currentCell;
                                currentPosX -= 1;
                                mirrorPosX += 1;
                                currentCell = puzzleCells[currentPosY, currentPosX].changingCellLocation(defaultStarting);
                                mirrorCell = puzzleCells[mirrorPosY, mirrorPosX].changingCellLocation(defaultMirror);
                                if (mirrorCell == currentCell)
                                {
                                    currentCell.undoCell();
                                    mirrorCell = tempMirrorCell;
                                    currentCell = tempCell;
                                }
                                else
                                {
                                    mirrorCell.setPrevCell(tempMirrorCell);
                                    currentCell.setPrevCell(tempCell);
                                    callAction();
                                }
                            }
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    direction = 0;
                    if (currentPosY + 1 < yHeight && (mirrorPosY - 1 >= 0))
                    {
                        if (puzzleCells[currentPosY + 1, currentPosX] != null && puzzleCells[mirrorPosY - 1, mirrorPosX] != null)
                        {
                            if (puzzleCells[currentPosY + 1, currentPosX].checkAvailability(direction) && puzzleCells[mirrorPosY - 1, mirrorPosX].checkAvailability(2) && !(currentPosY + 1 == mirrorPosY - 1 && currentPosX == mirrorPosX))
                            {
                                CellScript tempMirrorCell = mirrorCell;
                                CellScript tempCell = currentCell;
                                currentPosY += 1;
                                mirrorPosY -= 1;
                                mirrorCell = puzzleCells[mirrorPosY, mirrorPosX].changingCellLocation(defaultMirror);
                                currentCell = puzzleCells[currentPosY, currentPosX].changingCellLocation(defaultStarting);
                                if (mirrorCell == currentCell)
                                {
                                    currentCell.undoCell();
                                    mirrorCell = tempMirrorCell;
                                    currentCell = tempCell;
                                } else
                                {
                                    mirrorCell.setPrevCell(tempMirrorCell);
                                    currentCell.setPrevCell(tempCell);
                                    callAction();
                                }
                                
                            }
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                undoOneMove();
            }
            else if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1))
            {
                resetBoard();
            }
        }
    }
    void callAction()
    {
        if (currentCell != null)
        {
            if (mirrorCell == null)
            {
                if (currentCell.cellType == 2)
                {
                    int filledCells = 0;
                    for (int i = 0; i < yHeight; i++)
                    {
                        for (int j = 0; j < xHeight; j++)
                        {
                            if (puzzleCells[i, j] != null)
                            {
                                if (puzzleCells[i, j].isFilled)
                                {
                                    filledCells++;
                                }
                            }
                        }
                    }
                    if (filledCells == cellCount)
                    {
                        for (int i = 0; i < yHeight; i++)
                        {
                            for (int j = 0; j < xHeight; j++)
                            {
                                if (puzzleCells[i, j] != null)
                                {
                                    puzzleCells[i, j].fillCell(Color.green);
                                }
                            }
                        }
                        //puzzle solved
                        isSolved = true;
                        manager.userBeatAPuzzle();
                    }
                }
            }
            else
            {
                if (currentCell.cellType == 2 || mirrorCell.cellType == 2)
                {
                    int filledCells = 0;
                    for (int i = 0; i < yHeight; i++)
                    {
                        for (int j = 0; j < xHeight; j++)
                        {
                            if (puzzleCells[i, j] != null)
                            {
                                if (puzzleCells[i, j].isFilled)
                                {
                                    filledCells++;
                                }
                            }
                        }
                    }
                    if (filledCells == cellCount)
                    {
                        for (int i = 0; i < yHeight; i++)
                        {
                            for (int j = 0; j < xHeight; j++)
                            {
                                if (puzzleCells[i, j] != null)
                                {
                                    puzzleCells[i, j].fillCell(Color.green);
                                }
                            }
                        }
                        //puzzle solved
                        isSolved = true;
                        manager.userBeatAPuzzle();
                    }
                }
            }
        }
    }

    void undoOneMove()
    {
        if (currentCell != null)
        {
            if (mirrorCell == null)
            {
                if (currentCell.cellType != 1)
                {
                    currentCell.undoCell();
                    currentCell = currentCell.prevCell;
                }
            }
            else
            {
                if (currentCell.cellType != 1)
                {
                    mirrorCell.undoCell();
                    mirrorCell = mirrorCell.prevCell;
                    currentCell.undoCell();
                    currentCell = currentCell.prevCell;
                }
            }
        }
    }
    void resetBoard()
    {
        for (int i = 0; i < yHeight; i++)
        {
            for (int j = 0; j < xHeight; j++)
            {
                if (puzzleCells[i, j] != null)
                {
                    if (puzzleCells[i, j].cellType == 1)
                    {
                        currentCell = puzzleCells[i, j];
                    }
                    else if (puzzleCells[i, j].cellType == 6)
                    {
                        mirrorCell = puzzleCells[i, j];
                    }
                    puzzleCells[i, j].undoCell();
                }
            }
        }
    }

}
