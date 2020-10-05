using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DoomParticles : MonoBehaviour
{
    public Timer timer;
    public int manifestPhase = 5;

    private ParticleSystemRenderer doomParticles;

    void Start()
    {
        this.doomParticles = this.gameObject.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>();
    }
    
    void Update()
    {
    }

    void FixedUpdate()
    {
        if(this.timer.GetPhase() <= this.manifestPhase)
        {
            this.doomParticles.enabled = true;
        }
        else
        {
            this.doomParticles.enabled = false;
        }
    }
}
