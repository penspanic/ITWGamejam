using System;
using System.Collections.Generic;
using System.Linq;

namespace Ai
{

    public abstract class IAiBehaviour
    {
        public IAiBehaviour(ICharacterAi ai)
        {
            this.ai = ai;
        }

        public AiState AiState { get; protected set; } = AiState.Unknown;
        protected ICharacterAi ai;

        public abstract int GetBehaviourPoint();

        // return type이 bool이어야 할 필요가 있을까?
        public abstract void DoBehaviour();
        public virtual void CancelBehaviour() { }
    }
}
