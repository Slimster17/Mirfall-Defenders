using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType // List of sounds to play
{
    ArrowShot,
    SwordSlash,
    TrebuchetRotation,
    TrebuchetFire,
    StoneHit,
    ManDeath,
    Music
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Tooltip("Array of sound configurations")]
    [SerializeField] private SoundList[] soundList;
   
    private static SoundManager instance; // Singleton instance of SoundManager
    private AudioSource audioSource; // AudioSource component for playing sounds

    private void Awake() // Awake is called when the script instance is being loaded
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    public static void PlaySound(SoundType sound, float volume = 1) // Method to play a sound effect
    {
        SoundList soundList = instance.soundList[(int)sound];
        AudioClip[] clips = soundList.sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.outputAudioMixerGroup = soundList.mixer;
        instance.audioSource.PlayOneShot(randomClip, volume * soundList.volume);
    }
    public static void PlayLoopingSound(SoundType sound, AudioSource source, float volume = 1) // Method to play a looping sound effect

    {
        SoundList soundList = instance.soundList[(int)sound];
        AudioClip[] clips = soundList.sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        source.outputAudioMixerGroup = soundList.mixer;
        source.clip = randomClip;
        source.volume = volume * soundList.volume;
        source.loop = false; // Ensure it does not loop automatically
        source.Play();
    }
    public static void StopSound() // Method to stop playing sound
    {
        instance.audioSource.Stop();
    }
    public void Resize() // Method to resize the sound list array
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        bool differentSize = names.Length != soundList.Length;

        Dictionary<string, AudioClip[]> clips = new();

        if(differentSize)
        {
            for (int i = 0; i < soundList.Length; ++i)
            {
                if (soundList[i].sounds != null)
                    clips.Add(soundList[i].name, soundList[i].sounds);
            }
        }
            
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            string currentName = names[i];
            soundList[i].name = currentName;
            if (soundList[i].volume == 0) soundList[i].volume = 1;

            if (differentSize)
            {
                if (clips.ContainsKey(currentName))
                    soundList[i].sounds = clips[currentName];
                else
                    soundList[i].sounds = null;
            }
        }
    }
    
#if UNITY_EDITOR
        [CustomEditor(typeof(SoundManager))]
        public class SoundManagerEditor : Editor
    {
        private void OnEnable()
        {
            ((SoundManager)target).Resize(); // Resize the sound list array in the editor
        }
    }

#endif
}

[Serializable]
public struct SoundList
{
    [HideInInspector] public string name; // Name of the sound type
    [Range(0, 1)] public float volume; // Volume of the sound
    public AudioMixerGroup mixer; // Audio mixer group for the sound
    public AudioClip[] sounds; // Array of audio clips for the sound
}


