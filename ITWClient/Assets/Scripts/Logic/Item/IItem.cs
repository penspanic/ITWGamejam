using UnityEngine;

public enum ItemType
{
    HpPotion,
    MpPotion,
    ExtremePotion,
}

public abstract class IItem : MonoBehaviour, ITile
{
    public ItemType Type { get; set; }
    protected virtual void Awake()
    {

    }
}