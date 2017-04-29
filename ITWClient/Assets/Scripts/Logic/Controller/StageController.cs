using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour
{
    public bool IsStageStarted { get; set; }

    private ItemController itemController;
    private MapController mapController;
    private CharacterFactory characterFactory;
    private PlayerManager playerManager;
    private CharacterManager characterManager;
    private UiPlayerController uiPlayerController;

    private void Awake()
    {
        itemController = GameObject.FindObjectOfType<ItemController>();
        mapController = GameObject.FindObjectOfType<MapController>();
        characterFactory = GameObject.FindObjectOfType<CharacterFactory>();
        playerManager = GameObject.FindObjectOfType<PlayerManager>();
        characterManager = GameObject.FindObjectOfType<CharacterManager>();
        uiPlayerController = GameObject.FindObjectOfType<UiPlayerController>();

        IsStageStarted = false;
        int obstacleCount = Random.Range(5, 10);
        //mapController.CreateObstacles(obstacleCount);

        StartCoroutine(StageStartProcess());
    }

    private IEnumerator StageStartProcess()
    {
        playerManager.CreatePlayers();
        foreach(Player eachPlayer in playerManager.Players)
        {
            characterManager.Create(eachPlayer, TeamController.GetCharacterType(eachPlayer.PlayerNumber));
        }
        uiPlayerController.SetPlayers(playerManager.Players.ToArray());

        yield return new WaitForSeconds(5f);

        StartStage();
    }

    private void StartStage()
    {
        IsStageStarted = true;
    }
}