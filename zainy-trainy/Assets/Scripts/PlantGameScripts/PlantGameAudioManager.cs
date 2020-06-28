using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class PlantGameAudioManager : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioClip[] jumpClips;
    public AudioClip gulp;
    public List<AudioSource> jumpSources;
    AudioSource gulpSource;
    int jumpSoundCounter;
    void Awake()
    {
        foreach (AudioClip clip in jumpClips)
        {
            jumpSources.Add(gameObject.AddComponent<AudioSource>());
            jumpSources[jumpSources.Count - 1].clip = clip;
            jumpSources[jumpSources.Count - 1].volume = 1f;
        }
        gulpSource = gameObject.AddComponent<AudioSource>();
        gulpSource.clip = gulp;
        gulpSource.volume = 1;

    }

    public void playJumpSound()
    {
        jumpSources[jumpSoundCounter].Play();
        jumpSoundCounter = (jumpSoundCounter + 1) % 4;
    }

    public void playGulpSound()
    {
        gulpSource.Play();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
