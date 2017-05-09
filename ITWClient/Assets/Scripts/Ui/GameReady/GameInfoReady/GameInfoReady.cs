using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum GameInfoState : short
{
    SelectPlayer,
    SelectMode,
    SelectCPU,
    AllSelectDone,
}

/// <summary>
/// 게임 정보를 선택하는 창 부모 스크립트
/// </summary>
public class GameInfoReady : MonoBehaviour
{
    [SerializeField]
    private GameInfoRaw selectPlayer;  // 플레이어 선택창
    [SerializeField]
    private GameInfoRaw selectMode;  // 모드 선택
    [SerializeField]
    private GameInfoRaw selectCPU;  // CPU or 2PTeamMode선택
    [SerializeField]
    private GameInfoRaw selectCheck; // Check선택
    [SerializeField]
    private float infoRawAniTime;

    private GameInfoState gameInfoState;
    private Dictionary<GameInfoState, GameInfoRaw> selectDic;
    private Vector2[] oriPosRaws;
    private UiGameReadyController uiGameReady;
    private bool isCanTouch = false;
    private bool isDoneRaw = false;
    private bool canGetCpuCount = false;
    private bool isAllCheck = false;
    private bool isDone = false;


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
        selectCPU.InitGameInfoRaw();
        selectCheck.InitGameInfoRaw();

        isDoneRaw = false;
        isAllCheck = false;
        isCanTouch = false;

        MoveRightToCenter(selectPlayer);
        selectPlayer.UpdateDataByInfoType();
        selectCheck.UpdateDataByInfoType();
    }

    private void MoveRightToCenter(GameInfoRaw target)
    {
        isCanTouch = false;
        target.gameObject.SetActive(true);
        target.transform.localPosition = new Vector2(1280f, target.transform.localPosition.y);
        target.transform.DOLocalMoveX(0f, infoRawAniTime).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            uiGameReady.SetCursorEnable(true);
            isCanTouch = true;
            target.SetSelected(true, true);
        });
    }

    void Update()
    {
        if (isCanTouch == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Back.
        }
        if (Input.GetKeyDown(UIGameKey.Select_1P))
        {
            if (isDoneRaw == true)
            {
                if (selectCheck.GetCurrIdx() == 0)
                {
                    //back
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                    return;
                }

                if (gameInfoState != GameInfoState.AllSelectDone)
                {
                    return;
                }
            }

            switch (gameInfoState)
            {
                case GameInfoState.SelectPlayer:
                    selectPlayer.SelectByCurrIdx();
                    uiGameReady.howPlayer = (HowPlayer)selectPlayer.GetCurrIdx();
                    ++gameInfoState;
                    selectPlayer.SetSelected(false);
                    MoveRightToCenter(selectMode);
                    break;
                case GameInfoState.SelectMode:
                    selectMode.SelectByCurrIdx();
                    uiGameReady.gameMode = (GameMode)selectMode.GetCurrIdx();
                    ++gameInfoState;
                    selectMode.SetSelected(false);

                    if (IsSoloTeam(uiGameReady.gameMode, uiGameReady.howPlayer))
                    {
                        gameInfoState = GameInfoState.AllSelectDone;
                        uiGameReady.cpuCount = 3;
                        selectCheck.SetSelected(true, true, 1);
                        isDoneRaw = true;
                        isAllCheck = true;
                        return;
                    }
                    canGetCpuCount = selectCPU.InitCPUCols(uiGameReady.gameMode, uiGameReady.howPlayer);
                    MoveRightToCenter(selectCPU);
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
                            uiGameReady.versusMode = P2TeamMode.PL2vsCP2;
                        }
                        else if (selectIdx == 1)
                        {
                            uiGameReady.versusMode = P2TeamMode.PLCPvsPLCP;
                        }
                    }
                    ++gameInfoState;
                    selectCPU.SetSelected(false);
                    selectCheck.SetSelected(true, true, 1);
                    isAllCheck = true;
                    isDoneRaw = true;
                    uiGameReady.SetCursorEnable(false);
                    break;
                case GameInfoState.AllSelectDone:
                    var idx = selectCheck.GetCurrIdx();
                    if (idx == 0)
                    {
                        // back
                        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                    }
                    else if (idx == 1)
                    {
                        // next
                        isCanTouch = false;
                        selectCheck.SetSelected(false);
                        selectCheck.gameObject.SetActive(false);
                        uiGameReady.SetGameReadyState(GameReadyState.SelectCharacter);
                    }
                    break;
            }

            if ((int)gameInfoState <= (int)GameInfoState.SelectMode)
            {
                selectDic[gameInfoState].UpdateDataByInfoType();
            }
        }

        if (Input.GetKeyDown(UIGameKey.DownArrow_1P))
        {
            if (gameInfoState != GameInfoState.AllSelectDone)
            {
                uiGameReady.SetCursorEnable(false);
                isDoneRaw = true;
                selectDic[gameInfoState].SetSelected(false);
                selectCheck.SetSelected(true, true);
            }

        }
        if (Input.GetKeyDown(UIGameKey.UpArrow_1P))
        {
            if (gameInfoState != GameInfoState.AllSelectDone)
            {
                uiGameReady.SetCursorEnable(true);
                selectCheck.BackRawSelectByCurrIdx(true);
                selectDic[gameInfoState].SetSelected(true);
                selectCheck.SetSelected(false);
                isDoneRaw = false;
            }

        }
    }

    private bool IsSoloTeam(GameMode gm, HowPlayer howPlay)
    {
        if (gm == GameMode.Team && howPlay == HowPlayer.P1)
        {
            return true;
        }
        return false;
    }

}
