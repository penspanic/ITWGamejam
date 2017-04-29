using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    public int tileSizeX;
    public int tileSizeY;

    private IObstacle[] obstaclePrefabs;
    private List<IObstacle> obstacles = new List<IObstacle>();
    private ITile[,] tiles;
    private void Awake()
    {
        obstaclePrefabs = Resources.LoadAll<IObstacle>("Prefabs/Obstacle");
        tiles = new ITile[tileSizeX, tileSizeY];
    }
    #region ITile
    public bool IsEmpty(int x, int y)
    {
        return tiles[x, y] == null;
    }
    
    public ITile GetTile(int x, int y)
    {
        return tiles[x, y];
    }
    
    public IntVector2 GetEmptyTileIndex()
    {
        // 무한루프 돌 상황까진 없을 것 같다.
        int x, y = 0;
        while(true)
        {
            x = Random.Range(0, tileSizeX);
            y = Random.Range(0, tileSizeY);
            if(IsEmpty(x, y) == false)
                return new IntVector2(x, y);
        }
    }

    public void SetTile(ITile tile, int x, int y)
    {
        tiles[x, y] = tile;
    }
    #endregion

    public void CreateObstacles(int count)
    {
        for(int i = 0; i < count; ++i)
        {
            
            IObstacle obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            IObstacle newObstacle = Instantiate<IObstacle>(obstaclePrefab);
            obstacles.Add(newObstacle);
            IntVector2 coordinate = GetEmptyTileIndex();
            tiles[coordinate.x, coordinate.y] = newObstacle;
        }
    }
}