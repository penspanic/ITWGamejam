using UnityEngine;
using System.Collections.Generic;

public enum PlayerInputType
{
    Move,          // Axis
    DefaultAttack, // Key press
    SpecialAttack, // Key press
    Charge,        // Key press
}

public class PlayerInputController : MonoBehaviour
{

    Dictionary<PlayerInputType, string> bindedKeys;
    Dictionary<PlayerInputType, string> bindedAxes; //axis의 복수형이 axes라고 함...
    private void Awake()
    {

    }

    public void BindKey(PlayerInputType type, string keyName)
    {

    }

    public void BindAxis(PlayerInputType type, string axisName)
    {

    }

    private void Update()
    {

    }

    // 네트워크 상에서 상대 플레이어의 Input을 처리할 때?
    public void ProcesKey(PlayerInputType type)
    {

    }

    public void ProcessAxis(PlayerInputType type)
    {

    }
}