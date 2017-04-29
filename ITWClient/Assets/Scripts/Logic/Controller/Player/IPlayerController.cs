using UnityEngine;
using System.Collections;

/// <summary>
/// 실제 플레이어, AI 플레이어의 동작을 위해 추상 클래스로 분리.
/// </summary>
public abstract class IPlayerController : MonoBehaviour
{
    public Player TargetPlayer { get; protected set; }
    protected virtual void Awake()
    {

    }

    protected virtual void Update()
    {

    }

    public void ProcessKey(PlayerInputType inputType)
    {
        TargetPlayer.KeyDown(inputType);
    }

    public void ProcessMove(Vector2 normalizedDirection)
    {
        TargetPlayer.Move(normalizedDirection);
    }
}