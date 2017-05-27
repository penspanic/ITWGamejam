using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : Singleton<CharacterManager>
{
    public Dictionary<Player, ICharacter> Characters { get; private set; }

    public CharacterManager()
    {
        Characters = new Dictionary<Player, ICharacter>();
    }

    protected override void Awake()
    {
    }

    public void Create(Player player, CharacterType characterType)
    {
        ICharacter newCharacter = CharacterFactory.Instance.Create(characterType);
        player.SetCharacter(newCharacter);
        newCharacter.Initialize(player);
        int layer = LayerMask.NameToLayer("Team" + TeamController.GetTeam(player.PlayerNumber).TeamNumber.ToString());
        newCharacter.gameObject.layer = layer;
        newCharacter.OnDestroyed += OnCharacterDeath;
        Characters.Add(player, newCharacter);

        if(player.GetComponent<PlayerInputController>() != null)
            player.GetComponent<PlayerInputController>().Initialized = true;

        newCharacter.GetComponentInChildren<CharacterFloatingUi>().Initialize(player);

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

    public void OnCharacterDeath(IObject character)
    {
        Player key = null;
        foreach(var characterPair in Characters)
        {
            if(characterPair.Value == character as ICharacter)
            {
                key = characterPair.Key;
            }
        }
        if(key != null)
        {
            Characters.Remove(key);
        }
    }

    public ICharacter[] GetEmemys(ICharacter target)
    {
        int targetTeamNumber = target.Player.TeamNumber;

        List<ICharacter> ememys = new List<ICharacter>();
        foreach(var eachCharacter in Characters)
        {
            if(eachCharacter.Key.TeamNumber != targetTeamNumber)
            {
                ememys.Add(eachCharacter.Value);
            }
        }

        return ememys.ToArray();
    }

    public ICharacter GetNearestEnemy(ICharacter target)
    {
        ICharacter[] enemys = GetEmemys(target);

        float nearestDistance = float.MaxValue;
        ICharacter nearestEnemy = null;
        for(int i = 0; i < enemys.Length; ++i)
        {
            float distance = (enemys[i].transform.position - target.transform.position).magnitude;
            if(distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemys[i];
            }
        }

        return nearestEnemy;
    }
}