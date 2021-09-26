using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent : MonoBehaviour
{

    public AudioSource source;
    public AudioClip[] audioClips;

    public void PlayClip(int clipID)
    {
        source.clip = audioClips[clipID];
        source.Play();
    }
}
