using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ai
{
    public enum AiState
    {
        Sleep, // Ai가동되기 전 첫 상태
        Move, // 의미없는 움직임
        Chase, // 공격을 위해 적 쫓아가는 상태
        Charge, // MP 부족해서 충전해야하는 상태
        Launch,
    }
    public abstract class ICharacterAi : MonoBehaviour
    {
        public AiPlayer AiPlayer { get; protected set; }
        public AiState AiState { get; protected set; }
        public AiState PrevAiState { get; protected set; }
        public CharacterState CharacterState { get { return AiPlayer.TargetCharacter.State; } }
        public Vector3 CharacterPosition { get { return AiPlayer.TargetCharacter.transform.position; } }

        // 지금은 공격 대상이 캐릭터일 때만 구현하지만, 
        // 나중에 장애물 혹은 NPC등을 공격 대상으로 삼을 수도 있으니 IObject Type으로.
        protected IObject attackTarget = null;

        public void Initialize(AiPlayer player)
        {
            this.AiPlayer = player;
            AiState = AiState.Sleep;
            PrevAiState = AiState.Sleep;

            StartCoroutine(TargetSettingProcess());
        }

        public void Process()
        {

            // 캐릭터를 제어할 수 없는 상태에는 그냥 return 해버리기~
            if(CanProcessAi(AiPlayer.TargetCharacter.State) == false)
            {
                return;
            }

            SetBehavior();

            // 할 행동을 정한 후, 이전 AiState를 취소하자.
            CancelPrevState();

            switch(AiState) // 현재 AiState에 따른 행동을 하자.
            {
                case AiState.Sleep: // 일단 처음엔 가볍게 움직이는 것부터 시작하자.
                    AiState = AiState.Move;
                    break;
                case AiState.Charge:
                    Charge();
                    break;
                case AiState.Launch:
                    Launch();
                    break;
                case AiState.Chase:
                    Chase();
                    break;
                case AiState.Move:
                    Move();
                    break;
            }

            AiDebugRenderer.Instance.UpdateString(string.Format("State : {0} AiState : {1}", CharacterState, AiState), AiPlayer.PlayerNumber);
            // 후처리는 뭐가 필요할까??

            PrevAiState = AiState;
        }

        protected virtual void CancelPrevState()
        {
            if(PrevAiState == AiState.Charge && AiState != AiState.Charge && AiPlayer.TargetCharacter.IsCharging == true)
            {
                AiPlayer.TargetCharacter.CancelCharge();
            }
        }

        // 캐릭터 마다 다를 수 있기 때문에 하위 클래스에서 알아서 구현해서 사용하도록 한다.
        protected virtual bool CanProcessAi(CharacterState state)
        {
            switch (AiPlayer.TargetCharacter.State)
            {
                case CharacterState.Idle:
                    break;
                case CharacterState.Flying:
                    return false;
                case CharacterState.Dodge:
                    return false;
                case CharacterState.Hitted:
                    return false;
                case CharacterState.SkillActivated:
                    return false;
            }

            return true;
        }

        protected virtual void SetBehavior()
        {
            // 각각의 상태에 따른 우선순위 결정을 위해 정수형으로 표현.
            // 0 ~ 100 까지.
            Dictionary<AiState, int> behaviors = new Dictionary<AiState, int>();

            // 1. 캐릭터 상태에 따른 행동
            behaviors.Add(AiState.Charge, GetChargeBehaviorPoint());
            behaviors.Add(AiState.Launch, GetLaunchBehaviourPoint());
            behaviors.Add(AiState.Chase, GetChaseBehaviourPoint());
            behaviors.Add(AiState.Move, GetMoveBehaviourPoint());

            AiState = behaviors.OrderByDescending(behaviour => behaviour.Value).First().Key;
            // 2. 상대 행동 예측
        }

        #region Charge
        protected virtual int GetChargeBehaviorPoint()
        {
            // 익스트림 포션 먹은 상태에선 차징할 필요 없음.
            if(AiPlayer.TargetCharacter.IsExtremeMp == true)
            {
                return 0;
            }
            float remainRatio = (float)AiPlayer.TargetCharacter.Mp / (float)AiPlayer.TargetCharacter.MaxMp;
            // 일반적으로 스킬이 런치보다 마나 소모량이 많다는 가정.
            // 1. 스킬 한번 사용할 수 있는 MP가 있는가?
            if(AiPlayer.TargetCharacter.SkillNeedMp < AiPlayer.TargetCharacter.Mp)
            {
                return 30;
            }
            // 2. 런치 사용할 수 있는 MP가 있는가?
            if(AiPlayer.TargetCharacter.LaunchNeedMp < AiPlayer.TargetCharacter.Mp)
            {
                return 40;
            }

            return (int)((1f - remainRatio) * 100);
        }

        protected virtual void Charge()
        {
            // 이것만 해주면 될려나?
            // Charging 중인데 Charge 하면 캔슬되버림.
            if(AiPlayer.TargetCharacter.State != CharacterState.Charging)
            {
                AiPlayer.TargetCharacter.DoCharge();
            }
        }

        #endregion

        #region Launch
        protected virtual int GetLaunchBehaviourPoint()
        {
            if(AiPlayer.TargetCharacter.IsLaunchMpEnough == false)
            {
                return 0;
            }
            if(attackTarget == null)
            {
                return 0;
            }

            // 현재 Target과 나 사이의 거리가 Launch를 통해서 닿을 거리인가?
            float targetDistance = ((attackTarget as MonoBehaviour).transform.position - CharacterPosition).magnitude;
            if(AiPlayer.TargetCharacter.LaunchDistance > targetDistance)
            {
                return 90;
            }
            
            // 헤비가 반격대기 중인지 검사
            if(attackTarget as Heavy != null && 
                AiDifficultyController.Instance.IsRandomActivated(AiStatusIds.HeavyDodgeDetectWhenLaunch) == true)
            {
                return 0;
            }

            return 0;
        }

        protected virtual void Launch()
        {
            // Launch 할 때 난이도에 따라 랜덤한 방향으로 하자.
            Vector2 dir = ((attackTarget as MonoBehaviour).transform.position - CharacterPosition).normalized;
            float randomValue = AiDifficultyController.Instance.GetRandomValue(AiStatusIds.LaunchRandomDirectionValue);

            float randomX = Random.Range(-randomValue, randomValue);
            float randomY = Random.Range(-randomValue, randomValue);
            dir += new Vector2(randomX, randomY);
            dir.Normalize();

            AiPlayer.TargetCharacter.FacingDirection = dir;
            AiPlayer.TargetCharacter.DoLaunch();
        }

        #endregion

        #region Chase
        protected virtual int GetChaseBehaviourPoint()
        {
            // 적 쫓아가는 것 먼저 구현을 위해 일단 최고 우선순위로 설정.
            return 80;
        }

        protected virtual void Chase()
        {
            Vector2 targetDir = ((attackTarget as MonoBehaviour).transform.position - CharacterPosition).normalized;
            AiPlayer.Move(targetDir);
        }

        #endregion

        #region Move
        protected virtual int GetMoveBehaviourPoint()
        {
            return 0;
        }

        // 별 의미 없는 움직임. Chasing 하고 다름.
        private Vector2 endPos;
        private bool isMoving = false;
        protected virtual void Move()
        {
            // 첫 움직임 시작일 때 완료 목표 지점 설정
            if(isMoving == false)
            {
                isMoving = true;
                endPos = new Vector2(0, 0);
            }
        }

        #endregion

        private IEnumerator TargetSettingProcess()
        {
        
            while(true)
            {
                const float minDuration = 5f;
                const float maxDuration = 15f;
                float duration = Random.Range(minDuration, maxDuration);

                // 일단 가장 많이 때린 적을 타겟으로
                attackTarget = AttackListener.Instance.GetHighestAttackEnemy(AiPlayer.TargetCharacter);

                if(attackTarget == null) // 아직 하나도 맞은 게 없을 경우,
                {
                    // 가장 가까운 적으로 설정.
                    attackTarget = CharacterManager.Instance.GetNearestEnemy(AiPlayer.TargetCharacter);
                }

                yield return new WaitForSeconds(duration);
            }
        }
    }
}