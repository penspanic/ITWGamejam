using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GameInfoRaw : MonoBehaviour {
    [SerializeField]
    private GameInfoCell[] infoCells;
    [SerializeField]
    private GameInfoCell[] cpuMode;
    [SerializeField]
    private UnityEngine.UI.Text titleText;
    [SerializeField]
    private bool isBackRaw;

    private List<GameInfoCell> infoCellList;
    private UiGameReadyController uiController;
//    [SerializeField]
//    private string[] infos;

    private int currIdx;
    private bool isSelected = false;

    public void InitGameInfoRaw(int infoCnt = 99) 
    {
        uiController = FindObjectOfType<UiGameReadyController>();
        currIdx = 0;
        infoCellList = new List<GameInfoCell>();

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
            for (int i = 0; i < infoCnt; ++i)
            {
                infoCellList.Add(infoCells[i]);
            }
        }
        Debug.Log(name + "'s infoCellListCnt: " + infoCellList.Count);

        for (int i = 0; i < infoCellList.Count; ++i)
        {
            infoCellList[i].gameObject.SetActive(true);
        }

    }

    public void SetSelected(bool isSelect) 
    {
        isSelected = isSelect;
        if (isSelected == true)
        {
            SetCursorByCurrIdx();
        }
    }

    public bool InitCPUCols(GameMode gameMode, HowPlayer howPlayer)
    {
        currIdx = 0;
        switch (gameMode)
        {
            case GameMode.Personal:
                if (howPlayer == HowPlayer.P1)
                    InitGameInfoRaw(4);
                else if (howPlayer == HowPlayer.P2)
                    InitGameInfoRaw(3);

                if (titleText != null)
                {
                    titleText.text = "CPU 숫자";
                }

                // SetCursorByCurrIdx();
                return true;
            case GameMode.Team:
                Debug.Log("TEam");
                InitGameInfoRaw();
                UpdateCpuModeCols();

                if (titleText != null)
                {
                    titleText.text = "모드선택";
                }
                return false;
        }
        return true;
    }

    private void UpdateCpuModeCols() 
    {
        Debug.Log(infoCellList.Count);
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
        Debug.Log("Selected: " + name);

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

        for (int i = 0; i < infoCellList.Count; ++i)
        {
            Debug.Log("[InfoCell] pos : " + infoCellList[i].transform.position);
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
