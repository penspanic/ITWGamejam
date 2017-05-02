using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StatusBox : MonoBehaviour
{
    public enum StatusType
    {
        Hp,
        Mp,
    }

    private Player targetPlayer;
    private Image portraitImage;
    private Image[] heartImages;
    private Image mpGaugeImage;

    private static Sprite filledHeartSprite;
    private static Sprite emptyHeartSprite;

    private static Sprite basicMpGaugeSprite;
    private static Sprite extremeMpGaugeSprite;

    private void Awake()
    {
        if(filledHeartSprite == null)
        {
            filledHeartSprite = Resources.Load<Sprite>("Sprites/UI/Status/heart");
            emptyHeartSprite = Resources.Load<Sprite>("Sprites/UI/Status/heart0");
            basicMpGaugeSprite = Resources.Load<Sprite>("Sprites/UI/Status/mpbar");
            extremeMpGaugeSprite = Resources.Load<Sprite>("Sprites/UI/Status/mpbar2");
        }

        portraitImage = transform.FindChild("Portrait").GetComponent<Image>();
        mpGaugeImage = transform.FindChild("Right").FindChild("Mp").FindChild("Mp Value").GetComponent<Image>();

        List<Image> heartList = new List<Image>();
        Transform heartsParent = transform.FindChild("Right").FindChild("Heart");
        for(int i = 0; i < heartsParent.childCount; ++i)
        {
            heartList.Add(heartsParent.GetChild(i).GetComponent<Image>());
        }
        heartImages = heartList.ToArray();
    }

    public void SetPlayer(Player player)
    {
        this.targetPlayer = player;
        SetPortrait();
        Refresh();
    }

    private void SetPortrait()
    {
        portraitImage.sprite = Resources.Load<Sprite>("Sprites/UI/Portrait/" + targetPlayer.TargetCharacter.CharacterType.ToString());
        int playerNumber = targetPlayer.PlayerNumber;
        int teamNumber = TeamController.GetTeam(playerNumber).TeamNumber;
        portraitImage.transform.FindChild("Team Color").GetComponent<Image>().color = TeamController.GetTeamColor(teamNumber);
    }

    private void Update()
    {
        if(targetPlayer != null && targetPlayer.TargetCharacter != null)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        SetHearts();
        SetMpGauge();
    }

    private void SetHearts()
    {
        foreach(Image eachHeartImage in heartImages)
        {
            eachHeartImage.sprite = emptyHeartSprite;
        }

        for(int i = 0; i < targetPlayer.TargetCharacter.Hp; ++i)
        {
            heartImages[i].sprite = filledHeartSprite;
        }
    }

    private void SetMpGauge()
    {
        mpGaugeImage.fillAmount = (float)targetPlayer.TargetCharacter.Mp / (float)targetPlayer.TargetCharacter.MaxMp;
    }
}