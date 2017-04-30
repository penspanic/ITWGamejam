using UnityEngine;
using System.Collections;
using DG;

public class Heavy : ICharacter
{
    [SerializeField]
    private float skillMoveTime;
    [SerializeField]
    private float skillMoveDistance;

    private Coroutine skillCoroutine = null;
    protected override void Awake()
    {
        base.Awake();
        CharacterType = CharacterType.Heavy;
    }

    protected override bool CanMove()
    {
        if(State == CharacterState.SkillActivated)
        {
            return false;
        }
        return base.CanMove();
    }
    protected override void UseSkill()
    {
        base.UseSkill();
        animator.Play("skill", 0);
        IsInvincible = true;
        State = CharacterState.SkillActivated;
        skillCoroutine = StartCoroutine(SkillProcess());
    }

    private IEnumerator SkillProcess()
    {
        float elapsedTime = 0f;
        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + (prevMovedDirection * skillMoveDistance);
        
        while(elapsedTime < skillMoveTime)
        {
            yield return null;
            animator.enabled = false;
            elapsedTime += Time.deltaTime;
            transform.position = AnimationHelper.Arc(startPos, endPos, 0.5f, elapsedTime / skillMoveTime);
            if(elapsedTime / skillMoveTime > 0.3f)
            {
                animator.enabled = true;
            }
        }
        transform.position = endPos;

        if(State == CharacterState.SkillActivated)
        {
            OnSkillEnd();
        }
    }

    protected override void OnSkillEnd()
    {
        if(skillCoroutine != null)
        {
            StopCoroutine(skillCoroutine);
            skillCoroutine = null;
        }
        animator.enabled = true;
        State = CharacterState.Idle;
        IsInvincible = false;
    }

    protected override IEnumerator DodgeProcess()
    {
        IsDodgeCoolTime = true;
        yield return new WaitForSeconds(dodgeDuration);

        if(State == CharacterState.Dodge)
        {
            OnDodgeEnd();
        }

        yield return new WaitForSeconds(dodgeCoolTime - dodgeDuration);

        IsDodgeCoolTime = false;
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Character") == true)
        {
            ICharacter otherCharacter = other.gameObject.GetComponent<ICharacter>();
            if(otherCharacter.State == CharacterState.Flying
                && this.State == CharacterState.Dodge)
            {
                otherCharacter.OnDamaged(this, 1, true);
                return;
            }
        }
        else if(other.gameObject.CompareTag("Map") == true)
        {
            if(State == CharacterState.SkillActivated)
            {
                OnSkillEnd();
                return;
            }
        }
        base.OnCollisionEnter2D(other);
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}