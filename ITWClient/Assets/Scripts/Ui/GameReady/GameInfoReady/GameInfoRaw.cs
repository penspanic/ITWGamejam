using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GameInfoRaw : MonoBehaviour {
    [SerializeField]
    private GameInfoState infoType;
    [SerializeField]
    private GameInfoCell[] infoCells;
    [SerializeField]
    private GameInfoCell[] cpuMode;
    [SerializeField]
    private GameObject titleObj;
    [SerializeField]
    private bool isBackRaw;

    private List<GameInfoCell> infoCellList;
    private UiGameReadyController uiController;

    private int currIdx;
    private bool isSelected = false;

    public void InitGameInfoRaw(int infoCnt = 99) 
    {
        uiController = FindObjectOfType<UiGameReadyController>();
        currIdx = 0;
        infoCellList = new List<GameInfoCell>();

        InitByInfoType();

        if (infoType != GameInfoState.AllSelectDone)
        {
            gameObject.SetActive(false);
        }
    }

    private void InitByInfoType()
    {
        transform.localPosition = new Vector2(1280f, transform.localPosition.y);
        switch (infoType)
        {
            case GameInfoState.SelectPlayer:
                break;
            case GameInfoState.SelectMode:
                break;
            case GameInfoState.SelectCPU:
                break;
            case GameInfoState.AllSelectDone:
                transform.localPosition = new Vector2(0f, transform.localPosition.y);
                break;
            default:
                break;
        }
    }
    public void UpdateDataByInfoType(int infoCnt = 99)
    {
        currIdx = 0;
        switch (infoType)
        {
            case GameInfoState.SelectPlayer:
                break;
            case GameInfoState.SelectMode:
                break;
            case GameInfoState.SelectCPU:
                break;
            case GameInfoState.AllSelectDone:
                currIdx = 1;
                break;
            default:
                break;
        }
        
        for (int i = 0; i < infoCells.Length; ++i)
        {
            infoCells[i].gameObject.SetActive(false);
        }
        if (infoCnt == 99)
        {
            infoCellList.AddRange(infoCells);
        }
        else
        {
            int j = uiController.howPlayer == HowPlayer.P1 ? 1 : 0;
            for (; j < infoCnt; ++j)
            {
                infoCellList.Add(infoCells[j]);
            }
        }
        for (int i = 0; i < infoCellList.Count; ++i)
        {
            infoCellList[i].gameObject.SetActive(true);
        }

    }



    public void SetSelected(bool isSelect, bool setCursor = false, int startIdx = 99) 
    {
        isSelected = isSelect;
        if (isSelected == true)
        {
            if (startIdx != 99)
            {
                currIdx = startIdx;
            }

            if(setCursor == true) {
                SetCursorByCurrIdx();
            }
        }
    }

    public bool InitCPUCols(GameMode gameMode, HowPlayer howPlayer)
    {
        Debug.Log("GameMode: " + gameMode.ToString());
        Debug.Log("HowPlayer:" + howPlayer.ToString());
        currIdx = 0;
        switch (gameMode)
        {
            case GameMode.Personal:
                if (howPlayer == HowPlayer.P1)
                    UpdateDataByInfoType(4);
                else if (howPlayer == HowPlayer.P2)
                    UpdateDataByInfoType(3);

                if (titleObj != null)
                {
                    titleObj.SetActive(true);
                }

                // SetCursorByCurrIdx();
                return true;
            case GameMode.Team:
                UpdateDataByInfoType();
                UpdateCpuModeCols();

                if (titleObj != null)
                {
                    titleObj.SetActive(false);
                }
                return false;
        }
        return true;
    }

    private void UpdateCpuModeCols() 
    {
        for (int i = 0; i < infoCellList.Count; ++i)
        {
            infoCellList[i].gameObject.SetActive(false);
        }


        infoCellList.Clear();

        for (int i = 0; i < cpuMode.Length; ++i)
        {
            cpuMode[i].gameObject.SetActive(true);
            infoCellList.Add(cpuMode[i]);
        }
    }
	
	void Update () 
    {
        if (isSelected == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currIdx <= 0)
            {
                return;
            }
            --currIdx;
            SetCursorByCurrIdx();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currIdx >= infoCellList.Count - 1)
            {
                return;
            }
            ++currIdx;
            SetCursorByCurrIdx();
        }
	}

    public void SelectByCurrIdx()
    {
        infoCellList[currIdx].SelectCell(true);
    }

    public int GetCurrIdx()
    {
        return currIdx;
    }

    public void SetCursorByCurrIdx()
    {
        if (isBackRaw)
        {
            BackRawSelectByCurrIdx();
            return;
        }

        Invoke("DelaySetCursor", 0.05f);

    }

    private void DelaySetCursor() 
    {
        uiController.SetCursor(infoCellList[currIdx].transform.position);
    }

    public void BackRawSelectByCurrIdx(bool reset = false) {
        for (int i = 0; i < infoCellList.Count; ++i)
        {
            if (currIdx == i && reset == false)
            {
                infoCellList[i].SelectCell(true);
            }
            else
            {
                infoCellList[i].SelectCell(false);
            }
        }
    }
}
