using UnityEngine;
using System.Collections;
using System;

namespace Ai
{
    public class EscapeBehaviour : IAiBehaviour
    {
        public EscapeBehaviour(ICharacterAi ai) : base(ai)
        {
            AiState = AiState.Escape;
        }

        public override int GetBehaviourPoint()
        {
            // 일단 도망치는건 체력이 1 남았을 때만 하자.
            if (ai.Character.Hp == 1)
            {
                return 100;
            }

            return 0;
        }

        public override void DoBehaviour()
        {
            // Launch나 Heavy 스킬등으로 안전한 위치를 향해 사용하면 될 것 같은데..
            // 위의 방법은 마나 있을 때 하면 될 듯.
            // 마나가 없는 경우엔 가장 가까이 있는 적의 반대편으로 움직이자.
            // HP 아이템이 있으면 먹으러 가자.

            ai.IsEscaping = true;
            ai.EscapeTarget = CharacterManager.Instance.GetNearestEnemy(ai.Character);

            float targetDistance = float.MaxValue;
            if (ai.EscapeTarget != null)
            {
                targetDistance = ((ai.EscapeTarget as ICharacter).transform.position - ai.CharacterPosition).magnitude;
            }

            // 일정 범위 안에 있을 경우, 도망친다.
            const float TOLERABLE_ENEMY_DISTANCE = 1f;
            if (targetDistance < TOLERABLE_ENEMY_DISTANCE) // 이것도 난이도마다 다르게 설정할 필요가 있을려나...
            {
                ai.LogAi(string.Format("(Escape) Escaping from {0}, distance : {1}", (ai.EscapeTarget as MonoBehaviour).name, targetDistance));
            }
            else // 일정 범위 밖에 있을 경우, HP 포션을 먹으러 간다.
            {
                ai.LogAi(string.Format("(Escape) Try get HpPotion"));
                ai.TryGetItem(ItemType.HpPotion);
            }
        }

        public override void CancelBehaviour()
        {
            ai.IsEscaping = false;
        }
    }
}
