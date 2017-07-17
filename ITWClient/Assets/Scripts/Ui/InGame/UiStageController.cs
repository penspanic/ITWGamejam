using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    private bool isSceneChanging = false;
    private void Update()
    {
        if(stageController.IsStageProcessing == true)
        {
            remainTimeText.text = ((int)stageController.RemainElapsedTime).ToString();
        }
        else if(isSceneChanging == false)
        {
            if(Input.GetKeyDown(KeyCode.Return) == true || Input.GetButtonDown("Submit") == true)
            {
                SceneUtil.LoadScene("MainMenu");
            }
        }
    }

    private void OnStageEnd(int winTeamNumber)
    {
        BgmManager.Instance.Play(BgmType.GameOver);
        noticeBox.gameObject.SetActive(true);

        Debug.Log("WinTeamNumber : " + winTeamNumber);
        // TODO : 아래 if 구문 noticeBox 타입 다르게 처리 필요
        if(winTeamNumber == -1)
        {
            Debug.Log("StageEnd : Draw");
            // Draw
        }
        else if(TeamController.GetTeam(winTeamNumber).IsCpuTeam() == true)
        {
            Debug.Log("StageEnd : Lose");
            // Lose
        }
        else
        {
            Debug.Log("StageEnd : Win");
            // Team + winTeamNumber Win
        }

        StartCoroutine(noticeBox.ShowNoticeBox(NoticeType.Victory));
    }
}