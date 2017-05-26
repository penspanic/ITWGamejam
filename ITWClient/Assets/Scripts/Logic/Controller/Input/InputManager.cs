using UnityEngine;
using System.Collections;

/// <summary>
/// 플레이어들(1P, 2P, 멀티 상황등)의 Input들을 PlayerInputController에 바인딩 시킴.
/// </summary>
public class InputManager : Singleton<InputManager>
{

    protected override void Awake()
    {
        foreach (string name in Input.GetJoystickNames())
        {
            Debug.Log(name);
        }
    }

    public void SetPlayers(PlayerInputController[] inputControllers)
    {
        foreach(var inputController in inputControllers)
        {
            int playerNum = inputController.PlayerNumber;
            inputController.BindAxis(PlayerInputType.MoveHorizontal, "Horizontal" + playerNum.ToString());
            inputController.BindAxis(PlayerInputType.MoveVertical, "Vertical" + playerNum.ToString());
            inputController.BindKey(PlayerInputType.Launch, "Launch" + playerNum.ToString());
            inputController.BindKey(PlayerInputType.Skill, "Skill" + playerNum.ToString());
            inputController.BindKey(PlayerInputType.Dodge, "Dodge" + playerNum.ToString());
            inputController.BindKey(PlayerInputType.Charge, "Charge" + playerNum.ToString());
        }
    }
}