using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScripts : MonoBehaviour
{
    public static AudioScripts instance { get; private set; }
    private AudioSource source;
    

    void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
      
    }
    public void playSound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }
    

}
