using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteamShot : IProjectile
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float duration;

    private List<IObject> hittedObjects = new List<IObject>();
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(DestroyProcess());
    }

    private IEnumerator DestroyProcess()
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
            otherObject.OnHit(this, damage);
        }
    }
}