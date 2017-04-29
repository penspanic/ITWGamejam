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
    private ICharacter character;

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
        this.character = character;
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
            character.DoUseSkill();
            return;
        }

        switch(inputType)
        {
            case PlayerInputType.Launch:
                character.DoLaunch();
                break;
            case PlayerInputType.Charge:
                character.DoCharge();
                break;
            case PlayerInputType.Dodge:
                character.DoDodge();
                break;
        }
    }

    public void Move(Vector2 direction)
    {
        character.CanMove(direction);
    }
}