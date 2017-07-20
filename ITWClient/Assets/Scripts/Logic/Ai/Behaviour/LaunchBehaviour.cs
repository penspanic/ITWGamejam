using UnityEngine;
using System.Collections;
using System;

namespace Ai
{
    public class LaunchBehaviour : IAiBehaviour
    {
        public LaunchBehaviour(ICharacterAi ai) : base(ai)
        {
            AiState = AiState.Launch;
        }

        private float additionalDetactedTime = 0f;

        public override int GetBehaviourPoint()
        {
            if (ai.Character.IsLaunchMpEnough == false)
            {
                return 0;
            }
            if(ai.AttackTarget == null)
            {
                return 0;
            }

            // 현재 Target과 나 사이의 거리가 Launch를 통해서 닿을 거리인가?
            float targetDistance = ((ai.AttackTarget as MonoBehaviour).transform.position - ai.CharacterPosition).magnitude;
            if (targetDistance < ai.Character.LaunchDistance)
            {
                return 90;
            }

            ICharacter attackTargetCharacter = ai.AttackTarget as ICharacter;
            // 헤비가 반격대기 중인지 검사
            if (attackTargetCharacter != null && attackTargetCharacter.CharacterType == CharacterType.Heavy &&
                attackTargetCharacter.State == CharacterState.Dodge &&
                AiDifficultyController.Instance.IsRandomActivated(AiConstants.HeavyDodgeDetectWhenLaunch) == true)
            {
                return 0;
            }

            if(targetDistance < ai.Character.LaunchDistance + AiDifficultyController.Instance.GetStatusValue(AiConstants.LaunchAdditionalDetactLength))
            {
                if(additionalDetactedTime + AiDifficultyController.Instance.GetStatusValue(AiConstants.LaunchAdditionalDetactInterval) < Time.time)
                {
                    additionalDetactedTime = Time.time;
                    ai.LogAi("LaunchAdditionalDetact activated, Time : " + additionalDetactedTime);
                    return 90;
                }
            }

            return 0;
        }

        public override void DoBehaviour()
        {
            // Launch 할 때 난이도에 따라 랜덤한 방향으로 하자.
            Vector2 dir = ((ai.AttackTarget as MonoBehaviour).transform.position - ai.CharacterPosition).normalized;
            float randomValue = AiDifficultyController.Instance.GetRandomRangeValue(AiConstants.LaunchRandomDirection);

            float randomX = UnityEngine.Random.Range(-randomValue, randomValue);
            float randomY = UnityEngine.Random.Range(-randomValue, randomValue);
            dir += new Vector2(randomX, randomY);
            dir.Normalize();
            
            ai.Character.FacingDirection = dir;
            ai.Character.DoLaunch();
        }

    }
}
