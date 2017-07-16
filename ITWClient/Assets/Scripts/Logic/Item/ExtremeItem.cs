using UnityEngine;
using System.Collections;
using System;

public class ExtremeItem : IItem
{
    [SerializeField]
    private float duration;
    private ICharacter user;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void UseItem(ICharacter user)
    {
        this.user = user;
        foreach(SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = false;
        }
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(ItemEffectProcess());
    }

    private IEnumerator ItemEffectProcess()
    {
        SfxManager.Instance.Play(SfxType.Item_GetExtreme);
        user.IsExtremeMp = true;
        float elapsedTime = 0.0f;
        while(elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            if(user != null)
            {
                user.Mp = user.MaxMp;
            }
        }
        if(user != null)
        {
            user.IsExtremeMp = false;
        }

        Destroy();
    }
}