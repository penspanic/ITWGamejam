using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EffectType
{
    Hit,
    Charge,
    Die,
    Die_Portrait,
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
        effectPrefabs.Add(EffectType.Charge, Resources.Load<GameObject>("Prefabs/Effect/Charge"));
        effectPrefabs.Add(EffectType.Die, Resources.Load<GameObject>("Prefabs/Effect/Die"));
        effectPrefabs.Add(EffectType.Die_Portrait, Resources.Load<GameObject>("Prefabs/Effect/Die_Portrait"));
    }

    public GameObject ShowEffect(EffectType type, Vector2 pos, Transform parent = null)
    {
        GameObject newEffect = Instantiate(effectPrefabs[type]);
        if(parent != null)
            newEffect.transform.SetParent(parent);
        newEffect.transform.localPosition = pos;

        return newEffect;
    }
}