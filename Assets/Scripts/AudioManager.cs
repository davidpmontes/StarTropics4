using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    public float volume;
    public float pitch;

    public bool loop;

    [HideInInspector] public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Sound[] sounds;

    private Dictionary<string, Sound> soundsDict;

    private void Awake()
    {
        Instance = this;

        soundsDict = new Dictionary<string, Sound>();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            soundsDict.Add(s.name, s);
        }
    }

    public void Play(string name)
    {
        if (soundsDict[name].source.isPlaying)
        {
            return;
        }

        soundsDict[name].source.Play();
    }

    public void PlayOverlapping(string name)
    {
        soundsDict[name].source.PlayOneShot(soundsDict[name].source.clip);
    }

    public void Stop(string name)
    {
        soundsDict[name].source.Stop();
    }
}
