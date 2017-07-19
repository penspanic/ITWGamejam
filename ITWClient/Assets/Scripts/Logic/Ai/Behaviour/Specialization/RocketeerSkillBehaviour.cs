using UnityEngine;
using System.Collections;
using System;

namespace Ai
{

    public class RocketeerSkillBehaviour : IAiBehaviour
    {
        public RocketeerSkillBehaviour(ICharacterAi ai) : base(ai)
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
