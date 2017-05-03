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
    [SerializeField]
    private Vector2 poisonSizeRange;
    [SerializeField]
    private Vector2 poisonPosRange;
    [SerializeField]
    private GameObject dodgeEffect;

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

    protected override bool CanMove()
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

    protected override void Dodge()
    {
        base.Dodge();
        dodgeEffect.SetActive(true);
        dodgeEffect.GetComponent<Animator>().Play("DoctorEvade", -1, 0f);
    }

    protected override void OnDodgeEnd()
    {
        base.OnDodgeEnd();
        dodgeEffect.SetActive(false);
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
                Poison newPoison = Instantiate(poisonPrefab).GetComponent<Poison>();
                newPoison.SetOwner(this);
                newPoison.gameObject.layer = this.gameObject.layer;
                float scale = Random.Range(poisonSizeRange.x, poisonSizeRange.y);
                newPoison.transform.localScale = new Vector3(scale, scale, scale);

                Vector3 createPos = this.transform.position;
                createPos.x += Random.Range(poisonPosRange.x, poisonPosRange.y);
                createPos.y += Random.Range(poisonPosRange.x, poisonPosRange.y);
                newPoison.transform.position = createPos;

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