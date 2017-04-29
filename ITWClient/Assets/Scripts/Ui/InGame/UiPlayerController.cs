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
        GameObject statusBoxPrefab = Resources.Load<GameObject>("Prefabs/UI/InGame/Status Box");
        foreach(Player player in players)
        {
            StatusBox newBox = Instantiate(statusBoxPrefab).GetComponent<StatusBox>();
            newBox.SetCharacter(player.TargetCharacter);
            newBox.transform.SetParent(statusBoxesParent);
            newBox.transform.localScale = Vector3.one;
            statusBoxes.Add(newBox);
        }
    }
}