using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class StatusBox : MonoBehaviour
{
    public enum StatusType
    {
        Hp,
        Mp,
    }

    private ICharacter targetCharacter;
    private Image portraitImage;
    private GameObject[] heartObjects;
    private Image mpGaugeImage;
    private void Awake()
    {
        portraitImage = transform.FindChild("Portrait").GetComponent<Image>();
        mpGaugeImage = transform.FindChild("Mp").FindChild("Mp Value").GetComponent<Image>();
        heartObjects = transform.FindChild("Heart").GetComponentsInChildren<GameObject>();
    }

    public void SetCharacter(ICharacter character)
    {
        this.targetCharacter = character;
        SetPortrait();
        Refresh();
    }

    private void SetPortrait()
    {
        //portraitImage.sprite = Resources.Load<Sprite>("Sprites/UI/Portrait/" + targetCharacter.CharacterType.ToString());
    }

    public void Refresh()
    {
        SetHearts();
        SetMpGuage();
    }

    private void SetHearts()
    {
        foreach(GameObject heartObject in heartObjects)
        {
            heartObject.SetActive(false);
        }

        for(int i = 0; i < targetCharacter.Hp; ++i)
        {
            heartObjects[i].SetActive(true);
        }
    }

    private void SetMpGuage()
    {
        mpGaugeImage.fillAmount = (float)targetCharacter.Mp / (float)targetCharacter.MaxMp;
    }
}