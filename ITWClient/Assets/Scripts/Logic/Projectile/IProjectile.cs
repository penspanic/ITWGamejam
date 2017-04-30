using UnityEngine;
using System.Collections;

public abstract class IProjectile : MonoBehaviour
{
    public ICharacter owner { get; set; }

    protected virtual void Awake()
    {

    }

    public void SetOwner(ICharacter owner)
    {
        this.owner = owner;
    }

    protected virtual void Update()
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(owner.gameObject == owner.gameObject)
        {
            return;
        }
        switch(other.tag)
        {
            case "Character":
                break;
            default:
                break;
        }
    }
}