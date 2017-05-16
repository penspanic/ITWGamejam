using UnityEngine;
using System.Collections;

public enum AiState
{
    Sleep,
    Idle,
    Move,
    Charge,
}

public abstract class ICharacterAi : MonoBehaviour
{
    public AiPlayer AiPlayer { get; set; }

    // 지금은 공격 대상이 캐릭터일 때만 구현하지만, 
    // 나중에 장애물 혹은 NPC등을 공격 대상으로 삼을 수도 있으니 IObject Type으로.
    protected IObject attackTarget = null;
    public void Process()
    {
        switch(AiPlayer.TargetCharacter.State)
        {
            case CharacterState.Idle:
                break;
        }
    }

    #region TargetSetting
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
    #endregion
}