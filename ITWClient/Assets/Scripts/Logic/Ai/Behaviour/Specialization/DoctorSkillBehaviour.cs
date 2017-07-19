using UnityEngine;
using System.Collections;
using System;

namespace Ai
{

    public class DoctorSkillBehaviour : IAiBehaviour
    {
        public DoctorSkillBehaviour(ICharacterAi ai) : base(ai)
        {
            AiState = AiState.Skill;
        }

        public override int GetBehaviourPoint()
        {
            return 20;
        }

        public override void DoBehaviour()
        {
            // Doctor는 Skill쓰면서 Move도 같이할수 있으니 해당 내용 구현 필요.
        }

    }
}
