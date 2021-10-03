using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
        Debug.Log($"Length {systems.Length}");
        foreach(ParticleSystem p in systems)
        {
            p.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
