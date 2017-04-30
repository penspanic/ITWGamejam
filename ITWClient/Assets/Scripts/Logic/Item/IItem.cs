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
    public IntVector2 TilePos { get; set; }

    private MapController mapController;
    protected virtual void Awake()
    {
        mapController = GameObject.FindObjectOfType<MapController>();
    }

    public abstract void UseItem(ICharacter owner);

    protected void Destroy()
    {
        Destroy(this.gameObject);
        //mapController.RemoveTile(TilePos);
    }
}