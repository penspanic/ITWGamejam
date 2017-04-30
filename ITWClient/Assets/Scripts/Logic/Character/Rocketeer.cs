using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Rocketeer : ICharacter
{
    private bool isSkillActivated = false;
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
        State = CharacterState.Flying;
        Vector2 endPos = transform.position;
        endPos += prevMovedDirection * 2.5f;
        boxCollider.size *= 2f;
        animator.Play("flying", 0);
        IsInvincible = true;
        isSkillActivated = true;
        transform.DOMove(endPos, 0.5f).OnComplete(() => { OnLaunchEnd(); });
    }

    protected override void OnLaunchEnd()
    {
        base.OnLaunchEnd();
        if(isSkillActivated == true)
        {
            OnSkillEnd();
        }
    }

    protected override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    protected override void OnSkillEnd()
    {
        isSkillActivated = false;
        boxCollider.size /= 2f;
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
    }
}