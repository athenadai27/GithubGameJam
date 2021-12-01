using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    
    [Serializable]
    public struct StringClipPair
    {
        public string sceneName;
        public AudioClip clip;
    }

    [SerializeField]
    List<StringClipPair> sceneClipMappings;

    [SerializeField]
    List<StringClipPair> areaClipMappings;

    AudioSource audioSource;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip newClip = null;

        foreach (var pair in sceneClipMappings)
        {
            if (pair.sceneName == scene.name)
            {
                newClip = pair.clip;
                break;
            }
        }

        if (newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void TriggerMusic(string key)
    {
        AudioClip newClip = null;

        foreach (var pair in areaClipMappings)
        {
            if (pair.sceneName == key)
            {
                newClip = pair.clip;
            }
        }

        if (newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
