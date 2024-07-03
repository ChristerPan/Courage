using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class NPCSoundEffects : MonoBehaviour
{
    public AudioClip soundEffect;
    public bool canPlayAudio;
    public AudioMixerGroup outputMixerGroup;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = outputMixerGroup;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canPlayAudio = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canPlayAudio = false;
        }
    }

    public void PlayAudio()
    {
        if (canPlayAudio)
        {
            audioSource.clip = soundEffect;
            audioSource.Play();
        }
    }
    

}
