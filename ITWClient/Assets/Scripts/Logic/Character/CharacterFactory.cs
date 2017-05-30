using UnityEngine;
using System.Collections;

public class CharacterFactory : Singleton<CharacterFactory>
{
    protected override void Awake()
    {

    }

    public ICharacter Create(CharacterType type)
    {
        return Instantiate(Resources.Load<GameObject>("Prefabs/Character/" + type.ToString())).GetComponent<ICharacter>();
    }
}