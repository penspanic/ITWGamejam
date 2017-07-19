using UnityEngine;
using System.Collections;

namespace Ai
{
    public class RocketeerAi : ICharacterAi
    {
        protected override void CreateBehaviours()
        {
            base.CreateBehaviours();
            Behaviours[AiState.Skill] = new RocketeerSkillBehaviour(this);
        }
    }
}