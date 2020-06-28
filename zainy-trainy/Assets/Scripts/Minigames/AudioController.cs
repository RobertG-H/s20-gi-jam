using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class AudioController : MonoBehaviour
{
    protected AudioSource audioSource;

    [Serializable]
    protected class SFX
    {
        [SerializeField]
        private AudioClip audioClip;
        [SerializeField]
        [Range(0f,1f)]
        private float volume = 1f;
        [SerializeField]
        [Range(0f, 0.2f)]
        private float volumeVariation = 0.05f;
        [SerializeField]
        [Range(0f, 2f)]
        private float pitch = 1f;
        [SerializeField]
        [Range(0f, 0.2f)]
        private float pitchVariation = 0.05f;
        [SerializeField]
        public AudioClip GetAudioClip()
        {
            return audioClip;
        }
        public float GetVolume()
        {
            return volume + UnityEngine.Random.Range(-volumeVariation, volumeVariation);
        }
        public float GetPitch()
        {
            return pitch + UnityEngine.Random.Range(-pitchVariation, pitchVariation);
        }
    }
    void Awake()
    {
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    protected void PlaySFX(SFX sfx)
    {
        audioSource.pitch = sfx.GetPitch();
        audioSource.PlayOneShot(sfx.GetAudioClip(), sfx.GetVolume());
    }


    //To play clips when something dies (Won't stop playing if the GameObject is destroyed)
    protected void PlayLingeringSFX(SFX sfx)
    {
        AudioSource tempSource = new GameObject().AddComponent<AudioSource>() as AudioSource;
        tempSource.gameObject.name = String.Format("SFX {0}", sfx.GetAudioClip().name);
        tempSource.pitch = sfx.GetPitch();
        tempSource.PlayOneShot(sfx.GetAudioClip(), sfx.GetVolume());        
        Destroy(tempSource.gameObject, sfx.GetAudioClip().length);
        // tempSource.GetComponent<AudioSource>();
        // AudioSource.PlayClipAtPoint(sfx.GetAudioClip(), transform.position, sfx.GetVolume());
    }

    //Manually reference the source if sound effect plays in start.
    public void RefAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
