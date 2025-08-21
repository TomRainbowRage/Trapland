using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioSource audioSrc;
    public static bool ShowDebug = false;

    public static IDictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(AudioClip clip, string name = null)
    {
        if(name != null && !audioClips.ContainsKey(name))
        {
            audioClips.Add(name, clip);
        }
        
        if (audioSrc != null)
        {
            if(ShowDebug) Debug.LogWarning("Playing " + clip + "  Name : " + name);
            audioSrc.PlayOneShot(clip);
        }

    }

    public static void PlaySound(string name)
    {
        if(audioClips.ContainsKey(name))
        {
            if (audioSrc != null)
            {
                if(ShowDebug) Debug.LogWarning("Playing Name : " + name);
                audioSrc.PlayOneShot(audioClips[name]);
            }
        }
    }
}
