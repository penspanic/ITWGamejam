using UnityEngine;
using System.Collections;

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
    Poisoned,
}

/// <summary>
/// 각 캐릭터 마다의 상세 구현(스킬 등)을 담당.
/// </summary>
public abstract class ICharacter : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int launchNeedMp;
    [SerializeField]
    private int skillNeedMp;
    public int Hp { get; protected set; }
    public int Mp { get; protected set; }
    public bool IsInvincible { get; protected set; }
    public CharacterState State { get; protected set; }
    public CharacterType CharacterType { get; protected set; }
    protected virtual void Awake()
    {

    }

    protected virtual void Update()
    {

    }

    public void Move(Vector2 normalizedDirection)
    {
        transform.Translate(normalizedDirection * moveSpeed * Time.deltaTime);
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
        UseSkill();
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
            default:
                break;
        }
        return Mp > skillNeedMp;
    }

    protected virtual void UseSkill()
    {
        Mp -= -skillNeedMp;
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
            default:
                break;
        }
        return Mp > launchNeedMp;
    }

    protected void Launch()
    {
        Mp -= launchNeedMp;
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
        return true;
    }

    protected virtual void Charge()
    {

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
            default:
                break;
        }
        return true;
    }

    protected virtual void Dodge()
    {
        IsInvincible = true;
    }
}