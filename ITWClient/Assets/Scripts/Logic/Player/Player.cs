using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// PlayerController에서 내린 명령을 수행하기만 함, 게임 로직적인 내용은 담지 말아야 함.
/// </summary>
public class Player : MonoBehaviour
{
    public bool IsCpu { get; set; }
    public int TeamNumber { get; private set; }
    public int PlayerNumber { get; private set; }
    public ICharacter TargetCharacter { get; private set; }

    private void Awake()
    {

    }

    // 플레이 중에 동적으로 캐릭터를 바꿀 수 있도록? 도 할 수 있으면 좋을 것 같다.

    public void SetNumber(int teamNum, int playerNum)
    {
        this.TeamNumber = teamNum;
        this.PlayerNumber = playerNum;
    }

    public void SetCharacter(ICharacter character)
    {
        this.TargetCharacter = character;
    }

    private void Update()
    {

    }

    public void KeyDown(PlayerInputType inputType)
    {
        if(TargetCharacter.IsDead == true)
        {
            return;
        }

        switch(inputType)
        {
            case PlayerInputType.Launch:
                TargetCharacter.DoLaunch();
                break;
            case PlayerInputType.Dodge:
                TargetCharacter.DoDodge();
                break;
            case PlayerInputType.Skill:
                TargetCharacter.DoUseSkill();
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

    public void KeyState(List<PlayerInputType> keys)
    {
        if(TargetCharacter.IsDead == false)
        {
            TargetCharacter.ProcessKeystate(keys);
        }
    }

    public void Move(Vector2 direction)
    {
        if(TargetCharacter.IsDead == false)
        {
            TargetCharacter.DoMove(direction);
        }
    }
}