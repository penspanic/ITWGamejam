using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiStageController : MonoBehaviour
{

    private void Awake()
    {

    }

    public void StartStage()
    {
        StartCoroutine(ReadyStartProcess());
    }

    private IEnumerator ReadyStartProcess()
    {
        yield break;
    }


}