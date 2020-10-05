using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraBackgroundDoomPhase : MonoBehaviour
{
    public Timer timer;
    public int manifestPhase = 5;

    void Start()
    {
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        if (this.timer.GetPhase() <= this.manifestPhase)
        {
            this.gameObject.GetComponent<Camera>().backgroundColor = new Color(0.2f, 0.001f, 0.0f);
        }
        else
        {
            this.gameObject.GetComponent<Camera>().backgroundColor = new Color(0.0f, 0.05f, 0.2f);
        }
    }
}
