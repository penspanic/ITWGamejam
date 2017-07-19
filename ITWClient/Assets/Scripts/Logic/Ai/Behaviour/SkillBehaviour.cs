using UnityEngine;
using System.Collections;
using System;

namespace Ai
{

    public class SkillBehaviour : IAiBehaviour
    {
        public SkillBehaviour(ICharacterAi ai) : base(ai)
        {
        }

        public override int GetBehaviourPoint()
        {
            // 스킬은 캐릭터 마다 다르니 하위클래스에서 구현하도록 함.
            return 0;
        }

        public override void DoBehaviour()
        {
            throw new NotImplementedException();
        }
    }
}
