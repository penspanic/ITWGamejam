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
                if(GameObject.FindObjectOfType<T>() != null)
                {
                    _instance = GameObject.FindObjectOfType<T>();
                    return _instance;
                }
                _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        Debug.Log(gameObject.name + " Created.");
    }
}