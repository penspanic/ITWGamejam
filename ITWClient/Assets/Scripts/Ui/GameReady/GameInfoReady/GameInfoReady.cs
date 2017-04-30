using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField]
    private float infoRawAniTime;

    private GameInfoState gameInfoState;
    private Dictionary<GameInfoState, GameInfoRaw> selectDic;
    private Vector2[] oriPosRaws;
    private UiGameReadyController uiGameReady; 
    private bool isInit = false;
    private bool isDoneRaw = false;
    private bool canGetCpuCount = false;
    private bool isAllCheck = false;
    private bool isDone = false;
    //private bool isAnimating = false;

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

        isDoneRaw = false;
        isAllCheck = false;
        isInit = true;

        selectPlayer.transform.localPosition = new Vector2(1280f, selectPlayer.transform.localPosition.y);
        selectMode.transform.localPosition = new Vector2(1280, selectMode.transform.localPosition.y);
        selectCPU.transform.localPosition = new Vector2(1280, selectCPU.transform.localPosition.y);
        selectPlayer.gameObject.SetActive(false);
        selectMode.gameObject.SetActive(false);
        selectCPU.gameObject.SetActive(false);

        //isAnimating = true;
        MoveRightToCenter(selectPlayer);
        
    }

    private void MoveRightToCenter(GameInfoRaw target) 
    {
        target.gameObject.SetActive(true);
        target.transform.DOLocalMoveX(0f, infoRawAniTime).OnComplete(() =>
            {
                //isAnimating = false;
                target.SetSelected(true);
            });
    }

    void Update()
    {
        if (isInit == false)
        {
            return;
        }
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
                    MoveRightToCenter(selectMode);
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
                        selectCheck.SetSelected(true, true, 1);
                        isDoneRaw = true;
                        isAllCheck = true;
                        return;
                    }
                    canGetCpuCount = selectCPU.InitCPUCols(uiGameReady.gameMode, uiGameReady.howPlayer);
                    MoveRightToCenter(selectCPU);
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
                    selectCheck.SetSelected(true, true, 1);
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
                selectCheck.SetSelected(true, true);
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
