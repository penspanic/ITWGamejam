using UnityEngine;
using System.Collections;

public class Poison : IProjectile
{

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}