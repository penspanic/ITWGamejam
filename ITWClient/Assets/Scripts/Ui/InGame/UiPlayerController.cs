using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UiPlayerController : MonoBehaviour
{
    [SerializeField]
    private HorizontalLayoutGroup statusBoxesGroup;
    private List<StatusBox> statusBoxes = new List<StatusBox>();

    private void Awake()
    {

    }

    public void SetPlayers(Player[] players)
    {
        GameObject statusBoxPrefab = Resources.Load<GameObject>("Prefabs/UI/InGame/Status Box");
        List<RectOffset> offsets = new List<RectOffset>();
        offsets.Add(new RectOffset(-380, -280, 0, 0)); // 2인
        offsets.Add(new RectOffset(-140, -35, 0, 0));  // 3인
        offsets.Add(new RectOffset(-60, 40, 0, 0));    // 4인
        foreach(Player player in players)
        {
            StatusBox newBox = Instantiate(statusBoxPrefab).GetComponent<StatusBox>();
            newBox.SetPlayer(player);
            newBox.transform.SetParent(statusBoxesGroup.transform);
            newBox.transform.localScale = Vector3.one;
            statusBoxes.Add(newBox);
        }
        statusBoxesGroup.padding = offsets[players.Length - 2];
    }
}