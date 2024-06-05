using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource musicAudioSource;

    private void Start()
    {
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        // Debug.Log("Playing background music");
        SoundManager.PlayLoopingSound(SoundType.Music, musicAudioSource, 0.5f);
        StartCoroutine(CheckMusicPlaying());
    }

    private IEnumerator CheckMusicPlaying()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!musicAudioSource.isPlaying && musicAudioSource.time == 0)
            {
                PlayBackgroundMusic();
            }
        }
    }
}
