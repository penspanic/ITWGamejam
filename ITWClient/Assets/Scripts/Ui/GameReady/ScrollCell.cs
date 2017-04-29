using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum ScrollDir
{
    Up,
    Down
}

public enum ScrollCellType {
    Player,
    Team,
    Diff,
}   


public class ScrollCell : MonoBehaviour {

    public ScrollCellType scrollCellType;

    private float scrollTime;
    private List<Transform> cellContentList;
    private float cellHeight;
    private bool isChanging;
    private int currIdx;
    private RectTransform content;


    public virtual void InitScrollCell(int startIdx, PlayerCellType plType, float scroll = 0.4f) {
        Rect trsRect = transform.GetComponent<RectTransform>().rect;
        int contentCnt = transform.FindChild("Content").childCount;

        cellContentList = new List<Transform>();
        scrollTime = scroll;
        cellHeight = trsRect.height;
        isChanging = false;
        currIdx = 0;

        content = transform.FindChild("Content").GetComponent<RectTransform>();
        for (int i = 0; i < contentCnt; ++i)
        {
            cellContentList.Add(content.GetChild(i));
            cellContentList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, cellHeight * -i);
        }


    }



    public void OnValueChangedCell(ScrollDir dir)
    {
        
        if (dir == ScrollDir.Up)
        {
            if (currIdx == 0)
            {
                return;
            }


            --currIdx;
            content.DOLocalMoveY(currIdx * cellHeight, scrollTime).SetEase(Ease.OutBack);
        }
        else if (dir == ScrollDir.Down)
        {
            if (currIdx >= cellContentList.Count - 1)
            {
                return;
            }

            ++currIdx;
            content.DOLocalMoveY(currIdx * cellHeight, scrollTime).SetEase(Ease.OutBack);
        }
    }


}
