using UnityEngine;
using System.Collections.Generic;

public enum PlayerInputType
{
    MoveHorizontal,         // Axis
    MoveVertical,           // Axis
    Launch,                 // Key press
    Charge,                 // Key press
    Dodge,                  // Key press
}

public class PlayerInputController : MonoBehaviour
{
    public int PlayerNumber { get { return playerController.TargetPlayer.PlayerNumber; } }
    public bool Initialized { get; set; }

    private Dictionary<PlayerInputType, string> bindedKeys = new Dictionary<PlayerInputType, string>();
    private Dictionary<PlayerInputType, string> bindedAxes = new Dictionary<PlayerInputType, string>(); //axis의 복수형이 axes라고 함...

    private IPlayerController playerController;
    private void Awake()
    {
        Initialized = false;
        playerController = GetComponent<IPlayerController>();
        
        foreach(string name in Input.GetJoystickNames())
        {
            Debug.Log(name);
        }
    }
    
    public void BindKey(PlayerInputType type, string keyName)
    {
        bindedKeys.Add(type, keyName);
    }

    public void BindAxis(PlayerInputType type, string axisName)
    {
        bindedAxes.Add(type, axisName);
    }

    private void Update()
    {
        if(Initialized == false)
            return;

        playerController.TargetPlayer.ClearKeys();
        foreach(var keyPair in bindedKeys)
        {
            if(Input.GetButtonDown(keyPair.Value) == true)
            {
                playerController.ProcessKeyDown(keyPair.Key);
            }
            if(Input.GetButtonUp(keyPair.Value) == true)
            {
                playerController.ProcessKeyUp(keyPair.Key);
            }
            playerController.ProcessKeyState(keyPair.Key, Input.GetButton(keyPair.Value));
        }

        float horizontal = Input.GetAxis(bindedAxes[PlayerInputType.MoveHorizontal]);
        float vertical = Input.GetAxis(bindedAxes[PlayerInputType.MoveVertical]);
        Vector2 direction = new Vector2(horizontal, -vertical);
        if(direction.magnitude < 0.3f)
        {
            playerController.ProcessMove(Vector2.zero);
            return;
        }
        Debug.Log("Horizontal : " + horizontal + " Vertical : " + vertical);
        Debug.Log("Direction Length : " + direction.magnitude);
        playerController.ProcessMove(direction.normalized);
    }

    // 네트워크 상에서 상대 플레이어의 Input을 처리할 때?
    public void ProcesKey(PlayerInputType type)
    {

    }

    public void ProcessAxis(PlayerInputType type)
    {

    }
}