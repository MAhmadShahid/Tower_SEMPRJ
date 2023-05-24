using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    
    public static AudioManager instance;
    void Awake()
    {
        // to prevent Multiple instances of Audio Manager. Detail Below
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        //Detail:
        //Below function will prevent cut off of audio when we switch scenes. And create new instances of audio manager when we switch to new scene.
        //But it will result in multiple instances of a game object AudioManager. Above if-else will prevent from this
        DontDestroyOnLoad(gameObject);

        


        foreach(Sound s in sounds)
        {
            s.source = gameObject.GetComponent<AudioSource>();
            
            s.source.clip = s.clip;
            s.source.volume= s.volume;
            s.source.pitch= s.pitch;
            s.source.loop= s.loop;
        }
    }

    private void Start()
    {
        Play("theme");
    }


    //Apply sound of your choice on any event you want by calling this method and give name with which you added sound in Sounds array.\

    // you can call this method by the syntax => [' FindObjectOfType<AudioManager>().Play("[name of sound]"); ']
    public void Play(string name)
    {
        Sound s = Array.Find(sounds,sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound not found");
            return;
        }
        s.source.Play();

        
    }
}
