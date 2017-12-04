using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum AxisDown
{
    LEFT = 0,
    RIGHT,
    UP,
    DOWN,
    END_OF_AXIS_DOWN
}

public class ControllerAxisHelper : Singleton<ControllerAxisHelper>
{

    private const float AXIS_DOWN_START_VALUE = 0.2f;
    private const float AXIS_DOWN_END_VALUE = 0.7f;

    public bool Initialized { get; private set; } = false;
    private Dictionary<int/*ControllerNumber*/, Dictionary<AxisDown, float>> axisDatas;
    private Dictionary<int/*ControllerNumber*/, Dictionary<AxisDown, bool>> downedDatas;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);

        foreach (string name in Input.GetJoystickNames())
        {
            Debug.Log(name);
        }
    }

    public void Initialize(int controllerCount)
    {
        if(Initialized == true)
            return;

        Initialized = true;

        axisDatas = new Dictionary<int, Dictionary<AxisDown, float>>();
        downedDatas = new Dictionary<int, Dictionary<AxisDown, bool>>();
        for(int num = 1; num <= controllerCount; ++num)
        {
            axisDatas.Add(num, new Dictionary<AxisDown, float>());
            for(int i = (int)AxisDown.LEFT; i < (int)AxisDown.END_OF_AXIS_DOWN; ++i)
            {
                axisDatas[num].Add((AxisDown)i, 0f);
            }

            downedDatas.Add(num, new Dictionary<AxisDown, bool>());
            for (int i = (int)AxisDown.LEFT; i < (int)AxisDown.END_OF_AXIS_DOWN; ++i)
            {
                downedDatas[num].Add((AxisDown)i, false);
            }
        }
    }

    private void Update()
    {
        if (Initialized == false)
            return;

        for(int num = 1; num <= axisDatas.Count; ++num)
        {
            float horizontal = Input.GetAxis("Horizontal" + num);
            float vertical = Input.GetAxis("Vertical" + num);

            Dictionary<AxisDown, float> prevData = new Dictionary<AxisDown, float>(axisDatas[num]);
            axisDatas[num][AxisDown.LEFT] = -horizontal;
            axisDatas[num][AxisDown.RIGHT] = horizontal;
            axisDatas[num][AxisDown.UP] = vertical;
            axisDatas[num][AxisDown.DOWN] = -vertical;

            foreach(var data in axisDatas[num])
            {
                if(prevData[data.Key] <= AXIS_DOWN_END_VALUE && downedDatas[num][data.Key] == false && data.Value > AXIS_DOWN_END_VALUE)
                {
                    downedDatas[num][data.Key] = true;
                }
                else
                {
                    downedDatas[num][data.Key] = false;
                }

            }
        }
    }

    public bool IsAxisDown(int controllerNumber, AxisDown axis)
    {
        if(Initialized == false)
        {
            return false;
        }

        return downedDatas[controllerNumber][axis];
    }

    public bool IsAxisDownAll(AxisDown axis)
    {
        if(Initialized == false)
        {
            return false;
        }


        foreach(var downed in downedDatas)
        {
            foreach(var down in downed.Value)
            {
                if(down.Key == axis && down.Value == true)
                {
                    return true;
                }
            }
        }

        return false;
    }
}