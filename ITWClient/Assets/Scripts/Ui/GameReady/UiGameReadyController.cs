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

public class UiGameReadyController : MonoBehaviour {
    [SerializeField]
    private GameInfoReady gameInfoReady;
    [SerializeField]
    private CharacterReady charReady;
    [SerializeField]
    private Transform cursorTrs;
    [SerializeField]
    private Sprite[] bgs;
    [SerializeField]
    private Image bg;

    public GameReadyState gameReadyState { get; private set; }
    private float moveTime = 0.0f;

    // Data
    public HowPlayer howPlayer = HowPlayer.P1;
    public GameMode gameMode = GameMode.Personal;
    public PlayerTeamVersus versusMode = PlayerTeamVersus.None;
    public int cpuCount = 2;





    void Awake()
    {
        cursorTrs.gameObject.SetActive(true);
        SetGameReadyState(GameReadyState.SelectInfo);
    }

    public void SetGameReadyState(GameReadyState readyState) 
    {
        gameReadyState = readyState;
        switch (gameReadyState)
        {
            case GameReadyState.SelectInfo:
                bg.sprite = bgs[0];
                gameInfoReady.InitGameInfoReady();

                break;
            case GameReadyState.SelectCharacter:
                bg.sprite = bgs[1];
                cursorTrs.gameObject.SetActive(false);
                MoveNext();
                charReady.InitCharacterReady();
                break;

        }
    }

    public void MoveNext()
    {
        gameInfoReady.transform.localPosition = Vector2.zero;
        charReady.transform.localPosition = new Vector2(1280, 0);

        gameInfoReady.transform.DOLocalMoveX(-1280f, 0.5f).SetEase(Ease.OutCirc);
        charReady.transform.DOLocalMoveX(0f, 0.5f).SetEase(Ease.OutCirc).SetDelay(1f);
    }


    public void SetCursor(Vector2 position, bool isBack = false) 
    {
        // -1.5, -1.4
        // -1.1, -1.4
        cursorTrs.DOMove(position, 0.2f);
        //cursorTrs.position = position;

    }


}
