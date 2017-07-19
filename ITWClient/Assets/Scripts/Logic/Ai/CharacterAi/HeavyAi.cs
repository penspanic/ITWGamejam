using UnityEngine;
using System.Collections;

namespace Ai
{
    public class HeavyAi : ICharacterAi
    {
        protected override void CreateBehaviours()
        {
            base.CreateBehaviours();
            Behaviours[AiState.Skill] = new HeavySkillBehaviour(this);
        }
    }
}