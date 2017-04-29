using UnityEngine;
using System.Collections;
using System;

public class HpItem : IItem
{
    [SerializeField]
    private int HealValue;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void UseItem(ICharacter owner)
    {
        owner.Hp += HealValue;
        if(owner.Hp > owner.MaxHp)
        {
            owner.Hp = owner.MaxHp;
        }
    }
}