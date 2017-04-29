using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct TeamData
{
    public int TeamIndex;
    public List<PlayerInTeam> Players;
    Color TeamColor;
}

public struct PlayerInTeam
{
    public bool IsCpu;
    public int PlayerNumber;
    public CharacterType SelectedCharacter;
}

public static class TeamController
{
    public static List<TeamData> Teams = new List<TeamData>();
    static TeamController()
    {
        // dummy data
        TeamData redTeam = new TeamData();
        redTeam.TeamIndex = 0;
        redTeam.Players = new List<PlayerInTeam>();
        PlayerInTeam red1P = new PlayerInTeam();
        red1P.IsCpu = false;
        red1P.PlayerNumber = 1;
        red1P.SelectedCharacter = CharacterType.Rocketeer;
        redTeam.Players.Add(red1P);
        Teams.Add(redTeam);

        TeamData blueTeam = new TeamData();
        blueTeam.TeamIndex = 1;
        blueTeam.Players = new List<PlayerInTeam>();
        PlayerInTeam blue2P = new PlayerInTeam();
        blue2P.IsCpu = false;
        blue2P.PlayerNumber = 2;
        blue2P.SelectedCharacter = CharacterType.Rocketeer;
        blueTeam.Players.Add(blue2P);
        Teams.Add(blueTeam);
    }

    public static void SetTeam()
    {

    }

    public static CharacterType GetCharacterType(int playerNum)
    {
        foreach(TeamData eachTeam in Teams)
        {
            foreach(PlayerInTeam eachPlayer in eachTeam.Players)
            {
                if(eachPlayer.PlayerNumber == playerNum)
                {
                    return eachPlayer.SelectedCharacter;
                }
            }
        }
        throw new UnityException("playerNum not found.");
    }
}