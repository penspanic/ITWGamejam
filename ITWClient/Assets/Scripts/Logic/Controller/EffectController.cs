using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EffectType
{
    Hit,
}

public class EffectController : Singleton<EffectController>
{
    private Dictionary<EffectType, GameObject> effectPrefabs = new Dictionary<EffectType, GameObject>();
    protected override void Awake()
    {
    }

    public void LoadEffects()
    {
        effectPrefabs.Add(EffectType.Hit, Resources.Load<GameObject>("Prefabs/Effect/Hit"));
    }

    public void ShowEffect(EffectType type, Vector2 pos, Transform parent = null)
    {
        GameObject newEffect = Instantiate(effectPrefabs[type]);
        if(parent != null)
            newEffect.transform.SetParent(parent);
        newEffect.transform.localPosition = pos;
    }
}