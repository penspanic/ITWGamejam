﻿using UnityEngine;
using System.Collections;

public abstract class IObstacle : MonoBehaviour, IObject
{
    [SerializeField]
    private int MaxHp;

    public int Hp { get; protected set; }
    private void Awake()
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

    }

    public virtual void OnHit(IObject attacker, int damage, bool forced = false)
    {
        Hp -= damage;
        if(Hp <= 0)
        {
            OnDestroyed();
        }
    }

    protected virtual void OnDestroyed()
    {

    }
}