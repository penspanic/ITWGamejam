using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum NoticeType
{
    ReadyAndFight,
    SuddenDeath,
    Victory,
}

public class NoticeBox : MonoBehaviour
{
    [SerializeField]
    private ReadyFight readyFight;
    [SerializeField]
    private GameObject suddenDeath;
    [SerializeField]
    private GameObject victory;

    public float moveTime;

    public void InitNoticeBox()
    {
        transform.localPosition = new Vector2(0, -410f);
        readyFight.InitReadyFight();
        AllNoticeSetActive(false);
    }

    public IEnumerator ShowNoticeBox(NoticeType noticeType)
    {
        switch (noticeType)
        {
            case NoticeType.ReadyAndFight:
                readyFight.SetPositionReadyFightTrs();
                readyFight.gameObject.SetActive(true);
                break;
            case NoticeType.SuddenDeath:
                suddenDeath.SetActive(true);
                break;
            case NoticeType.Victory:
                victory.SetActive(true);
                break;
            default:
                break;
        }
        transform.localPosition = new Vector2(0, -410f);

        yield return transform.DOLocalMoveY(0f, moveTime).SetEase(Ease.Linear).WaitForCompletion();
        yield return new WaitForSeconds(0.8f);
        if (noticeType == NoticeType.ReadyAndFight)
        {
            yield return readyFight.ChangeToFight();
            yield return new WaitForSeconds(0.5f);
        }
        yield return transform.DOLocalMoveY(410f, moveTime).SetEase(Ease.InBack).WaitForCompletion();
        AllNoticeSetActive(false);
    }

    private void AllNoticeSetActive(bool enable)
    {
        readyFight.gameObject.SetActive(enable);
        suddenDeath.SetActive(enable);
        victory.SetActive(enable);
    }
}
