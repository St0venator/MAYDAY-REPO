using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource jumpSFX, shootSFX, swordSFX, mineSFX;

    public void PlayMineSFX()
    {
        mineSFX.Play();
    }
    public void PlayJumpSound()
    {
        jumpSFX.Play();
    }
    public void PlayShootSFX()
    {
        shootSFX.Play();
    }
    public void PlaySwordSFX()
    {
        swordSFX.Play();
    }

    public void ChangeVolume(float value)
    {
        AudioListener.volume = value;
    }



}
