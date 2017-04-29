using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UiMainMenuController : MonoBehaviour
{
    [SerializeField]
    private Transform titleTrs;

    private bool isTitlePressed;
    private bool canButtonPress;

    private void Awake()
    {
        isTitlePressed = false;
        canButtonPress = false;
    }


    void Update()
    {
        if (isTitlePressed == true)
        {
            return;
        }

        // pad Control??
        if (Input.GetMouseButtonUp(0))
        {
            isTitlePressed = true;
            titleTrs.DOLocalMoveY(400f, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    canButtonPress = true;
                });
        }
    }

    public void OnPressedTitle()
    {
        isTitlePressed = true;
    }

    public void OnPressedStartButton() 
    {
        if (canButtonPress == false)
        {
            return;
        }

        SceneManager.LoadScene("GameReady");
    }

    public void OnPressedCreditButton() 
    {
        if (canButtonPress == false)
        {
            return;
        }
    }

    public void OnPressedGuideButton()
    {
        if (canButtonPress == false)
        {
            return;
        }
    }

    public void OnPressedOptionButton()
    {
        if (canButtonPress == false)
        {
            return;
        }
    }
}