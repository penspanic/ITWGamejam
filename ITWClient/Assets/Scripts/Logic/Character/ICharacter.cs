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
public abstract class ICharacter : MonoBehaviour
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

    public int MaxHp;
    public int MaxMp;

    public bool IsDead { get; set; }
    public int Hp { get; set; }
    public int Mp { get; set; }
    public bool IsInvincible { get; protected set; }
    public bool IsDodgeCoolTime { get; protected set; }
    public bool IsCharging { get; protected set; }
    public CharacterState State { get; protected set; }
    public CharacterType CharacterType { get; protected set; }

    protected CharacterManager characterManager;
    protected Animator animator;
    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rigidBody;
    protected Vector2 prevDirection;
    protected Vector2 prevMovedDirection;

    private Coroutine chargeCoroutine = null;
    private Coroutine poisoningCoroutine = null;
    private List<GameObject> triggeredPoisons = new List<GameObject>();
    protected virtual void Awake()
    {
        characterManager = GameObject.FindObjectOfType<CharacterManager>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        Hp = MaxHp;
        Mp = MaxMp;
    }

    protected virtual void Update()
    {

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

    public void CanMove(Vector2 normalizedDirection)
    {
        if(CanMove() == true)
        {
            Move(normalizedDirection);
        }
    }

    protected bool CanMove()
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
            bool facingRight = normalizedDirection.x > 0f;
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y = facingRight == true ? 180 : 0;
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
        if(other.gameObject.CompareTag("Character") == true)
        {
            ICharacter otherCharacter = other.gameObject.GetComponent<ICharacter>();
            switch(State)
            {
                case CharacterState.Flying:
                    otherCharacter.OnDamaged(launchDamage);
                    OnLaunchEnd();
                    transform.DOKill();
                    break;
                case CharacterState.Dodge:
                    transform.DOKill();
                    break;
                case CharacterState.Charging:
                    CancelCharge();
                    break;
                default:
                    break;
            }
        }
        else if(other.gameObject.CompareTag("Map") == true)
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
    
    public void OnDamaged(int damage)
    {
        if(IsInvincible == true)
        {
            return;
        }
        Hp -= damage;
        if(Hp <= 0)
        {
            OnDead();
        }
        else
        {
            StartCoroutine(DamageProcess());
        }
    }

    private IEnumerator DamageProcess()
    {
        IsInvincible = true;
        animator.Play("hit", 0);
        State = CharacterState.Hitted;
        yield return new WaitForSeconds(1f);
        animator.Play("idle", 0);
        State = CharacterState.Idle;
        IsInvincible = false;
    }

    private void OnDead()
    {
        IsDead = true;
        GetComponent<Collider2D>().enabled = false;
        animator.Play("death", 0);

        characterManager.OnDead(this);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Item") == true)
        {
            IItem item = other.GetComponent<IItem>();
            item.UseItem(this);
        }
        if(other.CompareTag("Poison") == true && other.GetComponent<Poison>().owner != this)
        {
            OnPoisoned(other.gameObject);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Poison") == true && other.GetComponent<Poison>().owner != this)
        {
            triggeredPoisons.Remove(other.gameObject);
            if(triggeredPoisons.Count == 0)
            {
                StopCoroutine(poisoningCoroutine);
                poisoningCoroutine = null;
            }
        }
    }

    private void OnPoisoned(GameObject poisonObject)
    {
        triggeredPoisons.Add(poisonObject);
        if(poisoningCoroutine == null)
        {
            poisoningCoroutine = StartCoroutine(PoisonedProcess());
        }
    }

    private IEnumerator PoisonedProcess()
    {
        while(true)
        {
            yield return new WaitForSeconds(2f);
            triggeredPoisons.RemoveAll((GameObject obj) => { return obj == null; });
            if(triggeredPoisons.Count > 0)
            {
                OnDamaged(1);
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

    protected bool CanDodge()
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
        // 쿨타임 체크 필요
        return true;
    }

    protected virtual void Dodge()
    {
        CancelCharge();
        IsInvincible = true;
        State = CharacterState.Dodge;
        animator.Play("evade", 0);
        StartCoroutine(DodgeProcess());
    }

    protected virtual IEnumerator DodgeProcess()
    {
        IsDodgeCoolTime = true;
        const float dodgeTime = 0.5f;
        Vector2 endPos = transform.position;
        endPos += prevMovedDirection * 1f;
        transform.DOMove(endPos, dodgeTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(dodgeTime);
        
        IsInvincible = false;
        State = CharacterState.Idle;

        yield return new WaitForSeconds(1f);
        IsDodgeCoolTime = false;
    }
}