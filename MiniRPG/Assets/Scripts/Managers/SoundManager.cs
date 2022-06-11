using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _effectAudioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);
        }

        string[] names = System.Enum.GetNames(typeof(Define.Sound));
        for (int i = 0; i < _audioSources.Length; i++)
        {
            GameObject go = new GameObject { name = names[i] };
            go.transform.parent = root.transform;
            _audioSources[i] = go.GetOrAddComponent<AudioSource>();
        }

        _audioSources[(int)Define.Sound.BGM].loop = true;
    }

    public void Play(string path, Define.Sound soundType = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, soundType);
        Play(audioClip, soundType, pitch);
    }

    public void Play(AudioClip audioClip, Define.Sound soundType = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioSource audioSource = _audioSources[(int)soundType];

        if (audioClip == null)
            return;
        if (soundType == Define.Sound.BGM)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
            audioSource.clip = audioClip;
            audioSource.pitch = pitch;
            audioSource.Play();
        }
        else
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }

    }

    public AudioClip GetOrAddAudioClip(string path, Define.Sound soundType)
    {
        AudioClip audioClip = null;
        if (path.Contains("Sound/") == false)
            path = $"Sound/{path}";

        if (soundType == Define.Sound.BGM)
        {
            audioClip = Manager.Resource.Load<AudioClip>(path);
        }
        else
        {
            _effectAudioClips.TryGetValue(path, out audioClip);
            if (audioClip == null)
            {
                audioClip = Manager.Resource.Load<AudioClip>(path);
                _effectAudioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log("Failed GetOrAddAudioClip");
        return audioClip;
    }

    public void Close()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        _effectAudioClips.Clear();
    }
}
