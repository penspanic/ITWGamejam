using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 실제 플레이어, AI 플레이어의 동작을 위해 추상 클래스로 분리.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public Player TargetPlayer { get; set; }
    protected virtual void Awake()
    {
    }

    protected virtual void Update()
    {

    }

    public void ProcessKeyDown(PlayerInputType inputType)
    {
        TargetPlayer.KeyDown(inputType);
    }

    public void ProcessKeyUp(PlayerInputType inputType)
    {
        TargetPlayer.KeyUp(inputType);
    }

    public void ProcessKeyState(List<PlayerInputType> pressedKeys)
    {
        TargetPlayer.KeyState(pressedKeys);
    }

    public void ProcessMove(Vector2 normalizedDirection)
    {
        TargetPlayer.Move(normalizedDirection);
    }
}