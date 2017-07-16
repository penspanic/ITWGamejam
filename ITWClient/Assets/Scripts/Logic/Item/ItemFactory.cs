using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ItemFactory : MonoBehaviour
{
    private Dictionary<ItemType, GameObject> itemPrefabs = new Dictionary<ItemType, GameObject>();
    private void Awake()
    {
        itemPrefabs.Add(ItemType.HpPotion, Resources.Load<GameObject>("Prefabs/Item/HpPotion"));
        itemPrefabs.Add(ItemType.MpPotion, Resources.Load<GameObject>("Prefabs/Item/MpPotion"));
        itemPrefabs.Add(ItemType.ExtremePotion, Resources.Load<GameObject>("Prefabs/Item/ExtremePotion"));
    }

    public IItem CreateItem(ItemType type)
    {
        IItem newItem = Instantiate(itemPrefabs[type]).GetComponent<IItem>();
        //newItem.TilePos = mapController.GetEmptyTilePos();
        //mapController.SetTile(newItem);
        newItem.transform.position = MapController.GetRandomMapPos();
        return newItem;
    }
}