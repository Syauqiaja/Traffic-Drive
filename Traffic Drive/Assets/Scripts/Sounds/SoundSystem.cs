using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundSystem {

    public static GameObject soundSource;
    public static AudioSource source;
    public static float timeToPlay = 0f;
    
    public enum Tracks{
        Belok,
         Tombol,
         Klakson,
         TabrakBerat,
         TabrakSedang,
         TabrakRingan,

    }

    private static bool CanPlay(Tracks sound, float len){
        if(sound == Tracks.Belok){
            if(timeToPlay + len < Time.time){
                timeToPlay = Time.time;
                return true;
            }else{
                return false;
            }
        }
        return true;
    }

    public static void PlaySounds(Tracks sound){
        AudioClip clip = GetSounds(sound);
        if(CanPlay(sound, clip.length)){
            if(soundSource == null) {
                soundSource = new GameObject("sounds");
                source = soundSource.AddComponent<AudioSource>();
                source.volume = 0.2f;
            }
            source.PlayOneShot(clip);
        }
    }
    public static void PlaySounds(Tracks sound, float volume){
        AudioClip clip = GetSounds(sound);
        if(CanPlay(sound, clip.length)){
            if(soundSource == null) {
                soundSource = new GameObject("sounds");
                source = soundSource.AddComponent<AudioSource>();
                source.volume = volume;
            }
            source.PlayOneShot(clip);
        }
    }
    private static AudioClip GetSounds(Tracks sound){
        foreach (Sounds item in SoundManager.Instance.AudioSounds)
        {
            if(item.title == sound){
                return item.audioClip;
            }
        }
        return null;
    }
}

[System.Serializable]
public class Sounds{
    public AudioClip audioClip;
    public SoundSystem.Tracks title;
}
