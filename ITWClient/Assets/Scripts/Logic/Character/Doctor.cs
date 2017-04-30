using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Doctor : ICharacter
{
    [SerializeField]
    private GameObject poisonPrefab;
    [SerializeField]
    private float poisonCreateIntervalSecond;
    [SerializeField]
    private float skillMoveSpeed;

    private float originalMoveSpeed;

    private Coroutine skillCoroutine = null;
    protected override void Awake()
    {
        base.Awake();
        CharacterType = CharacterType.Doctor;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void ProcessKeystate(List<PlayerInputType> pressedKeys)
    {
        base.ProcessKeystate(pressedKeys);
        if(pressedKeys.Contains(PlayerInputType.Skill) == false && skillCoroutine != null)
        {
            CancelSkill();
        }
    }

    protected override void UseSkill()
    {
        if(skillCoroutine != null)
        {
            CancelSkill();
            return;
        }
        skillCoroutine = StartCoroutine(SkillProcess());
    }

    private IEnumerator SkillProcess()
    {
        State = CharacterState.SkillActivated;
        originalMoveSpeed = moveSpeed;
        moveSpeed = skillMoveSpeed;
        float elapsedTime = 0f;
        while(true)
        {
            yield return null;

            elapsedTime += Time.deltaTime;
            if(elapsedTime > poisonCreateIntervalSecond)
            {
                elapsedTime -= poisonCreateIntervalSecond;
                GameObject newPoison = Instantiate(poisonPrefab);
                newPoison.transform.position = this.transform.position;
                Mp -= (int)(skillNeedMp * poisonCreateIntervalSecond);
                if(Mp <= 0)
                {
                    Mp = 0;
                    CancelSkill();
                }
            }
        }
    }

    protected override bool CanUseSkill()
    {
        if(skillCoroutine == null)
        {
            return base.CanUseSkill();
        }
        else
        {
            return true;
        }
    }

    private void CancelSkill()
    {
        State = CharacterState.Idle;
        if(skillCoroutine != null)
        {
            StopCoroutine(skillCoroutine);
            skillCoroutine = null;
        }
        moveSpeed = originalMoveSpeed;
    }
}