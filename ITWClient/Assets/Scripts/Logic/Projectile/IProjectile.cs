using UnityEngine;
using System.Collections;

public abstract class IProjectile : MonoBehaviour, IObject
{
    public int Hp { get; set; }
    public IObject owner { get; set; }

    public event System.Action<IObject> OnCreated;
    public event System.Action<IObject> OnDestroyed;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        OnCreated(this);
    }

    public void SetOwner(IObject owner)
    {
        this.owner = owner;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

    }

    // Do nothing
    public virtual void OnHit(IObject attacker, int damage, bool forced = false)
    {

    }
}