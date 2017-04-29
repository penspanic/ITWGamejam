using UnityEngine;
using System.Collections;

/// <summary>
/// 스태이지 내에서 아이템 생성하고, 플레이어에게 지급하는 로직 처리.
/// </summary>
public class ItemController : MonoBehaviour
{

    // HP 회복 물약,
    // 일정시간 MP 무제한
    // MP 회복 물약
    private StageController stageController;
    private ItemFactory itemFactory;
    private void Awake()
    {
        stageController = GameObject.FindObjectOfType<StageController>();
        itemFactory = gameObject.AddComponent<ItemFactory>();
    }

    public void StartStage()
    {

    }

    private IEnumerator ItemCreateProcess()
    {
        while(stageController.IsStageStarted == true)
        {
            float nextCreateInterval = Random.Range(5, 15);
            yield return new WaitForSeconds(nextCreateInterval);
        }
    }
}