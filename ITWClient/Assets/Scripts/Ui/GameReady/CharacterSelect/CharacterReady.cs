using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SelectState
{
    PlayerSelect,
    CpuSelect,
}

public class CharacterReady : MonoBehaviour {
    [SerializeField]
    private CharacterSelecter[] selecters;

    private UiGameReadyController uiReadyController;
    private SelectState selectState;
    private List<CharacterType> characterTypeList;

    private int plCnt;
    private int cpuCnt;

    private bool isInit = false;
    private int currCheckPlCnt = 0;
    private int currCheckCpuCnt = 0;
    private int checkIdx = 0;

    private bool isStart = false;
    private bool isPlReady = false;
    private bool isP2Ready = false;

    public void InitCharacterReady() 
    {
        uiReadyController = FindObjectOfType<UiGameReadyController>();
        characterTypeList = new List<CharacterType>();

        plCnt = (int)uiReadyController.howPlayer + 1;
        cpuCnt = uiReadyController.cpuCount;

        selectState = SelectState.PlayerSelect;

        var currIdx = 0;
        var totalCnt = plCnt + cpuCnt;
        if (plCnt >= 1)
        {
            selecters[currIdx].InitCharacterSelecter(0, PlayerType.PL1);
            selecters[currIdx].OnSelected(true, "P1", GetTeamColorByPlayerType(PlayerType.PL1, currIdx));
            ++currIdx;
        }
        if (plCnt == 2)
        {
            selecters[currIdx].InitCharacterSelecter(0, PlayerType.PL2);
            selecters[currIdx].OnSelected(true, "P2", GetTeamColorByPlayerType(PlayerType.PL2, currIdx));
            ++currIdx;
        }
        for (int i = currIdx; i < totalCnt; ++i)
        {
            selecters[i].InitCharacterSelecter(0, PlayerType.CPU);
        }
        //for (int i = totalCnt; i < selecters.Length; ++i)
        //{
        //    selecters[i].InitCharacterSelecter(99, PlayerType.None);
        //}
        isInit = true;
        isPlReady = false;
        isP2Ready = false;
    }

    public void Update() 
    {
        if (isInit == false)
        {
            return;
        }
        switch (selectState)
        {
            case SelectState.PlayerSelect:
                if (plCnt >= 1)
                {
                    if (Input.GetKeyDown(UIGameKey.UpArrow_1P))
                    {
                        selecters[0].MoveNext(false);
                    }
                    if (Input.GetKeyDown(UIGameKey.DownArrow_1P))
                    {
                        selecters[0].MoveNext(true);
                    }
                    if (Input.GetKeyDown(UIGameKey.Select_1P))
                    {
                        if (IsCharacterNoneIdx(selecters[0].currIdx))
                        {
                            return;
                        }

                        if (isPlReady == true)
                        {
                            return;
                        }

                        if (characterTypeList.Count == 0)
                        {
                            characterTypeList.Add((CharacterType)selecters[0].DecideCharacter());
                        }
                        else
                        {
                            characterTypeList.Insert(0, (CharacterType)selecters[0].DecideCharacter());
                        }
                        ++currCheckPlCnt;
                        ++checkIdx;
                        isPlReady = true;
                    }
                }

                if (plCnt == 2)
                {
                    if (Input.GetKeyDown(UIGameKey.UpArrow_2P))
                    {
                        selecters[1].MoveNext(false);
                    }
                    if (Input.GetKeyDown(UIGameKey.DownArrow_2P))
                    {
                        selecters[1].MoveNext(true);
                    }

                    if (Input.GetKeyDown(UIGameKey.Select_2P))
                    {
                        if (IsCharacterNoneIdx(selecters[1].currIdx))
                        {
                            return;
                        }
                        if (isP2Ready == true)
                        {
                            return;
                        }


                        characterTypeList.Add((CharacterType)selecters[1].DecideCharacter());
                        ++currCheckPlCnt;
                        // selecters[1].OnSelected(true);
                        ++checkIdx;
                        isP2Ready = true;
                    }
                }

                if (currCheckPlCnt >= plCnt)
                {
                    if (currCheckCpuCnt >= cpuCnt)
                    {
                        StartGame();
                        return;
                    }
                    selecters[checkIdx].OnSelected(true, "CPU", GetTeamColorByPlayerType(PlayerType.CPU, checkIdx));
                    ++selectState;
                }
                break;
            case SelectState.CpuSelect:
                //Debug.Log(cpuCnt);
                if (currCheckCpuCnt >= cpuCnt)
                {
                    StartGame();
                    return;
                }
                if (Input.GetKeyDown(UIGameKey.UpArrow_1P))
                {
                    selecters[checkIdx].MoveNext();
                }
                if (Input.GetKeyDown(UIGameKey.DownArrow_1P))
                {
                    selecters[checkIdx].MoveNext();
                }
                if (Input.GetKeyDown(UIGameKey.Select_1P))
                {
                    if (IsCharacterNoneIdx(selecters[checkIdx].currIdx))
                    {
                        return;
                    }
                    characterTypeList.Add((CharacterType)selecters[checkIdx].DecideCharacter());
                    ++currCheckCpuCnt;
                    if (checkIdx <= selecters.Length - 2)
                    {
                        if (currCheckCpuCnt >= cpuCnt)
                        {
                            StartGame();
                            return;
                        }
                        ++checkIdx;
                        selecters[checkIdx].OnSelected(true, "CPU", GetTeamColorByPlayerType(PlayerType.CPU, checkIdx));
                    }
                }


                break;
        }



    }


