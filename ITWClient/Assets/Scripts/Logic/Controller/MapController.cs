using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    public int tileSizeX;
    public int tileSizeY;

    private IObstacle[] obstaclePrefabs;
    private List<IObstacle> obstacles = new List<IObstacle>();
    //private ITile[,] tiles;
    //private void Awake()
    //{
    //    obstaclePrefabs = Resources.LoadAll<IObstacle>("Prefabs/Obstacle");
    //    tiles = new ITile[tileSizeX, tileSizeY];
    //}
    //#region ITile
    //public bool IsEmpty(IntVector2 pos)
    //{
    //    return tiles[pos.x, pos.y] == null;
    //}
    
    //public ITile GetTile(IntVector2 pos)
    //{
    //    return tiles[pos.x, pos.y];
    //}
    
    //public void RemoveTile(IntVector2 pos)
    //{
    //    tiles[pos.x, pos.y] = null;
    //}

    //public IntVector2 GetEmptyTilePos()
    //{
    //    // 무한루프 돌 상황까진 없을 것 같다.
    //    int x, y = 0;
    //    while(true)
    //    {
    //        x = Random.Range(0, tileSizeX);
    //        y = Random.Range(0, tileSizeY);
    //        if(IsEmpty(new IntVector2(x, y)) == false)
    //            return new IntVector2(x, y);
    //    }
    //}


    //public void SetTile(ITile tile)
    //{
    //    tiles[tile.TilePos.x, tile.TilePos.y] = tile;
    //}
    //#endregion

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
            //IntVector2 coordinate = GetEmptyTilePos();
            //tiles[coordinate.x, coordinate.y] = newObstacle;
        }
    }
}