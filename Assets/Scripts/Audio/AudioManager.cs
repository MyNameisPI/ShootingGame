using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] private AudioSource _sFXPlayer;

    const float MIN_PITCH = 0.9f;
    const float MAX_PITCH = 1.1f;

    public void PlaySFX(AudioData audioData)
    {
        //使用PlayOneShot()方法 在播放新的音乐时不会把之前的关掉
        _sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    public  void PlayRandomSFX(AudioData audioData)
    {
        _sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
        PlaySFX(audioData);
    }

    public void PlayRandomSFX(AudioData[] audioDatas)
    {
        PlayRandomSFX(audioDatas[Random.Range(0, audioDatas.Length)]);
    }
}
