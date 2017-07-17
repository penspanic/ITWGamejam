using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct TeamData
{
    public int TeamNumber;
    public List<PlayerInTeam> Players;

    // Players 모두 IsCpu가 true일 때 true 리턴.
    public bool IsCpuTeam()
    {
        return Players.FindIndex((player) =>
        {
            return player.IsCpu == false;
        }) == -1;
    }
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
    public static Ai.AiDifficulty AiDifficulty = Ai.AiDifficulty.Easy;

    static TeamController()
    {
        // dummy data
        TeamData redTeam = new TeamData();
        redTeam.TeamNumber = 1;
        redTeam.Players = new List<PlayerInTeam>();
        PlayerInTeam red1P = new PlayerInTeam();
        red1P.IsCpu = true;
        red1P.PlayerNumber = 1;
        red1P.SelectedCharacter = CharacterType.Doctor;
        redTeam.Players.Add(red1P);
        //PlayerInTeam redAi = new PlayerInTeam();
        //redAi.IsCpu = true;
        //redAi.PlayerNumber = 3;
        //redAi.SelectedCharacter = CharacterType.Rocketeer;
        //redTeam.Players.Add(redAi);

        Teams.Add(redTeam);

        TeamData blueTeam = new TeamData();
        blueTeam.TeamNumber = 2;
        blueTeam.Players = new List<PlayerInTeam>();
        PlayerInTeam blue2P = new PlayerInTeam();
        blue2P.IsCpu = false;
        blue2P.PlayerNumber = 2;
        blue2P.SelectedCharacter = CharacterType.Heavy;
        blueTeam.Players.Add(blue2P);
        //PlayerInTeam blue3P = new PlayerInTeam();
        //blue3P.IsCpu = true;
        //blue3P.PlayerNumber = 4;
        //blue3P.SelectedCharacter = CharacterType.Engineer;
        //blueTeam.Players.Add(blue3P);

        Teams.Add(blueTeam);
    }

    public static void SetTeam()
    {

    }

    public static Color GetTeamColor(int teamNum) {
        switch (teamNum)
        {
            case 1:
                return new Color(242f / 255f, 35f / 255f, 9f / 255f);
            case 2:
                return new Color(27f / 255f, 45f / 255f, 246f / 255f);
            case 3:
                return new Color(239f / 255f, 211f / 255f, 43f / 255f);
            case 4:
                return new Color(77f / 255f, 219f / 255f, 22f / 255f);
            default:
                break;
        }
        throw new UnityException("There's no color for " + teamNum.ToString() + " Team.");
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

    public static Vector2[] GetCharacterCreatePos(int playerCount)
    {
        if(playerCount == 2)
        {
            return new Vector2[2] { new Vector2(-2f, 0f), new Vector2(2f, 0f) };
        }
        else if(playerCount == 3)
        {
            return new Vector2[3] { new Vector2(0f, 1.42f), new Vector2(-1.27f, -1.12f), new Vector2(1.27f, -1.12f) };
        }
        else if(playerCount == 4)
        {
            return new Vector2[4] { new Vector2(1.25f, 1.1f), new Vector2(-1.25f, 1.1f), new Vector2(1.25f, -1.1f), new Vector2(-1.25f, -1.1f) };
        }

        throw new UnityException("Invalid player count : " + playerCount);
    }
}