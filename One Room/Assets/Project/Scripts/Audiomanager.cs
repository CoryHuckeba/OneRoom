using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    ComputerOn,
    ComputerOff,
    DroneLeaving,
    DroneReturn,
    InvalidCommand,
    Background,
}

public class AudioManager : Singleton<AudioManager> {

    public SoundProfile[] sounds;
    public AudioSource source;
    public AudioClip backgroundNoise;

	// Use this for initialization
	void Start ()
    {
        
	}
	
    private AudioClip GetClip(SoundType t)
    {
        foreach (SoundProfile p in sounds)
        {
            if (p.type == t)
                return p.clip;
        }

        return null;
    }

    public void playSound(SoundType type)
    {
        AudioClip clip = GetClip(type);
        
        if (clip != null)
        {
            source.PlayOneShot(clip);
        }
    }
}

public class SoundProfile
{
    public SoundType type;
    public AudioClip clip;
}
