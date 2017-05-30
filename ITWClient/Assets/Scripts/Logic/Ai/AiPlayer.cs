using UnityEngine;
using System.Collections;



namespace Ai
{
    /// <summary>
    /// 플레이어로써의 AI를 담당, 캐릭터 특화 AI는 ICharacterAi를 통해 구현
    /// PlayerAi -> PlayerController 이런식으로 가야 되나?
    /// </summary>
    public class AiPlayer : Player
    {
        private ICharacterAi characterAi = null;

        protected override void Awake()
        {
            StageController.Instance.OnStageStart += OnStageStart;
        }

        protected override void Update()
        {
            base.Update();
            // 원래 여기서 FixedUpdate에서 하던 일을 했는데, Update문에서 velocity 관련해서 처리하면 
            // 물리 처리가 이상해지는게 있어서(AiCharacter Move가 vector대로 이동을 안함) FixedUpdate로 옮겼다.
            // RigidBody2D.velocity를 직접 수정하지 말라는 unity answer들이 많긴 한데 일단 시간이 없으니 이렇게 처리...
        }

        private void FixedUpdate()
        {
            if(characterAi == null || TargetCharacter == null)
            {
                return;
            }
            if (TargetCharacter.IsDead == false && StageController.Instance.IsStageStarted == true)
            {
                characterAi.Process();
            }

            AiDebugRenderer.Instance.UpdateString(string.Format("State : {0} AiState : {1}", TargetCharacter.State, characterAi.AiState), PlayerNumber);
        }

        public override void SetCharacter(ICharacter character)
        {
            base.SetCharacter(character);

            if (characterAi != null)
            {
                Destroy(characterAi);
                characterAi = null;
            }

            if(StageController.Instance.IsDisableAi == true)
            {
                return;
            }

            switch(character.CharacterType)
            {
                case CharacterType.Rocketeer:
                    characterAi = gameObject.AddComponent<RocketeerAi>();
                    break;
                case CharacterType.Heavy:
                    characterAi = gameObject.AddComponent<HeavyAi>();
                    break;
                case CharacterType.Engineer:
                    characterAi = gameObject.AddComponent<EngineerAi>();
                    break;
                case CharacterType.Doctor:
                    characterAi = gameObject.AddComponent<DoctorAi>();
                    break;
                default:
                    Debug.LogErrorFormat("Create CharcterAi failed, CharacterType : {0} name : {1}", character.CharacterType, character.name);
                    return;
            }

        }

        private void OnStageStart()
        {
            // 캐릭터가 동적으로 바뀌는 경우에 대한 것은 따로 고려해봐야 할 듯..
            if(characterAi != null)
            {
                characterAi.Initialize(this);
            }
        }
    }
}