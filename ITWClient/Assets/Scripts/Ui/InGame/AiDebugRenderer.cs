using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AiDebugRenderer : MonoBehaviour
{
    public Text[] texts;

    public static AiDebugRenderer Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AiDebugRenderer>();
            }
            return _instance;
        }
    }
    private static AiDebugRenderer _instance;
    private void Awake()
    {

    }

    public void UpdateString(string str, int playerNumber)
    {
        str = "Player " + playerNumber.ToString() + " : " + str;
        texts[playerNumber - 1].text = str;
    }
}