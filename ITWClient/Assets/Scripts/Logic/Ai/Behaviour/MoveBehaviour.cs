using UnityEngine;
using System.Collections;
using System;

namespace Ai
{
    // 별 의미 없는 움직임. Chasing 하고 다름.
    public class MoveBehaviour : IAiBehaviour
    {
        private Vector2 moveEndPos;
        public MoveBehaviour(ICharacterAi ai) : base(ai)
        {
            AiState = AiState.Move;
        }

        public override int GetBehaviourPoint()
        {
            if (ai.IsMoving == true)
            {
                return 60;
            }
            return 70; // 임시
        }

        public override void DoBehaviour()
        {
            // 첫 움직임 시작일 때 완료 목표 지점 설정
            if (ai.IsMoving == false)
            {
                ai.IsMoving = true;
                moveEndPos = MapController.GetRandomMapPos();
                ai.LogAi("MoveStart, EndPos : " + moveEndPos);
                return;
            }

            Vector2 moveDir = (moveEndPos - new Vector2(ai.CharacterPosition.x, ai.CharacterPosition.y)).normalized;
            ai.Character.DoMove(moveDir);

            float remainDistance = (new Vector2(ai.CharacterPosition.x, ai.CharacterPosition.y) - moveEndPos).magnitude;
            if (remainDistance < 0.05f)
            {
                ai.IsMoving = false;
            }
        }

    }
}
