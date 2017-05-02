using UnityEngine;
using System.Collections;

public delegate void OnObjectDestroyed(IObject target);
public interface IObject
{
    int Hp { get; }
    void OnHit(IObject attacker, int damage, bool forced = false);

    event OnObjectDestroyed OnDestroyed;
}