using UnityEngine;
using System.Collections;

public class StageController : MonoBehaviour
{
    public bool IsStageStarted { get; set; }

    private ItemController itemController;
    private MapController mapController;
    private void Awake()
    {
        itemController = GameObject.FindObjectOfType<ItemController>();
        mapController = GameObject.FindObjectOfType<MapController>();

        int obstacleCount = Random.Range(5, 10);
        mapController.CreateObstacles(obstacleCount);

        StartCoroutine(StageStartProcess());
    }

    private IEnumerator StageStartProcess()
    {
        yield return new WaitForSeconds(5f);
    }

    private void Update()
    {

    }
}