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

    public void OnStageStart()
    {
        StartCoroutine(ItemCreateProcess());
    }

    private IEnumerator ItemCreateProcess()
    {
        // 스테이지 플레이한 시간 지날 수록 아이템이 나올 확률이 올라가도록.
        float elapsedTime = 0f;
        while(stageController.IsStageStarted == true)
        {
            float nextCreateInterval = 20f;
            if(elapsedTime > 60f)
                nextCreateInterval = 15f;
            yield return new WaitForSeconds(nextCreateInterval);
            itemFactory.CreateItem(GetNewItemType());
            elapsedTime += Time.deltaTime;

        }
    }

    private ItemType GetNewItemType()
    {
        int randomValue = Random.Range(0, 10);
        if(randomValue >= 0 && randomValue < 4)
        {
            return ItemType.HpPotion;
        }
        else if(randomValue >= 4 && randomValue < 8)
        {
            return ItemType.MpPotion;
        }
        else
        {
            return ItemType.ExtremePotion;
        }
    }
}