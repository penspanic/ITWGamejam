using UnityEngine;
using System.Collections;

public abstract class IObstacle : MonoBehaviour, ITile
{
    public IntVector2 TilePos { get; set; }

    private void Awake()
    {

    }
}