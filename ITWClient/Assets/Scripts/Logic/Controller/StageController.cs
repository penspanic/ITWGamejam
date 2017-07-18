using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageController : Singleton<StageController>
{
    [SerializeField]
    private float maxStageTime;
    [SerializeField]
    private bool isDisableAi;
    public bool IsStageProcessing { get; private set; }
    public float RemainElapsedTime { get; private set; }
    public bool IsDisableAi { get { return isDisableAi; } }
    public static bool IsEditMode = true;
    #region Event
    public event System.Action OnStageStart;
    public event System.Action<int/*WinTeamNumber*/> OnStageEnd;
    #endregion

    private PlayerManager playerManager;
    private CameraController cameraController;
    private UiPlayerController uiPlayerController;
    private UiStageController uiStageController;

    protected override void Awake()
    {
        playerManager = GameObject.FindObjectOfType<PlayerManager>();
        cameraController = GameObject.FindObjectOfType<CameraController>();
        uiPlayerController = GameObject.FindObjectOfType<UiPlayerController>();
        uiStageController = GameObject.FindObjectOfType<UiStageController>();

        EffectController.Instance.LoadEffects();
        SfxManager.Instance.Initialize();
        Ai.AiDifficultyController.Instance.Initialize();

        IsStageProcessing = false;
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
        // Create Map
        MapController.Instance.CreateMap(MapType.FourGimmick);

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

        if(IsEditMode == false)
        {
            yield return new WaitForSeconds(3f);
        }

        StartCoroutine(StageTimeProcess());
        BgmManager.Instance.Play(BgmType.InGame1);
        uiStageController.ShowReadyStart();
        
        if(IsEditMode == false)
        {
            yield return new WaitForSeconds(3f);
        }

        IsStageProcessing = true;
        OnStageStart();
    }

    private IEnumerator StageTimeProcess()
    {
        while(true)
        {
            yield return null;

            if(IsStageProcessing == true)
            {
                RemainElapsedTime -= Time.deltaTime;
                if(RemainElapsedTime < 0f)
                {
                    IsStageProcessing = false;
                    OnStageEnd(GetWinTeamNumber());
                    yield break;
                }
            }
        }
    }

    public void OnCharacterDeath(IObject target)
    {
        var aliveTeamNumbers = (from aliveCharcter in CharacterManager.Instance.Characters
                                where aliveCharcter.Value.IsDead == false
                                select aliveCharcter.Value.Player.TeamNumber).Distinct();
                               
        if(aliveTeamNumbers.Count() == 0)
        {
            Debug.LogError("Alive team numbers is 0.");
            IsStageProcessing = false;
            OnStageEnd(-1/*draw*/);
            return;
        }
        else if(aliveTeamNumbers.Count() == 1)
        {
            IsStageProcessing = false;
            OnStageEnd(GetWinTeamNumber());
        }
        else
        {
            bool isPlayerTeamExist = false;
            foreach(int teamNumber in aliveTeamNumbers)
            {
                if(TeamController.GetTeamByPlayerNumber(teamNumber).IsCpuTeam() == false)
                {
                    isPlayerTeamExist = true;
                    break;
                }
            }

            if(isPlayerTeamExist == false)
            {
                IsStageProcessing = false;
                OnStageEnd(GetWinTeamNumber());
            }
        }
    }

    /// <returns>Draw시 -1 리턴.</returns>
    private int GetWinTeamNumber()
    {
        Dictionary<int/*TeamNumber*/, int/*RemainHp*/> hpDatas = new Dictionary<int, int>();

        foreach (var pair in CharacterManager.Instance.Characters)
        {
            int teamNumber = TeamController.GetTeamByPlayerNumber(pair.Key.PlayerNumber).TeamNumber;
            if(hpDatas.ContainsKey(teamNumber)==false)
            {
                hpDatas.Add(teamNumber, 0);
            }
            hpDatas[teamNumber] += pair.Value.Hp;
        }

        var sorted = (from hpPair in hpDatas
                      orderby hpPair.Value descending
                      select hpPair).ToArray();

        if(sorted.Length == 0)
        {
            return -1;
        }

        if(TeamController.GetTeamByPlayerNumber(sorted[0].Key).IsCpuTeam() == true)
        {
            return sorted[0].Key;
        }

        // 동점 처리는 Player Team끼리만 함.
        int topRemainHp = sorted[0].Value;
        for (int i = 1; i < sorted.Length; ++i)
        {
            if (sorted[i].Value == topRemainHp)
            {
                return -1;
            }
        }

        return sorted[0].Key;
    }
}