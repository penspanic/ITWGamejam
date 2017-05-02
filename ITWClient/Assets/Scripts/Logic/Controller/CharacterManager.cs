using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    private CharacterFactory characterFactory = null;
    public Dictionary<Player, ICharacter> Characters { get; private set; }
    public CharacterManager()
    {
        Characters = new Dictionary<Player, ICharacter>();
    }

    private void Awake()
    {
    }

    public void Create(Player player, CharacterType characterType)
    {
        ICharacter newCharacter = characterFactory.Create(characterType);
        player.SetCharacter(newCharacter);
        newCharacter.Initialize(player);
        int layer = LayerMask.NameToLayer("Team" + TeamController.GetTeam(player.PlayerNumber).TeamNumber.ToString());
        newCharacter.gameObject.layer = layer;
        newCharacter.OnDestroyed += OnCharacterDeath;
        Characters.Add(player, newCharacter);

        if(player.GetComponent<PlayerInputController>() != null)
            player.GetComponent<PlayerInputController>().Initialized = true;
    }

    public void ChangeCharacter(Player player, CharacterType characterType)
    {
        if(Characters.ContainsKey(player) == false)
        {
            throw new UnityException("Can not change character before create character, " + player.name);
        }

        Characters.Remove(player);
        Create(player, characterType);
    }

    public void OnCharacterDeath(IObject character)
    {
        ICharacter deadCharacter = character as ICharacter;
        // TODO : 죽은 캐릭터 가장 뒤에 보이도록 해야 함.
    }
}