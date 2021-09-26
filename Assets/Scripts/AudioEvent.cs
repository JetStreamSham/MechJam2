using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent : MonoBehaviour
{

    public int concurrentSounds = 4;
    public AudioSource[] source;
    public AudioClip[] audioClips;


    private void Start()
    {
        source = new AudioSource[concurrentSounds];
        for (int i = 0; i < concurrentSounds; i++)
        {
            GameObject childObject = new GameObject($"{gameObject.name} Audio Source {i}");
            childObject.transform.parent = transform;
            childObject.transform.position = Vector3.zero;
            childObject.transform.rotation = transform.rotation;
            source[i] = childObject.AddComponent<AudioSource>();
        }

    }
    public void PlayClip(int clipID)
    {
        for(int i = 0; i < concurrentSounds; i++)
        {
            if (!source[i].isPlaying)
            {
                source[i].clip = audioClips[clipID];
                source[i].Play();
                break;
            }
        }
    }
}
