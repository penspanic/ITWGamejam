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
    private GameObject[] heartObjects;
    private Image mpGaugeImage;
    private void Awake()
    {
        portraitImage = transform.FindChild("Portrait").GetComponent<Image>();
        mpGaugeImage = transform.FindChild("Right").FindChild("Mp").FindChild("Mp Value").GetComponent<Image>();

        List<GameObject> heartList = new List<GameObject>();
        Transform heartsParent = transform.FindChild("Right").FindChild("Heart");
        for(int i = 0; i < heartsParent.childCount; ++i)
        {
            heartList.Add(heartsParent.GetChild(i).gameObject);
        }
        heartObjects = heartList.ToArray();
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
        portraitImage.transform.FindChild("Team Color").GetComponent<Image>().color = TeamController.GetTeamColor(playerNumber);
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
        SetMpGuage();
    }

    private void SetHearts()
    {
        foreach(GameObject heartObject in heartObjects)
        {
            heartObject.SetActive(false);
        }

        for(int i = 0; i < targetPlayer.TargetCharacter.Hp; ++i)
        {
            heartObjects[i].SetActive(true);
        }
    }

    private void SetMpGuage()
    {
        mpGaugeImage.fillAmount = (float)targetPlayer.TargetCharacter.Mp / (float)targetPlayer.TargetCharacter.MaxMp;
    }
}