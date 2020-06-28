using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DejaVuToasterAudioController : AudioController
{
    [Serializable]
    private class SFXList
    {
        public SFX drift;
        public SFX dejaVu;
        
        public SFX gasGasGas;
    }

    [SerializeField]
    private SFXList soundEffects = new SFXList();

    public void PlayDrift()
    {
        PlaySFX(soundEffects.drift);
    }
    public AudioSource PlayDejaVu()
    {
        return PlayFadedSFX(soundEffects.dejaVu);
    }
    public void PlayGasGasGas()
    {
        PlaySFX(soundEffects.gasGasGas);
    }

    private AudioSource PlayFadedSFX(SFX sfx)
    {
        AudioSource tempSource = new GameObject().AddComponent<AudioSource>() as AudioSource;
        tempSource.gameObject.name = String.Format("SFX {0}", sfx.GetAudioClip().name);
        tempSource.pitch = sfx.GetPitch();
        tempSource.clip = sfx.GetAudioClip();
        return tempSource;
    }
}
