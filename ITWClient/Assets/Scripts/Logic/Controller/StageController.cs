using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour
{
    [SerializeField]
    private float maxStageTime;
    public bool IsStageStarted { get; set; }
    public float RemainElapsedTime { get; set; }

    #region Event
    public event System.Action OnStageStart;
    public event System.Action OnStageEnd;
    #endregion

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
        RemainElapsedTime = maxStageTime;
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
            eachPlayer.TargetCharacter.OnDestroyed += OnCharacterDeath;
        }
        uiPlayerController.SetPlayers(playerManager.Players.ToArray());
        cameraController.SetTargets(characterObjects.ToArray());

        yield return new WaitForSeconds(3f);

        StartStage();
    }

    private void StartStage()
    {
        IsStageStarted = true;
        StartCoroutine(StageTimeProcess());
        OnStageStart();
    }

    private IEnumerator StageTimeProcess()
    {
        while(IsStageStarted == true)
        {
            yield return null;
            RemainElapsedTime -= Time.deltaTime;
        }
    }

    public void OnCharacterDeath(IObject target)
    {
        ICharacter deadCharacter = target as ICharacter;
        HashSet<int> aliveTeams = new HashSet<int>(); // 중복 값 제거 위해.
        foreach(var pair in characterManager.Characters)
        {
            int teamNumber = TeamController.GetTeam(pair.Key.PlayerNumber).TeamNumber;
            if(pair.Value.IsDead == false)
            {
                aliveTeams.Add(teamNumber);
            }
        }

        if(aliveTeams.Count == 0) // 이런 경우는 안나올 것 같음.
        {

        }
        else if(aliveTeams.Count == 1)
        {
            IsStageStarted = false;
            OnStageEnd();
        }
    }
}