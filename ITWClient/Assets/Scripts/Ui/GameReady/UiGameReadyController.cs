using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum GameReadyState : short
{
    SelectInfo,
    SelectCharacter,
    Done
}

/// <summary>
/// PlayerMode
/// </summary>
public enum HowPlayer : short
{
    P1,
    P2,
}

/// <summary>
/// GameMode: 개인전, 팀전
/// </summary>
public enum GameMode : short
{
    Personal,
    Team
}

/// <summary>
/// 2PTeamMode, default = none
/// </summary>
public enum P2TeamMode
{
    None,
    PL2vsCP2,
    PLCPvsPLCP,
}

/// <summary>
/// 게임준비씬 담당 컨트롤러
/// </summary>
public class UiGameReadyController : MonoBehaviour
{
    [SerializeField]
    private GameInfoReady gameInfoReady; // 게임 정보 설정창
    [SerializeField]
    private CharacterReady charReady;   // 캐릭터 준비창
    [SerializeField]
    private Transform cursorTrs;
    [SerializeField]
    private Sprite[] bgs;
    [SerializeField]
    private Image bg;

    public float transitionTime; // 화면 전환 타임
    public GameReadyState gameReadyState { get; private set; }  // 게임 준비 상태

    // 인게임으로 가기전 필요한 데이터
    public HowPlayer howPlayer { get; set; }
    public GameMode gameMode { get; set; }
    public P2TeamMode versusMode { get; set; }
    public int cpuCount { get; set; }


    void Awake()
    {
        SetGameReadyState(GameReadyState.SelectInfo);
        if(BgmManager.Instance.Initialized == false)
        {
            BgmManager.Instance.LoadClips();
        }
        BgmManager.Instance.Play(BgmType.Select);
    }

    public void SetGameReadyState(GameReadyState readyState)
    {
        gameReadyState = readyState;
        SetBG(readyState);

        switch (gameReadyState)
        {
            case GameReadyState.SelectInfo:
                gameInfoReady.InitGameInfoReady();

                break;
            case GameReadyState.SelectCharacter:
                SetCursorEnable(false);
                StartCoroutine(MoveToSelectCharacter());
                break;

        }
    }

    private void SetBG(GameReadyState gs)
    {
        var bgIdx = (int)gs;

        if (bgIdx < bgs.Length)
        {
            bg.sprite = bgs[(int)gs];
        }
        else
        {
            Debug.Log("GameReadyState is over than BgIdx: " + gs.ToString());
            return;
        }
    }

    public void SetCursorEnable(bool isOn)
    {
        cursorTrs.gameObject.SetActive(isOn);
    }


    public IEnumerator MoveToSelectCharacter()
    {
        gameInfoReady.transform.localPosition = Vector2.zero;
        charReady.transform.localPosition = new Vector2(1280, 60f);

        gameInfoReady.transform.DOLocalMoveX(-1280f, 0.5f).SetEase(Ease.OutCirc);
        yield return charReady.transform.DOLocalMoveX(0f, 0.5f).SetEase(Ease.OutCirc).SetDelay(1f).WaitForCompletion();
        
        charReady.InitCharacterReady();
    }


    public void SetCursor(Vector2 position, bool isBack = false)
    {
        // -1.5, -1.4
        // -1.1, -1.4
        cursorTrs.DOMove(position, 0.2f);
        //cursorTrs.position = position;

    }


}
