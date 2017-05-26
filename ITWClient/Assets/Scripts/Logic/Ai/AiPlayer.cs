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
            if(characterAi != null && StageController.Instance.IsStageStarted == true)
            {
                characterAi.Process();
            }
        }
        public override void SetCharacter(ICharacter character)
        {
            base.SetCharacter(character);

            if (characterAi != null)
            {
                Destroy(characterAi);
                characterAi = null;
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