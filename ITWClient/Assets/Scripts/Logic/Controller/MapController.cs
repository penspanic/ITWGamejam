using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapController : Singleton<MapController>
{
    private GameObject[] obstaclePrefabs = null;
    private List<IObstacle> obstacles = new List<IObstacle>();

    protected override void Awake()
    {
        obstaclePrefabs = Resources.LoadAll<GameObject>("Prefabs/Obstacle");
    }

    public static Vector2 GetRandomMapPos()
    {
        const float widthRadius = 3.78f;
        const float heightRadius = 2.9f;

        float fK, fS;

        fK = Random.Range(0f, 1f) *360f * Mathf.PI / 180f; // 0~360 사이 임의의 각에 대한 라디안 값
        fS = Random.Range(0f, 1f); // 스케일

        float fX = widthRadius * Mathf.Cos(fK) * fS;
        float fY = heightRadius * Mathf.Sin(fK) * fS;

        return new Vector2(fX, fY);
    }
    public void CreateObstacles(int count)
    {
        for(int i = 0; i < count; ++i)
        {
            
            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            IObstacle newObstacle = Instantiate(obstaclePrefab).GetComponent<IObstacle>();
            obstacles.Add(newObstacle);

            SortingLayerController.Instance.AddTarget(newObstacle);
        }
    }
}