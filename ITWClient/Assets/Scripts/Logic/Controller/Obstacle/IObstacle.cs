using UnityEngine;
using System.Collections;

public abstract class IObstacle : MonoBehaviour, ITile
{
    public int TileX { get; set; }
    public int TileY { get; set; }

    private void Awake()
    {

    }
}