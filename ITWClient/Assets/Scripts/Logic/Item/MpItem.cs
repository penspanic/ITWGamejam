using UnityEngine;
using System.Collections;

public class MpItem : IItem
{
    [SerializeField]
    private int chargeValue;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void UseItem(ICharacter owner)
    {
        owner.Mp += chargeValue;
        if(owner.Mp > owner.MaxMp)
        {
            owner.Mp = owner.MaxMp;
        }
        Destroy();
    }
}