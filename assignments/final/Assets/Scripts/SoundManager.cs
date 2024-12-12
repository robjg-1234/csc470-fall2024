using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager 
{
    public static void unlockPuzzle()
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource src = soundGameObject.AddComponent<AudioSource>();
       src.PlayOneShot(GameManager.instance.puzzleUnlockSound);
    }
    public static void startFire(AudioClip fireSound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource src = soundGameObject.AddComponent<AudioSource>();
        src.PlayOneShot(fireSound);
    }
}
