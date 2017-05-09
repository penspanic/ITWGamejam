using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterFloatingUi : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image image;

    private void Awake()
    {
        //float parentScale = transform.parent.localScale.x;
        //float newScale = 1f / parentScale;
        //transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public void Initialize(Player player)
    {
        text.color = TeamController.GetTeamColor(player.TeamNumber);
        string teamTextStr = string.Empty;
        if(player.IsCpu == true)
        {
            teamTextStr = "CPU";
        }
        else
        {
            teamTextStr = player.PlayerNumber.ToString() + "P";
        }
        text.text = teamTextStr;

        image.color = TeamController.GetTeamColor(player.TeamNumber);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}