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

    // 현재 Create 이후에 호출되고 있음.
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

        SortingLayerController.Instance.AddTarget(newCharacter);
    }

    public void ChangeCharacter(Player player, CharacterType characterType)
    {
        if(Characters.ContainsKey(player) == false)
        {
            throw new UnityException("Can not change character before create character, " + player.name);
        }

        // TODO : List안에서 지우는 것말고 GameObject Destroy 시키는 것도 해야 할 듯
        Characters.Remove(player);
        Create(player, characterType);
    }

    private void Update()
    {
        for(int i = 0; i < Characters.Count; ++i)
        {

        }
    }

    public void OnCharacterDeath(IObject character)
    {
        ICharacter deadCharacter = character as ICharacter;
    }
}