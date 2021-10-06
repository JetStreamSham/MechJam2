using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioEvent : MonoBehaviour
{

    public int concurrentSounds = 4;
    public AudioSource[] source;
    public AudioClip[] audioClips;

    public AudioMixerGroup mixer;

    private void Start()
    {

        source = GetComponentsInChildren<AudioSource>();

        if (source == null || source.Length == 0)
        {
            source = new AudioSource[concurrentSounds];
            for (int i = 0; i < concurrentSounds; i++)
            {
                GameObject childObject = new GameObject($"{gameObject.name} Audio Source {i}");
                childObject.transform.parent = transform;
                childObject.transform.position = Vector3.zero;
                childObject.transform.localPosition = Vector3.zero;
                childObject.transform.rotation = transform.rotation;
                source[i] = childObject.AddComponent<AudioSource>();
                source[i].outputAudioMixerGroup = mixer;
                source[i].maxDistance = 75f;
                source[i].spatialBlend = 1f;
            }

        }else
        {
            concurrentSounds = source.Length;
        }


    }
    public void PlayClip(int clipID)
    {
        for (int i = 0; i < source.Length; i++)
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
