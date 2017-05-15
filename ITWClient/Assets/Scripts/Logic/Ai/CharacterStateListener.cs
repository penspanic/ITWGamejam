using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterStateListener : Singleton<CharacterStateListener>
{

    private Dictionary<ICharacter, List<KeyValuePair<float, CharacterState>>> states = new Dictionary<ICharacter, List<KeyValuePair<float, CharacterState>>>();
    private const float StateStoreTime = 15f; // 지난 StateStoreTime동안의 State들을 저장(초단위)
    protected override void Awake()
    {
    }

    public void OnCharacterCreated(IObject character)
    {
        states.Add(character as ICharacter, new List<KeyValuePair<float, CharacterState>>());
    }

    public void OnCharacterDestroyed(IObject character)
    {
        states.Remove(character as ICharacter);
    }

    private void Update()
    {
        foreach(var characterPair in states)
        {
            characterPair.Value.RemoveAll((pair) =>
            {
                return pair.Key + StateStoreTime < Time.time;
            });

            characterPair.Value.Add(new KeyValuePair<float, CharacterState>(Time.time, characterPair.Key.State));
        }
    }
}