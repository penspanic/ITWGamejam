using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageController : Singleton<StageController>
{
    [SerializeField]
    private float maxStageTime;
    public bool IsStageStarted { get; set; }
    public float RemainElapsedTime { get; set; }
    public static bool IsEditMode = false;
    #region Event
    public event System.Action OnStageStart;
    public event System.Action OnStageEnd;
    #endregion

    private PlayerManager playerManager;
    private UiPlayerController uiPlayerController;
    private CameraController cameraController;

    protected override void Awake()
    {
        playerManager = GameObject.FindObjectOfType<PlayerManager>();
        uiPlayerController = GameObject.FindObjectOfType<UiPlayerController>();
        cameraController = GameObject.FindObjectOfType<CameraController>();
        EffectController.Instance.LoadEffects();
        Ai.AiDifficultyController.Instance.Initialize();

        IsStageStarted = false;
        RemainElapsedTime = maxStageTime;
        //int obstacleCount = Random.Range(5, 10);
        //mapController.CreateObstacles(obstacleCount);
    }

    private void Start()
    {
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
            CharacterManager.Instance.Create(currPlayer, TeamController.GetCharacterType(currPlayer.PlayerNumber));
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
        HashSet<int> aliveTeams = new HashSet<int>(); // 중복 값 제거 위해.
        foreach(var pair in CharacterManager.Instance.Characters)
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