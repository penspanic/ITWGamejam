using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GuidePanel : MonoBehaviour
{
    public bool IsShowing { get; private set; }

    private Image guideImage;
    public void Show()
    {
        if(IsShowing == true)
        {
            return;
        }

        IsShowing = true;
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        if(IsShowing == false)
        {
            return;
        }

        IsShowing = false;
        this.gameObject.SetActive(false);
    }

    private Sprite[] guideSprites;
    private void Awake()
    {
        List<Sprite> guideSpriteList = new List<Sprite>();
        guideSpriteList.Add(Resources.Load<Sprite>("Sprites/UI/guide/Guide_Keyboard"));
        guideSpriteList.Add(Resources.Load<Sprite>("Sprites/UI/guide/Guide_Controller"));

        guideSprites = guideSpriteList.ToArray();
        guideImage = transform.Find("Guide Image").GetComponent<Image>();

        guideImage.sprite = guideSprites[0];
    }

    private void Update()
    {
        if(IsShowing == false)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape) == true || Input.GetButtonDown("Back") == true)
        {
            Hide();
        }
        else if(Input.anyKeyDown == true)
        {
            guideImage.sprite = guideImage.sprite == guideSprites[0] ? guideSprites[1] : guideSprites[0];
        }
    }
}