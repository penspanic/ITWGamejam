using UnityEngine;
using System.Collections;

public class Rocketeer : ICharacter
{
    protected override void Awake()
    {
        base.Awake();
        CharacterType = CharacterType.Rocketeer;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Charge()
    {
        base.Charge();
    }

    protected override void Dodge()
    {
        base.Dodge();
    }

    protected override void UseSkill()
    {
        base.UseSkill();
    }
}