using UnityEngine;
using System.Collections;

public class CharacterFactory : MonoBehaviour
{
    private void Awake()
    {

    }

    public ICharacter Create(CharacterType type)
    {
        return Instantiate(Resources.Load<GameObject>("Prefabs/Character/" + type.ToString())).GetComponent<ICharacter>();
    }
}