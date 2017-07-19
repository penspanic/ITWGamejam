using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ai
{
    public enum AiState
    {
        Unknown = -1, // 사용하면 안됨.

        Sleep, // Ai가동되기 전 첫 상태
        Move, // 의미없는 움직임
        Chase, // 공격을 위해 적 쫓아가는 상태
        Charge, // MP 부족해서 충전해야하는 상태
        Launch,
        Dodge, // 상대 공격을 피하는 상태
        Escape, // 위험한 상태여서 target으로부터 도망치는 상태
        Skill, // 스킬 사용중인 상태
    }
    public abstract class ICharacterAi : MonoBehaviour
    {
        public AiPlayer AiPlayer { get; protected set; }
        public AiState AiState { get; protected set; }
        public AiState PrevAiState { get; protected set; }
        public CharacterState CharacterState { get { return AiPlayer.TargetCharacter.State; } }
        public Vector3 CharacterPosition { get { return AiPlayer.TargetCharacter.transform.position; } }
        public ICharacter Character { get { return AiPlayer.TargetCharacter; } }
        public int FearPoint { get; private set; } = 0;
        public bool IsEscaping { get; set; } = false;
        public bool IsMoving { get; set; } = false;


        // 지금은 공격 대상이 캐릭터일 때만 구현하지만, 
        // 나중에 장애물 혹은 NPC등을 공격 대상으로 삼을 수도 있으니 IObject Type으로.
        public IObject AttackTarget { get; set; } = null;
        public IObject EscapeTarget { get; set; } = null;
        public IObject DodgeTarget { get; set; } = null;

        public Dictionary<AiState, IAiBehaviour> Behaviours { get; protected set; } = new Dictionary<AiState, IAiBehaviour>();

        protected virtual void CreateBehaviours()
        {
            Behaviours.Add(AiState.Sleep, new SleepBehaviour(this));
            Behaviours.Add(AiState.Move, new MoveBehaviour(this));
            Behaviours.Add(AiState.Chase, new ChaseBehaviour(this));
            Behaviours.Add(AiState.Charge, new ChargeBehaviour(this));
            Behaviours.Add(AiState.Launch, new LaunchBehaviour(this));
            Behaviours.Add(AiState.Dodge, new DodgeBehaviour(this));
            Behaviours.Add(AiState.Escape, new EscapeBehaviour(this));
            Behaviours.Add(AiState.Skill, new SkillBehaviour(this));
        }

        public void Initialize(AiPlayer player)
        {
            this.AiPlayer = player;
            AiState = AiState.Sleep;
            PrevAiState = AiState.Sleep;

            player.TargetCharacter.OnCollisionEnter += OnCharacterCollisionEnter;
            player.TargetCharacter.OnHpChanged += OnCharacterHpChanged;
            player.TargetCharacter.OnMpChanged += OnCharacterMpChanged;

            Behaviours.Clear();
            CreateBehaviours();

            StartCoroutine(TargetSettingProcess());
        }

        public void Process()
        {
            if(AttackTarget == null || AttackTarget as MonoBehaviour == null)
            {
                AttackTarget = null;
            }
            if(EscapeTarget == null || EscapeTarget as MonoBehaviour == null)
            {
                EscapeTarget = null;
            }
            if(DodgeTarget == null || DodgeTarget as MonoBehaviour == null)
            {
                DodgeTarget = null;
            }

            ProcessDangerDetact();

            // 캐릭터를 제어할 수 없는 상태에는 그냥 return 해버리기~
            if(CanProcessAi(AiPlayer.TargetCharacter.State) == false)
            {
                return;
            }

            AiState = GetTopPriorityBehaviour();

            if(AiState != PrevAiState)
            {
                Behaviours[PrevAiState].CancelBehaviour();
            }

            Behaviours[AiState].DoBehaviour();
            
            PrevAiState = AiState;
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

        protected AiState GetTopPriorityBehaviour()
        {
            // 각각의 상태에 따른 우선순위 결정을 위해 정수형으로 표현.

            var behaviourPoints = from behaviourPair in Behaviours
                                  select System.Tuple.Create(behaviourPair.Key, behaviourPair.Value.GetBehaviourPoint());

            return (from point in behaviourPoints
                    orderby point.Item2 descending
                    select point.Item1).First();

        }

        public void TryGetItem(ItemType itemType)
        {
            IItem item = ItemController.Instance.GetNearestItem(itemType, CharacterPosition);
            if(item == null)
            {
                return;
            }

            Vector2 moveDir = (item.transform.position - CharacterPosition).normalized;
            AiPlayer.TargetCharacter.DoMove(moveDir);
        }

        #region TargetSetting

        Coroutine targetSettingCoroutine;
        private void OnTargetDeath(IObject character)
        {
            AttackTarget.OnDestroyed -= OnTargetDeath;
            if (targetSettingCoroutine != null)
                StopCoroutine(targetSettingCoroutine);

            targetSettingCoroutine = StartCoroutine(TargetSettingProcess());
        }

        private IEnumerator TargetSettingProcess()
        {
            while(true)
            {
                const float minDuration = 5f;
                const float maxDuration = 15f;
                float duration = Random.Range(minDuration, maxDuration);

                if(AttackTarget != null)
                {
                    AttackTarget.OnDestroyed -= OnTargetDeath;
                }

                // 일단 가장 많이 때린 적을 타겟으로
                AttackTarget = AttackListener.Instance.GetHighestAttackEnemy(AiPlayer.TargetCharacter);

                if(AttackTarget == null) // 아직 하나도 맞은 게 없을 경우,
                {
                    // 가장 가까운 적으로 설정.
                    AttackTarget = CharacterManager.Instance.GetNearestEnemy(AiPlayer.TargetCharacter);
                }

                if(AttackTarget != null)
                {
                    AttackTarget.OnDestroyed += OnTargetDeath;
                }

                yield return new WaitForSeconds(duration);
            }
        }

        #endregion

        #region DangerDetact

        private ICharacter GetDodgeTargetEnemy()
        {
            ICharacter[] enemys = CharacterManager.Instance.GetEmemys(Character);
            for (int i = 0; i < enemys.Length; ++i)
            {
                if (enemys[i].IsHighThreat == false)
                {
                    continue;
                }

                if ((enemys[i].transform.position - CharacterPosition).magnitude < AiDifficultyController.Instance.GetStatusValue(AiConstants.DangerDetactDistance))
                {
                    return enemys[i];
                }
            }

            return null;
        }

        private float dangerDetactElapsedTime = 0f;
        private void ProcessDangerDetact()
        {
            dangerDetactElapsedTime += Time.deltaTime;
            if(dangerDetactElapsedTime < AiDifficultyController.Instance.GetStatusValue(AiConstants.DangerDetactInterval))
            {
                return;
            }

            // dodgeTarget이 없을 경우엔 시간초기화 하지 않는다.
            DodgeTarget = GetDodgeTargetEnemy();
            if(DodgeTarget == null)
            {
                return;
            }

            dangerDetactElapsedTime = 0f;
            if(AiDifficultyController.Instance.IsRandomActivated(AiConstants.DangerDetactProbability) == false)
            {
                DodgeTarget = null;
                return;
            }
        }

        #endregion

        #region Event listeners

        // ICharacter의 OnCollisionEnter Event listener 함수.
        private void OnCharacterCollisionEnter(Collision2D other)
        {
            if(IsMoving == true)
            {
                // 또 다른 처리할게 필요할까?
                IsMoving = false;
            }
        }

        private void OnCharacterHpChanged(int prevHp, int currHp)
        {
            if(prevHp == 1 && currHp > 1)
            {
                IsEscaping = false;
            }
        }

        private void OnCharacterMpChanged(int prevMp, int currMp)
        {

        }
        #endregion

        #region helpers
        public void LogAi(string message)
        {
            //[Player + N][CharcterType] message
            Debug.Log(string.Format("[Player{0}][{1}] {2}", AiPlayer.PlayerNumber, AiPlayer.TargetCharacter.CharacterType, message));
        }

        #endregion
    }
}