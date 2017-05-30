using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiStageController : MonoBehaviour
{
    [SerializeField]
    private Text remainTimeText;
    [SerializeField]
    private NoticeBox noticeBox;

    private StageController stageController;
    private void Awake()
    {
        stageController = GameObject.FindObjectOfType<StageController>();
        stageController.OnStageEnd += OnStageEnd;

        if(BgmManager.Instance.Initialized == false)
        {
            BgmManager.Instance.LoadClips();
        }

        noticeBox.gameObject.SetActive(true);
        noticeBox.InitNoticeBox();
    }

    private void Start()
    {
        remainTimeText.text = ((int)stageController.RemainElapsedTime).ToString();
    }

    public void ShowReadyStart()
    {
        StartCoroutine(ReadyStartProcess());
    }

    private IEnumerator ReadyStartProcess()
    {
        yield return noticeBox.ShowNoticeBox(NoticeType.ReadyAndFight);
        // yield break;
    }

    private void Update()
    {
        if(stageController.IsStageStarted == true)
        {
            remainTimeText.text = ((int)stageController.RemainElapsedTime).ToString();
        }
    }

    private void OnStageEnd()
    {
        BgmManager.Instance.Play(BgmType.GameOver);
        noticeBox.gameObject.SetActive(true);
        StartCoroutine(noticeBox.ShowNoticeBox(NoticeType.Victory));
    }
}