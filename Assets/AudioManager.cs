using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioSource AudioSource;
    public static void PlayPaddleHit()
    {
        PlayClip("metal-clang");
    }

    public static void PlayWallHit()
    {
        PlayClip("force-field-impact");
    }

    public static void PlayGameOverLeft()
    {
        PlayClip("game-over");
    }

    public static void PlayGameOverRight()
    {
        PlayClip("game-over-arcade");
    }

    private static void PlayClip(string soundName)
    {
        var clip = Resources.Load<AudioClip>("sounds/" + soundName);
        AudioSource.PlayOneShot(clip);
    }
}
