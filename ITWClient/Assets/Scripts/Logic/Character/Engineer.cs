using UnityEngine;
using System.Collections;

public class Engineer : ICharacter
{

    protected override void Awake()
    {
        base.Awake();
        CharacterType = CharacterType.Engineer;
    }

    protected override void UseSkill()
    {
        base.UseSkill();
    }
}