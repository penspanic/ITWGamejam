using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public GameReadyState gameReadyState { get; private set; }

    // Data
    public HowPlayer howPlayer = HowPlayer.P1;
    public GameMode gameMode = GameMode.Personal;
    public PlayerTeamVersus versusMode = PlayerTeamVersus.None;
    public int cpuCount = 2;





    void Awake()
    {
        SetGameReadyState(GameReadyState.SelectInfo);
    }

    public void SetGameReadyState(GameReadyState readyState) 
    {
        gameReadyState = readyState;
        switch (gameReadyState)
        {
            case GameReadyState.SelectInfo:
                gameInfoReady.InitGameInfoReady();

                break;
            case GameReadyState.SelectCharacter:
                MoveNext();
                charReady.InitCharacterReady();
                break;

        }
    }

    public void MoveNext()
    {
        gameInfoReady.transform.DOLocalMoveX(-1280, 0.8f).SetEase(Ease.InOutBack);
        charReady.transform.DOLocalMoveX(0f, 0.8f).SetEase(Ease.InOutBack);
    }


    public void SetCursor(Vector2 position) 
    {
        cursorTrs.position = position;
    }


}