    private void StartGame()
    {
        if (isStart == true)
        {
            return;
        }
        isStart = true;

        for (int i = 0; i < selecters.Length; ++i)
        {
            selecters[i].OnSelected(false, "");
        }


        TeamController.Teams.Clear();

        var totalCnt = plCnt + cpuCnt;

        switch (uiReadyController.gameMode)
        {
            case GameMode.Personal:
                for (int i = 0; i < totalCnt; ++i)
                {
                    TeamController.AddPlayerInTeam(i + 1, ((i + 1 <= plCnt) ? false : true), i+1, characterTypeList[i]);
                }
                break;
            case GameMode.Team:
                // case 1: team, pl1 -> 2(p1, cpu)vs(cpu, cpu)
                if (uiReadyController.howPlayer == HowPlayer.P1)
                {
                    TeamController.AddPlayerInTeam(1, false, 1, characterTypeList[0]);
                    TeamController.AddPlayerInTeam(1, true, 2, characterTypeList[1]);
                    TeamController.AddPlayerInTeam(2, true, 3, characterTypeList[2]);
                    TeamController.AddPlayerInTeam(2, true, 4, characterTypeList[3]);
                }
                else if (uiReadyController.howPlayer == HowPlayer.P2)
                {
                    if (uiReadyController.versusMode == P2TeamMode.PL2vsCP2)
                    {
                        TeamController.AddPlayerInTeam(1, false, 1, characterTypeList[0]);
                        TeamController.AddPlayerInTeam(1, false, 2, characterTypeList[1]);
                        TeamController.AddPlayerInTeam(2, true, 3, characterTypeList[2]);
                        TeamController.AddPlayerInTeam(2, true, 4, characterTypeList[3]);
                    }
                    else if (uiReadyController.versusMode == P2TeamMode.PLCPvsPLCP)
                    {
                        TeamController.AddPlayerInTeam(1, false, 1, characterTypeList[0]);
                        TeamController.AddPlayerInTeam(1, true, 2, characterTypeList[1]);
                        TeamController.AddPlayerInTeam(2, false, 3, characterTypeList[2]);
                        TeamController.AddPlayerInTeam(2, true, 4, characterTypeList[3]);
                    }
                }
                break;
            default:
                break;
        }

        for (int i = 0; i < TeamController.Teams.Count; ++i)
        {
            Debug.Log(TeamController.Teams[i].Players[0].SelectedCharacter.ToString());

        }

        SceneManager.LoadScene("InGame");

    }

    private bool IsCharacterNoneIdx(int idx) {
        return false;
        // return idx == 0 ? true : false;
    }

    private Color GetTeamColorByPlayerType(PlayerType pt, int plIdx)
    {
        if (uiReadyController.gameMode == GameMode.Personal)
        {
            return TeamController.GetTeamColor(plIdx + 1);
        }
        else if(uiReadyController.gameMode == GameMode.Team)
        {
            switch (pt)
            {
                case PlayerType.PL1:
                    return TeamController.GetTeamColor(1);
                case PlayerType.PL2:
                    if (uiReadyController.versusMode == P2TeamMode.PL2vsCP2)
                        return TeamController.GetTeamColor(1);
                    else if (uiReadyController.versusMode == P2TeamMode.PLCPvsPLCP)
                        return TeamController.GetTeamColor(2);
                    break;
                case PlayerType.CPU:
                    if (uiReadyController.versusMode == P2TeamMode.PL2vsCP2)
                        return TeamController.GetTeamColor(2);
                    else if (uiReadyController.versusMode == P2TeamMode.PLCPvsPLCP)
                        return TeamController.GetTeamColor(plIdx - 1);
                    else // none
                    {
                        if (uiReadyController.howPlayer == HowPlayer.P1)
                        {
                            if (plIdx < (plCnt + cpuCnt) / 2)
                            {
                                return TeamController.GetTeamColor(1);
                            }
                            else
                            { 
                                return TeamController.GetTeamColor(2);
                            }
                        }
                    }
                    break;
                default:
                    throw new UnityException("Wrong PlayerType!");
            }            
        }

        Debug.Log("RETURN WHITE?");
        return Color.white;

    }









}
