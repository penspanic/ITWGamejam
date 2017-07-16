using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum MapType
{
    FourGimmick,
    BigGimmick,
    PipeLine,
}

public class MapController : Singleton<MapController>
{
    [SerializeField]
    private Transform background;

    private Dictionary<MapType, IMap> mapDic;
    private GameObject[] obstaclePrefabs = null;
    private List<IObstacle> obstacles = new List<IObstacle>();

    private IMap currMap = null;

    protected override void Awake()
    {
        obstaclePrefabs = Resources.LoadAll<GameObject>("Prefabs/Obstacle");

        mapDic = new Dictionary<MapType, IMap>();
        mapDic.Add(MapType.FourGimmick, Resources.Load<IMap>("Prefabs/Map/FourGimmick"));
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

    public void CreateMap(MapType mapType)
    {
        currMap = Instantiate<IMap>(mapDic[mapType], background);

        if (currMap == null)
            throw new UnityException("Map Load Fail!!, MapType: " + mapType.ToString());

        currMap.InitMap();
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