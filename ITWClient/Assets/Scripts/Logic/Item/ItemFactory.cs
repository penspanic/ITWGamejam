using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ItemFactory : MonoBehaviour
{
    Dictionary<ItemType, GameObject> itemPrefabs;
    private void Awake()
    {
        itemPrefabs.Add(ItemType.HpPotion, Resources.Load<GameObject>("Prefabs/Item/HpPotion"));
        itemPrefabs.Add(ItemType.MpPotion, Resources.Load<GameObject>("Prefabs/Item/MpPotion"));
        itemPrefabs.Add(ItemType.ExtremePotion, Resources.Load<GameObject>("Prefabs/Item/ExtremePotion"));
    }

    public IItem CreateItem(ItemType type)
    {
        return Instantiate<GameObject>(itemPrefabs[type]).GetComponent<IItem>();
    }
}