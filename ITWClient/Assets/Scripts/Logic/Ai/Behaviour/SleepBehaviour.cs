using UnityEngine;
using System.Collections;
using System;

namespace Ai
{
    public class SleepBehaviour : IAiBehaviour
    {
        public SleepBehaviour(ICharacterAi ai) : base(ai)
        {
            AiState = AiState.Sleep;
        }

        public override int GetBehaviourPoint()
        {
            return 0;
        }

        public override void DoBehaviour()
        {

        }
    }
}