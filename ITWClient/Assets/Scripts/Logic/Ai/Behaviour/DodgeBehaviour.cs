using UnityEngine;
using System.Collections;
using System;

namespace Ai
{
    public class DodgeBehaviour : IAiBehaviour
    {
        public DodgeBehaviour(ICharacterAi ai) : base(ai)
        {
            AiState = AiState.Dodge;
        }

        public override int GetBehaviourPoint()
        {
            if (ai.DodgeTarget != null)
            {
                return 200;
            }

            return 0;
        }

        // TODO : 캐릭터 이외의 위험상황도 감지할 필요가 있을듯.
        public override void DoBehaviour()
        {
            if (ai.DodgeTarget == null)
            {
                return;
            }

            Vector2 dodgeDirection = Vector2.zero;
            if (ai.DodgeTarget is ICharacter)
            {
                dodgeDirection = (ai.DodgeTarget as ICharacter).FacingDirection;
            }
            else
            {
                dodgeDirection = (ai.CharacterPosition - (ai.DodgeTarget as MonoBehaviour).transform.position).normalized;
            }

            ai.Character.FacingDirection = dodgeDirection;
            ai.Character.DoDodge();
        }

        
    }
}
