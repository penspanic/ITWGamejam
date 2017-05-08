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
}

/// <summary>
/// 각 캐릭터 마다의 상세 구현(스킬 등)을 담당.
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
    public int Hp { get; set; }
    public int Mp { get; set; }
    public bool IsExtremeMp { get; set; }
    public bool IsInvincible { get; protected set; }
    public bool IsDodgeCoolTime { get; protected set; }
    public bool IsCharging { get; protected set; }
    public bool IsFacingRight { get; protected set; }
    public CharacterState State { get; protected set; }
    public CharacterType CharacterType { get; protected set; }
    public Player Player { get; protected set; }

    #region Event
    public event OnObjectDestroyed OnDestroyed;
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
    
    protected virtual void Awake()
    {
        characterManager = GameObject.FindObjectOfType<CharacterManager>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        characterSpriteAnimator = GetComponent<SpriteAnimator>();
        Hp = MaxHp;
        Mp = MaxMp;
    }

    protected virtual void Update()
    {

    }

    public void Initialize(Player player)
    {
        this.Player = player;
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
            IsFacingRight = normalizedDirection.x > 0f;
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y = IsFacingRight == true ? 180 : 0;
            transform.rotation = Quaternion.Euler(rotation);

            rigidBody.velocity = normalizedDirection * moveSpeed;
            animator.Play("walk", 0);
            State = CharacterState.Moving;
            prevMovedDirection = normalizedDirection;
        }

        prevDirection = normalizedDirection;
    }

    /*
    CanUse~()는 구체 클래스에서 재구현 할 수 있도록 virtual
    */
    
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

    protected void Launch()
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
        StartCoroutine(LandingProcess());
    }

    private IEnumerator LandingProcess()
    {
        animator.Play("landing", 0);
        yield return new WaitForSeconds(0.12f);
        State = CharacterState.Idle;
        IsInvincible = false;
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag(TagNames.Character) == true)
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

    public virtual void OnHit(IObject attacker, int damage, bool forced = false)
    {
        if(IsInvincible == true && forced == false)
        {
            return;
        }
        Hp -= damage;
        if(Hp <= 0)
        {
            OnDeath();
        }
        else
        {
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

    private void OnDeath()
    {
        IsDead = true;
        GetComponent<Collider2D>().enabled = false;
        animator.Play("death", 0);
        EffectController.Instance.ShowEffect(EffectType.Die, Vector2.zero, this.transform);
        OnDestroyed(this);
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