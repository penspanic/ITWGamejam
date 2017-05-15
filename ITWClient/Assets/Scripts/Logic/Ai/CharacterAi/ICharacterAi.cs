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