using UnityEngine;
using System.Collections;

public class Doctor : ICharacter
{
    protected override void Awake()
    {
        base.Awake();
        CharacterType = CharacterType.Doctor;
    }

    protected override void Update()
    {
        base.Update();
    }
}