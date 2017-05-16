using UnityEngine;
using System.Collections.Generic;

public enum PlayerInputType
{
    MoveHorizontal,         // Axis
    MoveVertical,           // Axis
    Launch,                 // Key press
    Dodge,                  // Key press
    Skill,                  // Key press
    Charge,                 // Trigger on / off
}

public class PlayerInputController : MonoBehaviour
{
    public PlayerController PlayerController { get; set; }
    public int PlayerNumber { get { return PlayerController.TargetPlayer.PlayerNumber; } }
    public bool Initialized { get; set; }

    private Dictionary<PlayerInputType, string> bindedKeys = new Dictionary<PlayerInputType, string>();
    private Dictionary<PlayerInputType, string> bindedAxes = new Dictionary<PlayerInputType, string>(); //axis의 복수형이 axes라고 함...

    private void Awake()
    {
        Initialized = false;
        StageController.Instance.OnStageStart += OnStageStart;
    }
    
    public void BindKey(PlayerInputType type, string keyName)
    {
        bindedKeys.Add(type, keyName);
    }

    public void BindAxis(PlayerInputType type, string axisName)
    {
        bindedAxes.Add(type, axisName);
    }

    private void OnStageStart()
    {

    }

    private void FixedUpdate()
    {
        if(Initialized == false || StageController.Instance.IsStageStarted == false)
            return;

        List<PlayerInputType> pressedKeys = new List<PlayerInputType>();
        if(bindedKeys.ContainsKey(PlayerInputType.Charge) == true)
        {
            if(Input.GetAxis(bindedKeys[PlayerInputType.Charge])> 0.3f)
            {
                pressedKeys.Add(PlayerInputType.Charge);
            }
        }
        foreach(var keyPair in bindedKeys)
        {
            if(Input.GetButton(keyPair.Value) == true)
            {
                pressedKeys.Add(keyPair.Key);
            }
        }
        PlayerController.ProcessKeyState(pressedKeys);

        foreach(var keyPair in bindedKeys)
        {
            if(Input.GetButtonDown(keyPair.Value) == true)
            {
                PlayerController.ProcessKeyDown(keyPair.Key);
            }
            if(Input.GetButtonUp(keyPair.Value) == true)
            {
                PlayerController.ProcessKeyUp(keyPair.Key);
            }
        }

        float horizontal = Input.GetAxis(bindedAxes[PlayerInputType.MoveHorizontal]);
        float vertical = Input.GetAxis(bindedAxes[PlayerInputType.MoveVertical]);
        Vector2 direction = new Vector2(horizontal, -vertical);
        if(direction.magnitude < 0.3f)
        {
            PlayerController.ProcessMove(Vector2.zero);
            return;
        }
        PlayerController.ProcessMove(direction.normalized);
    }

    // 네트워크 상에서 상대 플레이어의 Input을 처리할 때?
    public void ProcesKey(PlayerInputType type)
    {

    }

    public void ProcessAxis(PlayerInputType type)
    {

    }
}