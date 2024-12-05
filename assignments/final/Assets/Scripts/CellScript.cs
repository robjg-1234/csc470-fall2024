using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{
    public int cellRow = 0;
    public int cellColumn = 0;
    public Renderer rend;
    public int cellType;
    PuzzleScript parentScript;
    public bool isFilled = false;
    public CellScript prevCell;
    CellScript targetCell;
    public Color defaultColor;
    int timesPassed = 0;
    public List<CellScript> storedCells = new List<CellScript>();
    // Start is called before the first frame update
    private void OnEnable()
    {
        parentScript = GetComponentInParent<PuzzleScript>();
        parentScript.puzzleCells[cellRow, cellColumn] = this;
    }
    void Start()
    {

        if (cellType == 0)
        {
            //Default empty cells
            rend.material.color = Color.gray;
        }
        else if (cellType == 1)
        {
            //Starting Cell
            rend.material.color = Color.white;
            isFilled = true;
        }
        else if (cellType == 2)
        {
            //Ending Cell
            rend.material.color = Color.yellow;
        }
        else if (cellType == 3)
        {
            //Teleport Cell
            rend.material.color = Color.blue;
            for (int i = 0; i < parentScript.yHeight; i++)
            {
                for (int j = 0; j < parentScript.xHeight; j++)
                {
                    if (parentScript.puzzleCells[i, j] != null)
                    {
                        CellScript tempCell = parentScript.puzzleCells[i, j];
                        if (tempCell.cellType == 3 && tempCell != this)
                        {
                            targetCell = tempCell;
                            break;
                        }
                    }
                }
            }
        }
        else if (cellType == 4)
        {
            //Right Direction Cell
            rend.material.color = Color.cyan;
        }
        else if (cellType == 5)
        {
            //Bridge Cell
            rend.material.color = Color.magenta;
        }
        else if (cellType == 6)
        {
            //Mirror Cell
            rend.material.color = new Color(1f, 0.6578f, 0f, 1);
            isFilled = true;
        }
        defaultColor = rend.material.color;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setPrevCell(CellScript cell)
    {
        prevCell = cell;
    }
    public bool checkAvailability(int direction)
    {
        bool availability = true;
        if (isFilled)
        {
            availability = false;
        }
        else if (!cellAbility(direction))
        {
            availability = false;
        }
        return availability;
    }
    bool cellAbility(int dir)
    {
        bool isAbilityUsable = false;
        if (cellType == 4)
        {

            if (dir == 0)
            {
                if (parentScript?.xHeight > cellColumn + 1)
                {
                    CellScript tempCell = parentScript?.puzzleCells[cellRow, cellColumn + 1];
                    if (tempCell != null)
                    {
                        if (tempCell.checkAvailability(3))
                        {
                            targetCell = tempCell;
                            isAbilityUsable = true;
                        }
                    }
                }
            }
            else if (dir == 1)
            {
                if (parentScript?.yHeight > cellRow + 1)
                {
                    CellScript tempCell = parentScript?.puzzleCells[cellRow + 1, cellColumn];
                    if (tempCell != null)
                    {
                        if (tempCell.checkAvailability(0))
                        {
                            targetCell = tempCell;
                            isAbilityUsable = true;
                        }
                    }
                }
            }
            else if (dir == 2)
            {
                if (0 <= cellColumn - 1)
                {
                    CellScript tempCell = parentScript?.puzzleCells[cellRow, cellColumn - 1];
                    if (tempCell != null)
                    {
                        if (tempCell.checkAvailability(1))
                        {
                            targetCell = tempCell;
                            isAbilityUsable = true;
                        }
                    }
                }
            }
            else if (dir == 3)
            {
                if (0 <= cellRow - 1)
                {
                    CellScript tempCell = parentScript?.puzzleCells[cellRow - 1, cellColumn];
                    if (tempCell != null)
                    {
                        if (tempCell.checkAvailability(2))
                        {
                            targetCell = tempCell;
                            isAbilityUsable = true;
                        }
                    }
                }
            }
            if (targetCell != null)
            {
                targetCell.storedCells.Add(this);
            }
        }
        else if (cellType == 5)
        {
            if (dir == 0)
            {
                if (parentScript?.yHeight > cellRow + 1)
                {
                    CellScript tempCell = parentScript?.puzzleCells[cellRow + 1, cellColumn];
                    if (tempCell != null)
                    {
                        if (tempCell.checkAvailability(dir))
                        {
                            targetCell = tempCell;
                            isAbilityUsable = true;
                        }
                    }
                }
            }
            else if (dir == 1)
            {
                if (0 <= cellColumn - 1)
                {
                    CellScript tempCell = parentScript?.puzzleCells[cellRow, cellColumn - 1];
                    if (tempCell != null)
                    {
                        if (tempCell.checkAvailability(dir))
                        {
                            targetCell = tempCell;
                            isAbilityUsable = true;
                        }
                    }
                }
            }
            else if (dir == 2)
            {
                if (0 <= cellRow - 1)
                {
                    CellScript tempCell = parentScript?.puzzleCells[cellRow - 1, cellColumn];
                    if (tempCell != null)
                    {
                        if (tempCell.checkAvailability(dir))
                        {
                            targetCell = tempCell;
                            isAbilityUsable = true;
                        }
                    }
                }
            }
            else if (dir == 3)
            {
                if (parentScript?.xHeight > cellColumn + 1)
                {
                    CellScript tempCell = parentScript?.puzzleCells[cellRow, cellColumn + 1];
                    if (tempCell != null)
                    {
                        if (tempCell.checkAvailability(dir))
                        {
                            targetCell = tempCell;
                            isAbilityUsable = true;
                        }
                    }
                }
            }
            if (targetCell != null)
            {
                targetCell.storedCells.Add(this);
            }
        }
        else
        {
            isAbilityUsable = true;
        }
        return isAbilityUsable;
    }
    public CellScript changingCellLocation(Color attachedColor)
    {
        if (targetCell == null)
        {
            targetCell = this;
        }
        if (cellType == 5)
        {
            if (timesPassed < 1)
            {
                timesPassed += 1;
                targetCell.fillCell(attachedColor);
                return targetCell;
            }
            else
            {
                timesPassed += 1;
                fillCell(attachedColor);
                targetCell.fillCell(attachedColor);
                return targetCell;
            }
            
        }
        else
        {
            fillCell(attachedColor);
            targetCell.fillCell(attachedColor);
            return targetCell;
        }
    }
    public void fillCell(Color colorOfCell)
    {
        isFilled = true;
        rend.material.color = colorOfCell;
    }
    public void undoCell()
    {
        if (targetCell == null)
        {
            targetCell = this;
        }
        backToDefault();
        targetCell.backToDefault();

    }
    public void backToDefault()
    {
        if (cellType != 1)
        {
            isFilled = false;
            rend.material.color = defaultColor;
            if (storedCells.Count > 0)
            {
                for (int i = 0; i < storedCells.Count; i++)
                {
                    storedCells[i].backToDefault();
                }
            }
            storedCells.Clear();
            if (cellType == 5)
            {
                if (timesPassed > 0)
                {
                    timesPassed -= 1;
                }
            }
        }
    }
}
