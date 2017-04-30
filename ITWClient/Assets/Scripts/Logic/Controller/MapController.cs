using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    public int tileSizeX;
    public int tileSizeY;

    private IObstacle[] obstaclePrefabs;
    private List<IObstacle> obstacles = new List<IObstacle>();

    public Vector2 GetRandomMapPos()
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
            
            IObstacle obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            IObstacle newObstacle = Instantiate<IObstacle>(obstaclePrefab);
            obstacles.Add(newObstacle);
        }
    }
}