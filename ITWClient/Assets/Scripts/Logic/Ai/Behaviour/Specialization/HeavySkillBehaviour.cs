using UnityEngine;
using System.Collections;
using System;

namespace Ai
{

    public class HeavySkillBehaviour : IAiBehaviour
    {
        public HeavySkillBehaviour(ICharacterAi ai) : base(ai)
        {
            AiState = AiState.Skill;
        }

        public override int GetBehaviourPoint()
        {
            return 20;
        }

        public override void DoBehaviour()
        {

        }
    }
}
