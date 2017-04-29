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
    private CameraController cameraController;

    private void Awake()
    {
        itemController = GameObject.FindObjectOfType<ItemController>();
        mapController = GameObject.FindObjectOfType<MapController>();
        characterFactory = GameObject.FindObjectOfType<CharacterFactory>();
        playerManager = GameObject.FindObjectOfType<PlayerManager>();
        characterManager = GameObject.FindObjectOfType<CharacterManager>();
        uiPlayerController = GameObject.FindObjectOfType<UiPlayerController>();
        cameraController = GameObject.FindObjectOfType<CameraController>();

        IsStageStarted = false;
        int obstacleCount = Random.Range(5, 10);
        //mapController.CreateObstacles(obstacleCount);

        StartCoroutine(StageStartProcess());
    }

    private IEnumerator StageStartProcess()
    {
        playerManager.CreatePlayers();
        List<GameObject> characterObjects = new List<GameObject>();
        foreach(Player eachPlayer in playerManager.Players)
        {
            characterManager.Create(eachPlayer, TeamController.GetCharacterType(eachPlayer.PlayerNumber));
            characterObjects.Add(eachPlayer.TargetCharacter.gameObject);
        }
        uiPlayerController.SetPlayers(playerManager.Players.ToArray());
        cameraController.SetTargets(characterObjects.ToArray());
        yield return new WaitForSeconds(5f);

        StartStage();
    }

    private void StartStage()
    {
        IsStageStarted = true;
    }
}