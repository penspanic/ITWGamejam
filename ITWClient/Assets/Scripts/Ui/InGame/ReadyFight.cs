using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReadyFight : MonoBehaviour {
    [SerializeField]
    private Transform readyTrs;
    [SerializeField]
    private Transform fightTrs;

    public float scrollTime;

    public void InitReadyFight()
    {
        SetPositionReadyFightTrs();
    }

    public void SetPositionReadyFightTrs()
    {
        readyTrs.localPosition = Vector2.zero;
        fightTrs.localPosition = new Vector2(0, 60);
    }

    public IEnumerator ChangeToFight()
    {
        readyTrs.DOLocalMoveY(-60, scrollTime).SetEase(Ease.InOutBack);
        yield return fightTrs.DOLocalMoveY(0f, scrollTime).SetEase(Ease.InOutBack).WaitForCompletion();
    }
}
