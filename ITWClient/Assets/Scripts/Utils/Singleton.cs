using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        Debug.Log(gameObject.name + " Created.");
        DontDestroyOnLoad(this.gameObject);
    }
}