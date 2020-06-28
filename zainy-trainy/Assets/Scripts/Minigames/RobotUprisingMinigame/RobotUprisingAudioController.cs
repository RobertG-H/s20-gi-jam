using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RobotUprisingAudioController : AudioController
{
    [Serializable]
    private class SFXList
    {
        public SFX type;
    }

    [SerializeField]
    private SFXList soundEffects = new SFXList();

    public void PlayType()
    {
        PlaySFX(soundEffects.type);
    }
}
