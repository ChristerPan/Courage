using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager instance;

    public AudioClip bgmClip;
    public AudioMixerGroup outputMixerGroup;


    AudioSource bgmSource;
    AudioSource currentActUnitAudioSource;
    AudioSource currentActUnitTargetAudioSource;
    private void Awake()
    {
        instance = this;

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.outputAudioMixerGroup = outputMixerGroup;
        currentActUnitAudioSource = gameObject.AddComponent<AudioSource>();
        currentActUnitAudioSource.outputAudioMixerGroup = outputMixerGroup;
        currentActUnitTargetAudioSource = gameObject.AddComponent<AudioSource>();
        currentActUnitTargetAudioSource.outputAudioMixerGroup = outputMixerGroup;

        instance.bgmSource.clip = instance.bgmClip;
        instance.bgmSource.loop = true;
        instance.bgmSource.Play();
    }

    public static void CurrentActUnitAudio(AudioClip audioClip)
    {
        instance.currentActUnitAudioSource.clip = audioClip;
        instance.currentActUnitAudioSource.Play();
    }

    public static void CurrentActUnitTargetAudio(AudioClip audioClip)
    {
        instance.currentActUnitTargetAudioSource.clip = audioClip;
        instance.currentActUnitTargetAudioSource.Play();
    }

}
