using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : IProjectile
{
    [SerializeField]
    private float duration;
    [SerializeField]
    private int damage;
    private List<IObject> hittedObjects = new List<IObject>();

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(ExplosionProcess());
    }

    private IEnumerator ExplosionProcess()
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        IObject otherObject = other.GetComponent<IObject>();
        if(otherObject == null)
        {
            return;
        }
        if(otherObject != owner && hittedObjects.Contains(otherObject) == false)
        {
            otherObject.OnHit(this, 1);
            hittedObjects.Add(otherObject);
        }
    }
}