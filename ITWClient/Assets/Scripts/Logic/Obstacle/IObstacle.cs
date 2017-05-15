using UnityEngine;
using System.Collections;

public abstract class IObstacle : MonoBehaviour, IObject
{
    [SerializeField]
    private int MaxHp;

    public int Hp { get; protected set; }

    public event System.Action<IObject> OnCreated;
    public event System.Action<IObject> OnDestroyed;

    private void Awake()
    {
    }

    private void Start()
    {
        if (OnCreated != null)
            OnCreated(this);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

    }

    public virtual void OnHit(IObject attacker, int damage, bool forced = false)
    {
        Hp -= damage;
        if(Hp <= 0)
        {
            OnDestroyed(this);
            Destroy(this.gameObject);
        }
    }
}