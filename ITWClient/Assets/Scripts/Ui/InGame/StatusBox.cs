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
    private List<Image> heartImages = new List<Image>();
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

        portraitImage = transform.Find("Portrait").GetComponent<Image>();
        mpGaugeImage = transform.Find("Right").Find("Mp").Find("Mp Value").GetComponent<Image>();

        Transform heartsParent = transform.Find("Right").Find("Heart");
        for(int i = 0; i < heartsParent.childCount; ++i)
        {
            heartImages.Add(heartsParent.GetChild(i).GetComponent<Image>());
            heartImages[i].enabled = false;
        }
    }

    public void SetPlayer(Player player)
    {
        if(this.targetPlayer != null)
        {
            this.targetPlayer.TargetCharacter.OnDestroyed -= OnCharacterDeath;
        }

        targetPlayer = player;
        targetPlayer.TargetCharacter.OnDestroyed += OnCharacterDeath;
        targetPlayer.TargetCharacter.OnDamaged += OnCharacterDamaged;

        SetPortrait();
        heartImages.RemoveRange(targetPlayer.TargetCharacter.MaxHp, heartImages.Count - targetPlayer.TargetCharacter.MaxHp);
    }

    private void SetPortrait()
    {
        portraitImage.sprite = Resources.Load<Sprite>("Sprites/UI/Portrait/" + targetPlayer.TargetCharacter.CharacterType.ToString());
        int playerNumber = targetPlayer.PlayerNumber;
        int teamNumber = TeamController.GetTeamByPlayerNumber(playerNumber).TeamNumber;
        portraitImage.transform.Find("Team Color").GetComponent<Image>().color = TeamController.GetTeamColor(teamNumber);
        Text teamText = portraitImage.transform.Find("Team Text").GetComponent<Text>();
        teamText.color = TeamController.GetTeamColor(teamNumber);
        string teamTextStr = string.Empty;
        if(targetPlayer.IsCpu == true)
        {
            teamTextStr = "CPU";
        }
        else
        {
            teamTextStr = playerNumber.ToString() + "P";
        }
        teamText.text = teamTextStr;
    }

    private void OnCharacterDeath(IObject character)
    {
        portraitImage.color = new Color(0.25f, 0.25f, 0.25f);
        GameObject effect = EffectController.Instance.ShowEffect(EffectType.Die_Portrait, new Vector2(-48f, 0f), this.transform);
        effect.transform.localScale = new Vector3(100f, 100f, 100f);
    }

    private void Update()
    {
        if(targetPlayer != null && targetPlayer.TargetCharacter != null)
        {
            Refresh();
        }
    }


    private void OnCharacterDamaged(int prevHp, int currHp)
    {
    }

    public void Refresh()
    {
        SetHearts();
        SetMpGauge();
    }

    private void SetHearts()
    {
        for(int i = 0; i < targetPlayer.TargetCharacter.MaxHp; ++i)
        {
            heartImages[i].enabled = true;
            heartImages[i].sprite = emptyHeartSprite;
        }

        for(int i = 0; i < targetPlayer.TargetCharacter.Hp; ++i)
        {
            heartImages[i].sprite = filledHeartSprite;
        }
    }

    private void SetMpGauge()
    {
        mpGaugeImage.sprite = targetPlayer.TargetCharacter.IsExtremeMp ? extremeMpGaugeSprite : basicMpGaugeSprite;
        mpGaugeImage.fillAmount = (float)targetPlayer.TargetCharacter.Mp / (float)targetPlayer.TargetCharacter.MaxMp;
    }
}