using UnityEngine;
using System.Collections;

public enum AiState
{
    Sleep,
    Idle,
    Move,
    Charge,

}

/// <summary>
/// 대부분의 AI를 담당, 캐릭터 특화 AI는 어떻게 해야 할지 잘 모르겠음
/// PlayerAi -> PlayerController 이런식으로 가야 되나?
/// </summary>
public class PlayerAi : MonoBehaviour
{
    public AiState State { get; set; }
    Player player;
    ICharacter targetEnemy; 
    private void Awake()
    {
        State = AiState.Sleep;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    private void Update()
    {
        ProcessAi();
    }

    private void ProcessAi()
    {
        switch(State)
        {
            case AiState.Sleep:
                return;
        }
    }

    private void SetTarget()
    {
        const float minDuration = 5f;
        const float maxDuration = 15f;
        float duration = Random.Range(minDuration, maxDuration);
        StartCoroutine(TargetSettingProcess(duration, null, () => { SetTarget(); }));
    }

    private IEnumerator TargetSettingProcess(float duration, ICharacter target, System.Action afterJob)
    {
        yield return new WaitForSeconds(duration);
        afterJob();
    }
}