using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour
{
    [SerializeField]
    private float maxStageTime;
    public bool IsStageStarted { get; set; }
    public float RemainElapsedTime { get; set; }
    public static bool IsEditMode = true;
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
        EffectController.Instance.LoadEffects();

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
        Vector2[] createPositions = TeamController.GetCharacterCreatePos(playerManager.Players.Count);
        for(int i = 0; i < playerManager.Players.Count;++i)
        {
            Player currPlayer = playerManager.Players[i];
            characterManager.Create(currPlayer, TeamController.GetCharacterType(currPlayer.PlayerNumber));
            characterObjects.Add(currPlayer.TargetCharacter.gameObject);
            currPlayer.TargetCharacter.OnDestroyed += OnCharacterDeath;
            currPlayer.TargetCharacter.transform.position = createPositions[i];
        }
        uiPlayerController.SetPlayers(playerManager.Players.ToArray());
        cameraController.SetTargets(characterObjects.ToArray());

        if(IsEditMode == true)
        {
            StartCoroutine(StageTimeProcess());
            OnStageStart();
            IsStageStarted = true;
            yield break;
        }

        yield return new WaitForSeconds(3f);

        StartCoroutine(StageTimeProcess());
        OnStageStart();

        yield return new WaitForSeconds(3f);
        IsStageStarted = true;
    }

    private IEnumerator StageTimeProcess()
    {
        while(true)
        {
            yield return null;

            if(IsStageStarted == true)
            {
                RemainElapsedTime -= Time.deltaTime;
            }
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