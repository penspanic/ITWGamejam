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
        stageController.OnStageStart += OnStageStart;
        stageController.OnStageEnd += OnStageEnd;
    }

    private void Start()
    {
        remainTimeText.text = ((int)stageController.RemainElapsedTime).ToString();
    }

    private void OnStageStart()
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

    private void OnStageEnd()
    {

    }
}