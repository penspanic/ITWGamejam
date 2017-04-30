using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameInfoState : short
{
    SelectPlayer,
    SelectMode,
    SelectCPU,
    SelectCheck,
}

public enum HowPlayer : short
{
    P1,
    P2,
}

public enum PlayerTeamVersus
{
    None,
    PL2vsCP2,
    PLCPvsPLCP,
}

public enum GameMode : short
{
    Personal,
    Team
}

public class GameInfoReady : MonoBehaviour {
    [SerializeField]
    private GameInfoRaw selectPlayer;
    [SerializeField]
    private GameInfoRaw selectMode;
    [SerializeField]
    private GameInfoRaw selectCPU;
    [SerializeField]
    private GameInfoRaw selectCheck;

    private GameInfoState gameInfoState;
    private Dictionary<GameInfoState, GameInfoRaw> selectDic;
    private UiGameReadyController uiGameReady;
    private bool isDoneRaw = false;
    private bool canGetCpuCount = false;
    private bool isAllCheck = false;
    private bool isDone = false;

    // SaveData


    public void InitGameInfoReady() 
    {
        uiGameReady = FindObjectOfType<UiGameReadyController>();
        selectDic = new Dictionary<GameInfoState, GameInfoRaw>();
        selectDic.Add(GameInfoState.SelectPlayer, selectPlayer);
        selectDic.Add(GameInfoState.SelectMode, selectMode);
        selectDic.Add(GameInfoState.SelectCPU, selectCPU);

        gameInfoState = GameInfoState.SelectPlayer;
        selectPlayer.InitGameInfoRaw();
        selectMode.InitGameInfoRaw();
        // selectCPU.InitGameInfoRaw();
        selectCheck.InitGameInfoRaw();
        selectPlayer.SetSelected(true);
        isDoneRaw = false;
        isAllCheck = false;
    }



    void Update()
    {
        if (uiGameReady.gameReadyState != GameReadyState.SelectInfo)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Todo.
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isDoneRaw == true)
            {
                // done or quit
                switch (selectCheck.GetCurrIdx())
                {
                    case 0:
                        // back
                        break;
                    case 1:
                        if (isAllCheck == true)
                        {
                            selectCheck.SetSelected(false);
                            selectCheck.gameObject.SetActive(false);
                            uiGameReady.SetGameReadyState(GameReadyState.SelectCharacter);
                        }
                        break;
                    default:
                        break;
                }
                return;

            }
            switch (gameInfoState)
            {
                case GameInfoState.SelectPlayer:
                    selectPlayer.SelectByCurrIdx();
                    uiGameReady.howPlayer = (HowPlayer)selectPlayer.GetCurrIdx();
                    ++gameInfoState;
                    selectPlayer.SetSelected(false);
                    selectMode.SetSelected(true);
                    break;
                case GameInfoState.SelectMode:
                    selectMode.SelectByCurrIdx();
                    uiGameReady.gameMode = (GameMode)selectMode.GetCurrIdx();
                    ++gameInfoState;
                    selectMode.SetSelected(false);

                    if (IsSoloTeam(uiGameReady.gameMode, uiGameReady.howPlayer))
                    {
                        gameInfoState = GameInfoState.SelectCheck;
                        uiGameReady.cpuCount = 3;
                        selectCheck.SetSelected(true);
                        isDoneRaw = true;
                        isAllCheck = true;
                        return;
                    }
                    canGetCpuCount = selectCPU.InitCPUCols(uiGameReady.gameMode, uiGameReady.howPlayer);
                    selectCPU.SetSelected(true);
                    break;
                case GameInfoState.SelectCPU:
                    selectCPU.SelectByCurrIdx();
                    var selectIdx = selectCPU.GetCurrIdx(); // if canGetCputCnt true -> pvc, pvp Idx
                    if (canGetCpuCount == true)
                    {
                        if (uiGameReady.howPlayer == HowPlayer.P1)
                        {
                            uiGameReady.cpuCount = selectIdx + 1;
                        }
                        else
                        {
                            uiGameReady.cpuCount = selectIdx;
                        }

                    }
                    else
                    {
                        uiGameReady.cpuCount = 2;
                        if (selectIdx == 0)
                        {
                            uiGameReady.versusMode = PlayerTeamVersus.PL2vsCP2;
                        }
                        else if (selectIdx == 1)
                        {
                            uiGameReady.versusMode = PlayerTeamVersus.PLCPvsPLCP;
                        }
                    }
                    ++gameInfoState;
                    selectCPU.SetSelected(false);
                    selectCheck.SetSelected(true);
                    isAllCheck = true;
                    isDoneRaw = true;
                    break;
            }

            if ((int)gameInfoState <= (int)GameInfoState.SelectMode)
            {
                selectDic[gameInfoState].InitGameInfoRaw();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gameInfoState != GameInfoState.SelectCheck)
            {
                selectDic[gameInfoState].SetSelected(false);
                selectCheck.SetSelected(true);
                isDoneRaw = true;
            }

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            
            if (gameInfoState != GameInfoState.SelectCheck)
            {
                selectCheck.BackRawSelectByCurrIdx(true);
                selectDic[gameInfoState].SetSelected(true);
                selectCheck.SetSelected(false);
                isDoneRaw = false;
            }

        }
    }

    private bool IsSoloTeam(GameMode gm, HowPlayer howPlay) {
        if (gm == GameMode.Team && howPlay == HowPlayer.P1)
        {
            return true;
        }
        return false;
    }

}
