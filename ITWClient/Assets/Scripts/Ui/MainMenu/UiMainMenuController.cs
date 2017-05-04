using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UiMainMenuController : MonoBehaviour
{
    [SerializeField]
    private Transform titleTrs;
    [SerializeField]
    private MenuButton[] buttons;

    private bool isOpening;
    private bool isTitlePressed;
    private bool isAnimating;
    private bool canButtonPress;
    private int currIdx;

    private void Awake()
    {
        isOpening = true;
        isAnimating = false;
        canButtonPress = false;
        currIdx = 0;

        for (int i = 0; i < buttons.Length; ++i)
        {
            buttons[i].InitMenuButton();
        }
        buttons[2].OnSelected(true);
        currIdx = 2;
        StartTitleAnimation();
    }

    private void StartTitleAnimation() {
        titleTrs.position = new Vector3(-12.4f, 0f);
        isAnimating = true;
        titleTrs.DOMoveX(0, 1.5f).SetEase(Ease.OutBounce, 0f).OnComplete(() =>
            {
                isAnimating = false;
                isOpening = false;
            });
    }



    void Update()
    {
        if (isAnimating == true || isOpening == true)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnPressedTitle();
        }

        // pad Control??


        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currIdx <= 0)
            {
                return;
            }
            buttons[currIdx].OnSelected(false);
            --currIdx;
            buttons[currIdx].OnSelected(true);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currIdx >= buttons.Length - 1)
            {
                return;
            }
            buttons[currIdx].OnSelected(false);
            ++currIdx;
            buttons[currIdx].OnSelected(true);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(currIdx);
            switch (currIdx)
            {
                case 0: // option
                    OnPressedOptionButton();
                    break;
                case 1: // guide
                    OnPressedGuideButton();
                    break;
                case 2: // start
                    OnPressedStartButton();
                    break;
                case 3: // credit
                    OnPressedCreditButton();
                    break;
                case 4: // exit
                    OnPressedQuit();
                    break;
            }
        }
    }

    public void OnPressedTitle()
    {
        if (isTitlePressed == true)
        {
            return;
        }
        isTitlePressed = true;
        isAnimating = true;
        titleTrs.DOMoveY(-7.2f, 0.7f).SetEase(Ease.InBack).OnComplete(() =>
        {
            isAnimating = false;
            canButtonPress = true;
        });
    }

    public void OnPressedStartButton() 
    {
        Debug.Log(canButtonPress);
        if (canButtonPress == false)
        {
            OnPressedTitle();
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

    public void OnPressedQuit() 
    {
        Application.Quit();
    }
}