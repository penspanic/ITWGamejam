using UnityEngine;

public enum ItemType
{
    HpPotion,
    MpPotion,
    ExtremePotion,
}

public abstract class IItem : MonoBehaviour
{
    public ItemType ItemType { get; set; }
    public event System.Action<IItem> OnDestroy;

    protected virtual void Awake()
    {
    }

    public abstract void UseItem(ICharacter owner);

    protected void Destroy()
    {
        if(OnDestroy != null)
        {
            OnDestroy(this);
        }

        Destroy(this.gameObject);
    }
}