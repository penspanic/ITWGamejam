using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 스태이지 내에서 아이템 생성하고, 플레이어에게 지급하는 로직 처리.
/// </summary>
public class ItemController : Singleton<ItemController>
{

    [SerializeField]
    float basicCreateInterval;
    [SerializeField]
    float nextStepElapsedTime;
    [SerializeField]
    float nextStepDecreaseInterval;

    private ItemFactory itemFactory;
    private Dictionary<ItemType, List<IItem>> items = new Dictionary<ItemType, List<IItem>>();
    private void Awake()
    {
        StageController.Instance.OnStageStart += OnStageStart;
        itemFactory = gameObject.AddComponent<ItemFactory>();
    }

    private void OnStageStart()
    {
        StartCoroutine(ItemCreateProcess());
    }

    private IEnumerator ItemCreateProcess()
    {
        // 스테이지 플레이한 시간 지날 수록 아이템이 나올 확률이 올라가도록.
        float elapsedTime = 0f;
        while(StageController.Instance.IsStageProcessing == true)
        {
            float nextCreateInterval = basicCreateInterval;
            if(elapsedTime > nextStepElapsedTime)
                nextCreateInterval = nextStepDecreaseInterval;
            yield return new WaitForSeconds(nextCreateInterval);
            ItemType newItemType = GetNewItemType();
            IItem newItem = itemFactory.CreateItem(newItemType);
            newItem.OnDestroy += OnItemDestroy;
            if(items.ContainsKey(newItemType) == false)
            {
                items.Add(newItemType, new List<IItem>());
            }
            items[newItemType].Add(newItem);

            SfxManager.Instance.Play(SfxType.Item_Create);
            elapsedTime += Time.deltaTime;

        }
    }
    
    private void OnItemDestroy(IItem item)
    {
        items[item.ItemType].Remove(item);
        item.OnDestroy -= OnItemDestroy;
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

    public IItem[] GetItems(ItemType itemType)
    {
        if(items.ContainsKey(itemType) == false)
        {
            return null;
        }

        return items[itemType].ToArray();
    }

    public IItem GetNearestItem(ItemType itemType, Vector3 position)
    {
        if(items.ContainsKey(itemType) == false)
        {
            return null;
        }

        var sortedItems = items[itemType].OrderBy((item) => (item.transform.position - position).magnitude);
        if (sortedItems.Count() > 0)
        {
            return sortedItems.First();
        }

        return null;
    }
}