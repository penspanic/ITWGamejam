using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleEmitterActivater : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem[] particleSystems;

    private ParticleSystem.EmissionModule[] ems;

    private void Awake()
    {
        List<ParticleSystem.EmissionModule> emList = new List<ParticleSystem.EmissionModule>();
        foreach(ParticleSystem system in particleSystems)
        {
            emList.Add(system.emission);
        }
        ems = emList.ToArray();
    }

    public void SetEmitterEnable(float duration)
    {
        for(int i = 0; i < ems.Length; ++i)
        {
            ems[i].enabled = true;
        }

        Invoke("SetEmitterDisable", duration);
    }

    private void SetEmitterDisable()
    {
        for(int i = 0; i < ems.Length; ++i)
        {
            ems[i].enabled = false;
        }
    }
}