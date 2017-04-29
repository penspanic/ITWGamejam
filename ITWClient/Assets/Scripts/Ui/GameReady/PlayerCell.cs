using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerCellType
{
    None,
    Player,
    CPU,
}

public class PlayerCell : MonoBehaviour {
    public PlayerCellType cellType;

    private ScrollCell[] cells; // pl, team, diff 
    //private PlayerCellType currCellType;


    public void InitCell()
    {
        cells = GetComponentsInChildren<ScrollCell>();

        for (int i = 0; i < cells.Length; ++i)
        {
            cells[i].InitScrollCell(0, cellType);
        }

    }

    public ScrollCell[] GetScrollCells() {
        return cells;
    }

    public bool IsCorrectCell() {
        return true;
    }


}
