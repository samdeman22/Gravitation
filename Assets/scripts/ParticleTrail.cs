using UnityEngine;
using System.Collections;

public class ParticleTrail : MonoBehaviour
{
    public ParticleSystem particleSystem { get; set; }

    void Start()
    {
        //particleSystem = (ParticleSystem)gameObject.AddComponent(typeof(ParticleSystem));
        //particleSystem.simulationSpace = ParticleSystemSimulationSpace.World;
    }

    void Update()
    {
        //ParticleSystem.Particle[] p = new ParticleSystem.Particle[particleSystem.particleCount + 1];
        //int l = particleSystem.GetParticles(p);
        //for (int i = 0; i < l; i++)
        //    p[i].velocity = Vector3.zero;
        //particleSystem.SetParticles(p, l);
    }
}
