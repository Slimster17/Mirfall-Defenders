using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource musicAudioSource; // AudioSource component for playing music

    private void Start() // Start is called before the first frame update
    {
        musicAudioSource = gameObject.AddComponent<AudioSource>(); 
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic() // Method to play background music
    {
        // Debug.Log("Playing background music");
        SoundManager.PlayLoopingSound(SoundType.Music, musicAudioSource, 0.5f);
        StartCoroutine(CheckMusicPlaying());
    }

    private IEnumerator CheckMusicPlaying() // Coroutine to check if music is playing
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
