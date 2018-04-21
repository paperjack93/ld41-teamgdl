using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    private ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        if (ps && !ps.IsAlive()) Destroy(gameObject);
    }

    void StopEmitting()
    {
        if (ps && ps.IsAlive()) ps.Stop();
    }
}