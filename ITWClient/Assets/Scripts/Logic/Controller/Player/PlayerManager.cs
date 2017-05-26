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
                GameObject newPlayerObject = new GameObject("Player" + playerData.PlayerNumber.ToString());
                newPlayerObject.transform.SetParent(playersParent.transform);
                PlayerController playerController = newPlayerObject.AddComponent<PlayerController>();

                if(playerData.IsCpu == true)
                {
                    newPlayerObject.AddComponent<Ai.AiPlayer>();
                }
                else
                {
                    newPlayerObject.AddComponent<Player>();
                    PlayerInputController inputController = newPlayerObject.AddComponent<PlayerInputController>();
                    inputControllers.Add(inputController);
                    inputController.PlayerController = playerController;
                }

                Player newPlayer = newPlayerObject.GetComponent<Player>();
                playerController.TargetPlayer = newPlayer;
                newPlayer.IsCpu = playerData.IsCpu;
                newPlayer.SetNumber(team.TeamNumber, playerData.PlayerNumber);
                Players.Add(newPlayer);
            }
        }

        InputManager.Instance.SetPlayers(inputControllers.ToArray());
    }
}