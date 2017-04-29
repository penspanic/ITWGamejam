using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// PlayerController에서 내린 명령을 수행하기만 함, 게임 로직적인 내용은 담지 말아야 함.
/// </summary>
public class Player : MonoBehaviour
{
    public int PlayerNumber { get; protected set; }
    private List<PlayerInputType> inputtedKeys = new List<PlayerInputType>();
    private List<PlayerInputType> prevInputtedKeys = new List<PlayerInputType>();
    public ICharacter TargetCharacter { get; protected set; }

    private void Awake()
    {

    }

    // 플레이 중에 동적으로 캐릭터를 바꿀 수 있도록? 도 할 수 있으면 좋을 것 같다.

    public void SetNumber(int num)
    {
        PlayerNumber = num;
    }

    public void SetCharacter(ICharacter character)
    {
        this.TargetCharacter = character;
    }

    private void Update()
    {

    }

    public void ClearKeys()
    {
        prevInputtedKeys.Clear();
        prevInputtedKeys.AddRange(inputtedKeys);
        inputtedKeys.Clear();
    }

    public void KeyDown(PlayerInputType inputType)
    {
        inputtedKeys.Add(inputType);

        // Special Attack
        if(prevInputtedKeys.Contains(PlayerInputType.Charge) == true
            && inputtedKeys.Contains(PlayerInputType.Charge) == true
            && inputtedKeys.Contains(PlayerInputType.Launch) == true)
        {
            TargetCharacter.DoUseSkill();
            return;
        }

        switch(inputType)
        {
            case PlayerInputType.Launch:
                TargetCharacter.DoLaunch();
                break;
            case PlayerInputType.Charge:
                TargetCharacter.DoCharge();
                break;
            case PlayerInputType.Dodge:
                TargetCharacter.DoDodge();
                break;
        }
    }

    public void KeyUp(PlayerInputType inputType)
    {
        switch(inputType)
        {
            case PlayerInputType.Charge:
                TargetCharacter.CancelCharge();
                break;
            default:
                break;
        }
    }

    public void KeyState(PlayerInputType inputType, bool pressed)
    {
        switch(inputType)
        {
            case PlayerInputType.Charge:
                if(pressed == false && TargetCharacter.IsCharging == true)
                {
                    TargetCharacter.CancelCharge();
                }
                break;
            default:
                break;
        }
    }


    public void Move(Vector2 direction)
    {
        TargetCharacter.CanMove(direction);
    }
}