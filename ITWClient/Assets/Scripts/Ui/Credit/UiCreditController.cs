using UnityEngine;
using System.Collections;

public class UiCreditController : MonoBehaviour
{

    private void Awake()
    {

    }

    private bool isSceneChanging = false;
    private void Update()
    {
        if(Input.anyKeyDown == true && isSceneChanging == false)
        {
            isSceneChanging = true;
            SceneUtil.LoadScene("MainMenu");
        }
    }
}