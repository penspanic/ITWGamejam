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

    public void InitCharacterReady() 
    {
        uiReadyController = FindObjectOfType<UiGameReadyController>();
        characterTypeList = new List<CharacterType>();

        plCnt = (int)uiReadyController.howPlayer + 1;
        cpuCnt = uiReadyController.cpuCount;

        Debug.Log(plCnt.ToString());
        Debug.Log(cpuCnt.ToString());
        selectState = SelectState.PlayerSelect;

        var currIdx = 0;
        var totalCnt = plCnt + cpuCnt;
        if (plCnt >= 1)
        {
            selecters[currIdx].InitCharacterSelecter(0, PlayerType.PL1);
            selecters[currIdx++].OnSelected(true, "P1");
        }
        if (plCnt == 2)
        {
            selecters[currIdx].InitCharacterSelecter(0, PlayerType.PL2);
            Debug.Log(currIdx);
            selecters[currIdx++].OnSelected(true, "P2");
        }
        for (int i = currIdx; i < totalCnt; ++i)
        {
            selecters[i].InitCharacterSelecter(0, PlayerType.CPU);
        }
        for (int i = totalCnt; i < selecters.Length; ++i)
        {
            selecters[i].InitCharacterSelecter(99, PlayerType.None);
        }
        isInit = true;

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
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        Debug.Log("DOWN?");
                        selecters[0].MoveNext();
                    }
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (IsCharacterNoneIdx(selecters[0].currIdx))
                        {
                            return;
                        }
                        characterTypeList.Add((CharacterType)selecters[0].DecideCharacter());
                        ++currCheckPlCnt;

                        ++checkIdx;
                        Debug.Log("checkIdx:" + checkIdx);
                    }
                }

                if (plCnt == 2)
                {
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        selecters[1].MoveNext();
                    }
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (IsCharacterNoneIdx(selecters[1].currIdx))
                        {
                            return;
                        }
                        characterTypeList.Add((CharacterType)selecters[1].DecideCharacter());
                        ++currCheckPlCnt;
                        // selecters[1].OnSelected(true);
                        ++checkIdx;
                    }
                }

                if (currCheckPlCnt >= plCnt)
                {
                    selecters[checkIdx].OnSelected(true, "CPU");
                    ++selectState;
                }
                break;
            case SelectState.CpuSelect:
                //Debug.Log(cpuCnt);
                if (currCheckCpuCnt >= cpuCnt)
                {
                    // 
                    Debug.Log("gameStart");
                    StartGame();
                    return;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    selecters[checkIdx].MoveNext();
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (IsCharacterNoneIdx(selecters[checkIdx].currIdx))
                    {
                        return;
                    }
                    characterTypeList.Add((CharacterType)selecters[checkIdx].DecideCharacter());
                    ++currCheckCpuCnt;
                    if (checkIdx <= selecters.Length - 2)
                    {
                        selecters[++checkIdx].OnSelected(true, "CPU");
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

        var plNum = (int)uiReadyController.howPlayer;
        var totalCnt = plNum + 1 + cpuCnt;

        switch (uiReadyController.gameMode)
        {
            case GameMode.Personal:
                for (int i = 0; i < totalCnt; ++i)
                {
                    TeamController.AddPlayerInTeam(i + 1, ((i + 1 <= plNum) ? false : true), i+1, characterTypeList[i]);
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
        return idx == 0 ? true : false;
    }









}
