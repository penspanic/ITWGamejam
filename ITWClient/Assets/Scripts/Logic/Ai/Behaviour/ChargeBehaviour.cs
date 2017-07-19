using UnityEngine;
using System.Collections;
using System;

namespace Ai
{
    public class ChargeBehaviour : IAiBehaviour
    {
        public ChargeBehaviour(ICharacterAi ai) : base(ai)
        {
            AiState = AiState.Charge;
        }

        public override int GetBehaviourPoint()
        {
            // 익스트림 포션 먹은 상태에선 차징할 필요 없음.
            if (ai.Character.IsExtremeMp == true)
            {
                return 0;
            }
            float remainRatio = (float)ai.Character.Mp / (float)ai.Character.MaxMp;
            // 일반적으로 스킬이 런치보다 마나 소모량이 많다는 가정.
            // 1. 스킬 한번 사용할 수 있는 MP가 있는가?
            if (ai.Character.IsSkillMpEnough)
            {
                return 30;
            }
            // 2. 런치 사용할 수 있는 MP가 있는가?
            if (ai.Character.IsLaunchMpEnough)
            {
                return 40;
            }

            return (int)((1f - remainRatio) * 100);
        }

        public override void DoBehaviour()
        {
            // 이것만 해주면 될려나?
            // Charging 중인데 Charge 하면 캔슬되버림.
            if (ai.Character.State != CharacterState.Charging)
            {
                ai.Character.DoCharge();
            }
        }

        public override void CancelBehaviour()
        {
            if(ai.Character.State == CharacterState.Charging)
            {
                ai.Character.CancelCharge();
            }
        }
    }
}
