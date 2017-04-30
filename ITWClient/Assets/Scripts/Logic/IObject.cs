using UnityEngine;
using System.Collections;

public interface IObject
{
    int Hp { get; }
    void OnHit(IObject attacker, int damage, bool forced = false);
}