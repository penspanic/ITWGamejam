using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public enum CharacterType
{
    Unknown,
    Rocketeer,
    Heavy,
    Engineer,
    Doctor,
}

public enum CharacterState
{
    Idle,
    Moving,
    Flying, // Launch 후 날아갈 때
    SkillActivated,
    Hitted, // 피격 후 경직상태(무적이긴 함)
    Dodge,
    Charging,
    Poisoned,
    Dead,
}

/// <summary>
/// 기본적인 캐릭터의 구현을 담당.
// 변경이 필요할 경우 이 클래스를 상속받은 구체 클래스들이 구현한다.
/// </summary>
public abstract class ICharacter : MonoBehaviour, IObject
{
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected int launchNeedMp;
    [SerializeField]
    protected int skillNeedMp;
    [SerializeField]
    protected int launchDamage;
    [SerializeField]
    protected float launchDistance;
    [SerializeField]
    protected float launchMoveTime;
    [SerializeField]
    protected int dodgeNeedMp;
    [SerializeField]
    protected float dodgeDistance;
    [SerializeField]
    protected float dodgeCoolTime;
    [SerializeField]
    protected float dodgeDuration;

    public int MaxHp;
    public int MaxMp;

    public bool IsDead { get; set; }
    private int _hp;
    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            OnHpChanged?.Invoke(_hp, value);
            _hp = value;
        }
    }
    private int _mp;
    public int Mp
    {
        get
        {
            return _mp;
        }
        set
        {
            OnMpChanged?.Invoke(_mp, value);
            _mp = value;
        }
    }
    public bool IsExtremeMp { get; set; }
    public bool IsInvincible { get; protected set; }
    public bool IsDodgeCoolTime { get; protected set; }
    public bool IsCharging { get; protected set; }
    public bool IsFacingRight { get; protected set; }
    public int SkillNeedMp { get { return skillNeedMp; } }
    public bool IsSkillMpEnough { get { return Mp > SkillNeedMp; } }
    public int LaunchNeedMp { get { return launchNeedMp; } }
    public bool IsLaunchMpEnough { get { return Mp > launchNeedMp; } }
    public int DodgeNeedMp { get { return dodgeNeedMp; } }
    public bool IsDodgeMpEnough { get { return Mp > dodgeNeedMp; } }
    public float LaunchDistance { get { return launchDistance; } }
    public Vector2 FacingDirection // 스킬이나 런치, 닷지등 할 때 향하는 방향 | 일단 이렇게 땜빵해놓긴 하는데.. 나중에 문제 없을지 모르겠다.
    {
        get { return prevMovedDirection; }
        set { prevMovedDirection = value; RotateByFacingDirection(prevMovedDirection); }
    }
    public virtual bool IsHighThreat { get { return State == CharacterState.Flying || State == CharacterState.SkillActivated; } } // 이 캐릭터가 위협적인 상태인지 여부.
    public CharacterState State { get; protected set; }
    public CharacterType CharacterType { get; protected set; }
    public Player Player { get; protected set; }

    #region Event
    public event System.Action<IObject> OnCreated; // IObject interface implementation
    public event System.Action<IObject> OnDestroyed; // IObject interface implementation
    public event System.Action<int/*prevHp*/, int/*currHp*/> OnDamaged;
    public event System.Action<Collision2D/*other*/> OnCollisionEnter;
    public event System.Action<int/*prevHp*/, int/*currHp*/> OnHpChanged;
    public event System.Action<int/*prevMp*/, int/*currMp*/> OnMpChanged;
    #endregion

    protected CharacterManager characterManager;
    protected Animator animator;
    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rigidBody;
    protected Vector2 prevDirection;
    protected Vector2 prevMovedDirection;
    protected GameObject chargeEffect = null;
    protected SpriteAnimator characterSpriteAnimator;

    private List<Poison> triggeredPoisons = new List<Poison>();
    private Coroutine chargeCoroutine = null;
    private Coroutine poisoningCoroutine = null;
    private ParticleSystem.EmissionModule runningDustEmission;
    
    protected virtual void Awake()
    {
        characterManager = GameObject.FindObjectOfType<CharacterManager>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        characterSpriteAnimator = GetComponent<SpriteAnimator>();
        Hp = MaxHp;
        Mp = MaxMp;

        runningDustEmission = transform.Find("Running_dust").GetComponent<ParticleSystem>().emission;
    }

    protected virtual void Update()
    {
        runningDustEmission.enabled = State == CharacterState.Moving;
    }

    public void Initialize(Player player)
    {
        this.Player = player;
        OnCreated?.Invoke(this);
    }

    public virtual void ProcessKeystate(List<PlayerInputType> pressedKeys)
    {
        if(pressedKeys.Contains(PlayerInputType.Charge) == true)
        {
            DoCharge();
        }
        if(pressedKeys.Contains(PlayerInputType.Charge) == false && IsCharging == true)
        {
            CancelCharge();
        }
    }

    public void DoMove(Vector2 normalizedDirection)
    {
        if(CanMove() == true)
        {
            Move(normalizedDirection);
        }
    }

    protected virtual bool CanMove()
    {
        switch(State)
        {
            case CharacterState.Hitted:
                return false;
            case CharacterState.Flying:
                return false;
            case CharacterState.Dodge:
                return false;
            case CharacterState.Charging:
                return false;
            case CharacterState.SkillActivated:
                return false;
        }
        return true;
    }

    protected virtual void Move(Vector2 normalizedDirection)
    {
        if(normalizedDirection == Vector2.zero)
        {
            animator.Play("idle", 0);
            State = CharacterState.Idle;
        }
        else
        {
            RotateByFacingDirection(normalizedDirection);
            Vector2 velocity = normalizedDirection * moveSpeed;
            rigidBody.velocity = velocity;
            animator.Play("walk", 0);
            State = CharacterState.Moving;
            prevMovedDirection = normalizedDirection;
        }

        prevDirection = normalizedDirection;
    }

    private void RotateByFacingDirection(Vector2 facingDirection)
    {
        IsFacingRight = facingDirection.x > 0f;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y = IsFacingRight == true ? 180 : 0;
        transform.rotation = Quaternion.Euler(rotation);
    }


    public void DoUseSkill()
    {
        if(CanUseSkill() == true)
        {
            UseSkill();
        }
    }

    protected virtual bool CanUseSkill()
    {
        switch(State)
        {
            case CharacterState.SkillActivated:
                return false;
            case CharacterState.Flying:
                return false;
            case CharacterState.Hitted:
                return false;
            case CharacterState.Charging:
                return false;
            case CharacterState.Dodge:
                return false;
            default:
                break;
        }
        return Mp >= skillNeedMp;
    }

    protected virtual void UseSkill()
    {
        Mp -= skillNeedMp;
    }

    protected virtual void OnSkillEnd()
    {

    }

    public void DoLaunch()
    {
        if(CanLaunch() == true)
        {
            Launch();
        }
    }

    protected virtual bool CanLaunch()
    {
        switch(State)
        {
            case CharacterState.Flying:
                return false;
            case CharacterState.Hitted:
                return false;
            case CharacterState.Dodge:
                return false;
            case CharacterState.Charging:
                return false;
            case CharacterState.SkillActivated:
                return false;
            default:
                break;
        }
        return Mp >= launchNeedMp;
    }

    protected virtual void Launch()
    {
        Mp -= launchNeedMp;
        animator.Play("flying", 0);
        State = CharacterState.Flying;
        IsInvincible = true;
        Vector2 endPos = transform.position;
        endPos += prevMovedDirection * launchDistance;
        transform.DOMove(endPos, launchMoveTime).OnComplete(() => { OnLaunchEnd(); }).SetEase(Ease.Linear);
    }

    protected virtual void OnLaunchEnd()
    {
        if(State == CharacterState.Flying)
        {
            StartCoroutine(LandingProcess());
        }
    }

    private IEnumerator LandingProcess()
    {
        animator.Play("landing", 0);
        SfxManager.Instance.Play(SfxType.Character_Landing);
        yield return new WaitForSeconds(0.12f);
        State = CharacterState.Idle;
        IsInvincible = false;
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        OnCollisionEnter?.Invoke(other);

        if (other.gameObject.CompareTag(TagNames.Character) == true)
        {
            ICharacter otherCharacter = other.gameObject.GetComponent<ICharacter>();
            switch(State)
            {
                case CharacterState.Flying:
                    otherCharacter.OnHit(this, launchDamage);
                    OnLaunchEnd();
                    transform.DOKill();
                    break;
                case CharacterState.Dodge:
                    transform.DOKill();
                    OnDodgeEnd();
                    break;
                case CharacterState.Charging:
                    CancelCharge();
                    break;
                default:
                    break;
            }
        }
        else if(other.gameObject.CompareTag(TagNames.Map) == true)
        {
            switch(State)
            {
                case CharacterState.Flying:
                    transform.DOKill();
                    OnLaunchEnd();
                    break;
                case CharacterState.Dodge:
                    transform.DOKill();
                    break;
                default:
                    break;
            }
            rigidBody.velocity = Vector2.zero;
        }
    }

    protected virtual void CallEndEventWhenHit()
    {
        switch(State)
        {
            case CharacterState.Flying:
                transform.DOKill();
                OnDodgeEnd();
                break;
            case CharacterState.Charging:
                CancelCharge();
                break;
            default:
                break;
        }
    }

    // IObject interface implementation
    public virtual void OnHit(IObject attacker, int damage, bool forced = false)
    {
        if(IsInvincible == true && forced == false)
        {
            return;
        }
        CallEndEventWhenHit();
        int prevHp = Hp;
        Hp -= damage;
        if(Hp <= 0)
            Hp = 0;

        OnDamaged?.Invoke(prevHp, Hp);
        Ai.AttackListener.Instance.OnDamaged(attacker, this, damage);
        if(Hp == 0)
        {
            OnDeath();
        }
        else
        {
            SfxManager.Instance.Play(SfxType.Character_Hit);
            StartCoroutine(DamageProcess(attacker));
        }
    }

    private IEnumerator DamageProcess(IObject attacker)
    {
        const float invincibleTime = 1f;
        EffectController.Instance.ShowEffect(EffectType.Hit, new Vector2(0f, 0.1f), transform);
        characterSpriteAnimator.SetColor(new Color(255f / 255f, 109f / 255f, 109f / 255f));
        StartCoroutine(characterSpriteAnimator.Twinkle(invincibleTime, 0.2f));
        animator.Play("hit", 0);

        IsInvincible = true;
        State = CharacterState.Hitted;

        Vector2 hitDir = this.transform.position - (attacker as MonoBehaviour).transform.position;
        hitDir.Normalize();
        const float hitMoveDistance = 0.5f;
        const float hitMoveTime = 0.3f;
        Vector2 endPos = this.transform.position;
        endPos += hitDir * hitMoveDistance;
        transform.DOMove(endPos, hitMoveTime);

        SetCollidable(false);

        yield return new WaitForSeconds(invincibleTime);

        SetCollidable(true);
        State = CharacterState.Idle;
        IsInvincible = false;

        characterSpriteAnimator.SetColor(Color.white);
        animator.Play("idle", 0);
    }


    protected virtual void OnDeath()
    {
        Debug.Log(this.name + " Dead.");
        State = CharacterState.Dead;
        IsDead = true;
        GetComponent<Collider2D>().enabled = false;
        animator.Play("death", 0);
        EffectController.Instance.ShowEffect(EffectType.Die, new Vector2(0f, 0.1f), this.transform);
        OnDestroyed?.Invoke(this);

        StartCoroutine(DisappearProcess());
    }

    private IEnumerator DisappearProcess()
    {
        yield return new WaitForSeconds(1f);

        const float ALPHA_TIME = 0.5f;
        float elapsedTime = 0f;
        while(elapsedTime < ALPHA_TIME)
        {
            elapsedTime += Time.deltaTime;
            characterSpriteAnimator.SetColor(new Color(1, 1, 1, 1 - elapsedTime / ALPHA_TIME));
            yield return null;
        }
        Destroy(this.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(TagNames.Item) == true)
        {
            IItem item = other.GetComponent<IItem>();
            item.UseItem(this);
        }
        if(other.CompareTag(TagNames.Poison) == true)
        {
            OnPoisoned(other.gameObject.GetComponent<Poison>());
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag(TagNames.Poison) == true)
        {
            triggeredPoisons.Remove(other.gameObject.GetComponent<Poison>());
            if(triggeredPoisons.Count == 0)
            {
                StopCoroutine(poisoningCoroutine);
                poisoningCoroutine = null;
            }
        }
    }

    private void OnPoisoned(Poison poison)
    {
        triggeredPoisons.Add(poison);
        if(poisoningCoroutine == null)
        {
            poisoningCoroutine = StartCoroutine(PoisonedProcess(poison));
        }
    }

    private IEnumerator PoisonedProcess(Poison poison)
    {
        while(true)
        {
            yield return new WaitForSeconds(1.3f);
            triggeredPoisons.RemoveAll((Poison eachPoison) => { return eachPoison == null; });
            if(triggeredPoisons.Count > 0)
            {
                OnHit(poison as IObject, 1);
            }
        }
    }

    public void DoCharge()
    {
        if(CanCharge() == true)
        {
            Charge();
        }
    }

    protected bool CanCharge()
    {
        switch(State)
        {
            case CharacterState.Flying:
                return false;
            case CharacterState.SkillActivated:
                return false;
            case CharacterState.Hitted:
                return false;
            default:
                break;
        }
        return IsCharging == false;
    }

    protected virtual void Charge()
    {
        animator.Play("charge", 0);
        State = CharacterState.Charging;
        chargeEffect = EffectController.Instance.ShowEffect(EffectType.Charge, new Vector2(0f, 0.2f), transform);
        chargeCoroutine = StartCoroutine(ChargeProcess());
    }

    private IEnumerator ChargeProcess()
    {
        IsCharging = true;
        float elapsedTime = 0f;
        SfxManager.Instance.PlayLoop(SfxType.Character_Charge);
        while(true)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            if(elapsedTime > 0.1f)
            {
                elapsedTime -= 0.1f;
                Mp += 20;
            }
            if(Mp > MaxMp)
            {
                Mp = MaxMp;
            }
        }
    }

    private void StopCharging()
    {
        if(IsCharging == true)
        {
            SfxManager.Instance.StopLoop(SfxType.Character_Charge);
            StopCoroutine(chargeCoroutine);
            IsCharging = false;
            Destroy(chargeEffect);
            chargeEffect = null;
        }
    }

    public void CancelCharge()
    {
        State = CharacterState.Idle;
        animator.Play("idle", 0);
        StopCharging();
    }

    public void DoDodge()
    {
        if(CanDodge() == true)
        {
            Dodge();
        }
    }


    protected virtual bool CanDodge()
    {
        switch(State)
        {
            case CharacterState.Hitted:
                return false;
            case CharacterState.Flying:
                return false;
            case CharacterState.SkillActivated:
                return false;
            case CharacterState.Dodge:
                return false;
            case CharacterState.Charging:
                return false;
            default:
                break;
        }
        if(Mp <= dodgeNeedMp)
        {
            return false;
        }
        return IsDodgeCoolTime == false;
    }

    protected virtual void Dodge()
    {
        Mp -= dodgeNeedMp;
        IsInvincible = true;
        State = CharacterState.Dodge;
        animator.Play("evade", 0);
        SetCollidable(false);
        StartCoroutine(DodgeProcess());
    }

    protected virtual void OnDodgeEnd()
    {
        IsInvincible = false;
        SetCollidable(true);
        State = CharacterState.Idle;
    }

    protected virtual IEnumerator DodgeProcess()
    {
        IsDodgeCoolTime = true;
        Vector2 endPos = transform.position;
        endPos += prevMovedDirection * dodgeDistance;
        transform.DOMove(endPos, dodgeDuration).SetEase(Ease.Linear);
        yield return new WaitForSeconds(dodgeDuration);

        if(State == CharacterState.Dodge) // 중간에 다른데서 State를 바꿨을 수도 있음.
        {
            OnDodgeEnd();
        }

        yield return new WaitForSeconds(dodgeCoolTime - dodgeDuration);

        IsDodgeCoolTime = false;
    }

    protected void SetCollidable(bool value)
    {
        if(value == true)
            gameObject.layer = LayerMask.NameToLayer(LayerNames.Team + Player.TeamNumber.ToString());
        else
            gameObject.layer = LayerMask.NameToLayer(LayerNames.NonCollidable);
    }
}