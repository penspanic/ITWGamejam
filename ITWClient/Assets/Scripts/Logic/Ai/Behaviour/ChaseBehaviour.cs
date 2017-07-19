using UnityEngine;
using System.Collections;
using System;

namespace Ai
{
    public class ChaseBehaviour : IAiBehaviour
    {
        public ChaseBehaviour(ICharacterAi ai) : base(ai)
        {
        }

        public override int GetBehaviourPoint()
        {
            // TODO : FearPoint가 높으면 낮은 Point를 return하도록 해야 함.
            return 80;
        }

        public override void DoBehaviour()
        {
            Vector2 targetDir = ((ai.AttackTarget as MonoBehaviour).transform.position - ai.CharacterPosition).normalized;
            // TODO : 너무 적을 최단거리로 쫓아가면 이상하니 자연스럽도록 개선해야 함.
            ai.AiPlayer.Move(targetDir);
        }
    }
}