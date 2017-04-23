using UnityEngine;
using System.Collections;

public enum CharacterActionType
{
    DefaultAttack,
    SpecialAttack,
    Charge,
}
/// <summary>
/// 각 캐릭터 마다의 상세 구현(스킬 등)을 담당.
/// </summary>
public abstract class ICharacter : MonoBehaviour
{
    public int Hp { get; protected set; }
    public int Mp { get; protected set; }
    protected virtual void Awake()
    {

    }

    protected virtual void Update()
    {

    }

    public void DoAction()
    {

    }
}