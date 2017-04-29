using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    private CharacterFactory characterFactory = null;
    private Dictionary<Player, ICharacter> characters = new Dictionary<Player, ICharacter>();
    private void Awake()
    {
    }

    public void Create(Player player, CharacterType characterType)
    {
        ICharacter newCharacter = characterFactory.Create(characterType);
        player.SetCharacter(newCharacter);
        characters.Add(player, newCharacter);
        if(player.GetComponent<PlayerInputController>() != null)
            player.GetComponent<PlayerInputController>().Initialized = true;
    }

    public void ChangeCharacter(Player player, CharacterType characterType)
    {
        if(characters.ContainsKey(player) == false)
        {
            throw new UnityException("Can not change character before create character, " + player.name);
        }

        characters.Remove(player);
        Create(player, characterType);
    }
}