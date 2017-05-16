using UnityEngine;

public enum ItemType
{
    HpPotion,
    MpPotion,
    ExtremePotion,
}

public abstract class IItem : MonoBehaviour
{
    public ItemType Type { get; set; }

    protected virtual void Awake()
    {
    }

    public abstract void UseItem(ICharacter owner);

    protected void Destroy()
    {
        Destroy(this.gameObject);
    }
}