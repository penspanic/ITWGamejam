using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Rocketeer : ICharacter
{
    protected override void Awake()
    {
        base.Awake();
        CharacterType = CharacterType.Rocketeer;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Charge()
    {
        base.Charge();
    }

    protected override void Dodge()
    {
        base.Dodge();
    }

    protected override void UseSkill()
    {
        base.UseSkill();
        State = CharacterState.SkillActivated;
        Vector2 endPos = transform.position;
        endPos += prevMovedDirection * 2.5f;
        boxCollider.size *= 2f;
        animator.Play("flying");
        IsInvincible = true;
        transform.DOMove(endPos, 0.5f).OnComplete(() => { OnSkillEnd(); });
    }

    protected override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    protected override void OnSkillEnd()
    {
        IsInvincible = false;
        State = CharacterState.Idle;
        animator.Play("idle");
        boxCollider.size /= 2f;
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if(other.gameObject.CompareTag("Character") == true)
        {
            ICharacter otherCharacter = other.gameObject.GetComponent<ICharacter>();
            switch(State)
            {
                case CharacterState.SkillActivated:
                    OnSkillEnd();
                    otherCharacter.OnDamaged(1);
                    break;
                default:
                    break;
            }
        }
    }
}