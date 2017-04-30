using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct TeamData
{
    public int TeamNumber;
    public List<PlayerInTeam> Players;
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
        redTeam.TeamNumber = 1;
        redTeam.Players = new List<PlayerInTeam>();
        PlayerInTeam red1P = new PlayerInTeam();
        red1P.IsCpu = false;
        red1P.PlayerNumber = 1;
        red1P.SelectedCharacter = CharacterType.Rocketeer;
        redTeam.Players.Add(red1P);
        PlayerInTeam redAi = new PlayerInTeam();
        redAi.IsCpu = true;
        redAi.PlayerNumber = 3;
        redAi.SelectedCharacter = CharacterType.Rocketeer;
        redTeam.Players.Add(redAi);

        Teams.Add(redTeam);

        TeamData blueTeam = new TeamData();
        blueTeam.TeamNumber = 2;
        blueTeam.Players = new List<PlayerInTeam>();
        PlayerInTeam blue2P = new PlayerInTeam();
        blue2P.IsCpu = false;
        blue2P.PlayerNumber = 2;
        blue2P.SelectedCharacter = CharacterType.Doctor;
        blueTeam.Players.Add(blue2P);

        Teams.Add(blueTeam);
    }

    public static void SetTeam()
    {

    }

    public static Color GetTeamColor(int teamNum) {
        switch (teamNum)
        {
            case 1:
                return Color.red;
            case 2:
                return Color.blue;
            case 3:
                return Color.green;
            case 4:
                return Color.yellow;
            default:
                return Color.black;
        }
    }

    public static TeamData GetTeam(int playerNum){
        foreach (var eachTeam in Teams)
        {
            foreach (PlayerInTeam player in eachTeam.Players)
            {
                if (playerNum == player.PlayerNumber)
                {
                    return eachTeam;
                }
            }
        }
        throw new UnityException("TeamData not found, playerNum : " + playerNum.ToString());
    }

    public static void AddTeam(int teamNum, List<PlayerInTeam> plTeam) {
        for (int i = 0; i < Teams.Count; ++i)
        {
            if (Teams[i].TeamNumber == teamNum)
            {
                Debug.Log("TeamNum Overlap..");
                return;
            }
        }

        TeamData newTeam = new TeamData();
        newTeam.TeamNumber = teamNum;
        newTeam.Players = plTeam;
        Teams.Add(newTeam);
    }

    public static void AddPlayerInTeam(int teamNum, bool isCPU, int plNum, CharacterType charType) {
        bool existTeam = false;
        int teamIdx = 0;

        foreach (var eachTeam in Teams)
        {
            foreach (PlayerInTeam player in eachTeam.Players)
            {
                if (plNum == player.PlayerNumber)
                {
                    Debug.Log("PlayerNum is overlap..");
                    return;
                }
            }
        }

        var newPlayer = new PlayerInTeam();
        newPlayer.IsCpu = isCPU;
        newPlayer.PlayerNumber = plNum;
        newPlayer.SelectedCharacter = charType;

        for (int i = 0; i < Teams.Count; ++i)
        {
            if (Teams[i].TeamNumber == teamNum)
            {
                teamIdx = i;
                Teams[i].Players.Add(newPlayer);
                return;
            }
        }

        List<PlayerInTeam> players = new List<PlayerInTeam>();
        players.Add(newPlayer);
        AddTeam(teamNum, players);
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