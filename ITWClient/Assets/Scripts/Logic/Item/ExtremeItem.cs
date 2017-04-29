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
        StartCoroutine(ItemEffectProcess());
    }

    private IEnumerator ItemEffectProcess()
    {
        float elapsedTime = 0.0f;
        while(elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            user.Mp = user.MaxMp;
        }
        Destroy();
    }
}