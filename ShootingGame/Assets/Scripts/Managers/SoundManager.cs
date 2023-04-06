using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("SoundManager is not null");
            return;
        }

        instance = this;
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
        Debug.Log(audioClip.name);
    }
}
