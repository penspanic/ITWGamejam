using UnityEngine;
using System.Collections;

namespace Ai
{
    public class DoctorAi : ICharacterAi
    {
        protected override void CreateBehaviours()
        {
            base.CreateBehaviours();
            Behaviours[AiState.Skill] = new DoctorSkillBehaviour(this);
        }
    }
}