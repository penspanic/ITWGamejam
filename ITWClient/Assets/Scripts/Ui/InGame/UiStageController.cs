using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiStageController : MonoBehaviour
{
    [SerializeField]
    private Text remainTimeText;

    private StageController stageController;
    private void Awake()
    {
        stageController = GameObject.FindObjectOfType<StageController>();
    }

    private void Start()
    {
        remainTimeText.text = ((int)stageController.RemainElapsedTime).ToString();
    }

    public void StartStage()
    {
        StartCoroutine(ReadyStartProcess());
    }

    private IEnumerator ReadyStartProcess()
    {
        yield break;
    }

    private void Update()
    {
        if(stageController.IsStageStarted == true)
        {
            remainTimeText.text = ((int)stageController.RemainElapsedTime).ToString();
        }
    }
}