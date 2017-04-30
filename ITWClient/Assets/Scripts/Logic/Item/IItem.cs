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

    private MapController mapController;
    protected virtual void Awake()
    {
        mapController = GameObject.FindObjectOfType<MapController>();
    }

    public abstract void UseItem(ICharacter owner);

    protected void Destroy()
    {
        Destroy(this.gameObject);
    }
}