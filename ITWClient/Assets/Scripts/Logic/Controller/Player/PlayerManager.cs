using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public List<Player> Players { get; private set; }
    private void Awake()
    {

    }
    
    public void CreatePlayers()
    {
        Players = new List<Player>();

        Transform playersParent = GameObject.Find("Players").transform;
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        List<PlayerInputController> inputControllers = new List<PlayerInputController>();
        foreach(TeamData team in TeamController.Teams)
        {
            foreach(PlayerInTeam playerData in team.Players)
            {
                GameObject newPlayerObject = Instantiate(playerPrefab);
                newPlayerObject.name = "Player" + playerData.PlayerNumber.ToString();
                newPlayerObject.transform.SetParent(playersParent.transform);
                if(playerData.IsCpu == true)
                {
                    newPlayerObject.AddComponent<AiPlayer>();
                }
                else
                {
                    newPlayerObject.AddComponent<Player>();
                    PlayerInputController inputController = newPlayerObject.AddComponent<PlayerInputController>();
                    inputControllers.Add(inputController);
                }
                Player newPlayer = newPlayerObject.GetComponent<Player>();
                newPlayer.IsCpu = playerData.IsCpu;
                newPlayer.SetNumber(team.TeamNumber, playerData.PlayerNumber);
                Players.Add(newPlayer);
            }
        }

        GameObject.FindObjectOfType<InputManager>().SetPlayers(inputControllers.ToArray());
    }
}