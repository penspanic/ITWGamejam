using UnityEngine;
using System.Collections;

public class Heavy : ICharacter
{

    protected override void Awake()
    {
        base.Awake();
        CharacterType = CharacterType.Heavy;
    }

    protected override void UseSkill()
    {
        base.UseSkill();
    }
}