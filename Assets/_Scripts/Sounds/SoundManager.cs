using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    ArrowShot,
    SwordSlash,
    TrebuchetRotation,
    TrebuchetFire,
    StoneHit,
    ManDeath,
    FootStep
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        SoundList soundList = instance.soundList[(int)sound];
        AudioClip[] clips = soundList.sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.outputAudioMixerGroup = soundList.mixer;
        instance.audioSource.PlayOneShot(randomClip, volume * soundList.volume);
    }
    
    public static void StopSound()
    {
        instance.audioSource.Stop();
    }

    public void Resize()
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
            ((SoundManager)target).Resize();
        }
    }

#endif
}

[Serializable]
public struct SoundList
{
    [HideInInspector] public string name;
    [Range(0, 1)] public float volume;
    public AudioMixerGroup mixer;
    public AudioClip[] sounds;
}

