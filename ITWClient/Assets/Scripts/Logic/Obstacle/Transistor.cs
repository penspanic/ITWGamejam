using UnityEngine;
using System.Collections;

public class Transistor : IObstacle
{
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float attackIntaval;
    [SerializeField]
    private float attackDamage;

    public override void InitObstacle()
    {
        base.InitObstacle();

        
    }
}
