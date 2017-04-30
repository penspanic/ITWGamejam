using UnityEngine;
using System.Collections;

public class ParticleEmitterActivater : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem[] particleSystems;

    private void Awake()
    {

    }

    public void SetEmitterEnable()
    {
        foreach(ParticleSystem system in particleSystems)
        {
            ParticleSystem.EmissionModule module = system.emission;
            module.enabled = true;
        }
    }

    public void SetEmitterDisable()
    {
        foreach(ParticleSystem system in particleSystems)
        {
            ParticleSystem.EmissionModule module = system.emission;
            module.enabled = false;
            system.Clear();
        }
    }
}