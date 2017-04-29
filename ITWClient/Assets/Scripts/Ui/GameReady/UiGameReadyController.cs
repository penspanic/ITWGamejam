using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UiGameReadyController : Singleton<UiGameReadyController> {
    [SerializeField]
    private PlayerCell[] raws;
    [SerializeField]
    private GameObject lastRaw;
    [SerializeField]
    private int maxCols;
    [SerializeField]
    private Transform cursor;

    private ScrollCell[,] scrollCellArr;
    private int currX, currY;
    // private TeamData

    void Awake()
    {
        InitScrollCellArr();
    }

    private void InitScrollCellArr() {
        scrollCellArr = new ScrollCell[raws.Length + 1, maxCols];
        for (int i = 0; i < raws.Length; ++i)
        {
            raws[i].InitCell();
            var cells = raws[i].GetScrollCells();

            for (int j = 0; j < cells.Length; ++j)
            {
                scrollCellArr[i, j] = cells[j];
            }

//            if (i == raws.Length - 1)
//            {
//                scrollCellArr[i, 2] = cells[1];
//            }
        }

        var scrolls = lastRaw.GetComponentsInChildren<ScrollCell>();
        for (int i = 0; i < scrolls.Length; ++i)
        {
            scrolls[i].InitScrollCell(0, PlayerCellType.None);
            scrollCellArr[raws.Length, i] = scrolls[i];
        }
        scrollCellArr[raws.Length, scrolls.Length] = scrolls[1];


        currX = currY = 0;
    }
        


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Up Arrow..

            scrollCellArr[currY, currX].OnValueChangedCell(ScrollDir.Up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Down Arrow..
            scrollCellArr[currY, currX].OnValueChangedCell(ScrollDir.Down);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currY <= 0)
            {
                return;
            }
            --currY;
            SetCursorByCurrPos();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("scrollCellArr.GetLength(0): " + scrollCellArr.GetLength(0));
            Debug.Log("currY: " + currY);
            if (currY >= scrollCellArr.GetLength(0) - 1)
            {
                return;
            }
            ++currY;
            SetCursorByCurrPos();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("scrollCellArr.GetLength(1): " + scrollCellArr.GetLength(1));
            Debug.Log("currX: " + currX);
            if (currX >= scrollCellArr.GetLength(1) - 1)
            {
                return;
            }
            ++currX;
            SetCursorByCurrPos();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currX <= 0)
            {
                return;
            }
            --currX;
            SetCursorByCurrPos();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currY < scrollCellArr.GetLength(0)- 1)
            {
                return;
            }

            if (currX == 0)
            {
                OnPressedDone();
            }
            else
            {
                OnPressedExit();
            }


        }
    }

    public void OnPressedDone() {
        // check raws..

    }

    public void OnPressedExit() {
    }

    private void SetCursorByCurrPos() {
        Vector2 newPos = scrollCellArr[currY, currX].transform.position;
        Debug.Log(newPos.x);
        newPos *= 100;
        cursor.localPosition = newPos;
    }



}
