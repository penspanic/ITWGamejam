using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UiPlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform statusBoxesParent;
    private List<StatusBox> statusBoxes = new List<StatusBox>();

    private void Awake()
    {

    }

    public void SetPlayers(Player[] players)
    {
        foreach(Player player in players)
        {

        }
    }
}