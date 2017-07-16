using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneUtil : MonoBehaviour
{

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        SfxManager.Instance.StopAll();
    }
}