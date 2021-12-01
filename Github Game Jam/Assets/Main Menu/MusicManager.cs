using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager mPlayer;
    
    [Serializable]
    public struct SceneClipPair
    {
        public string sceneName;
        public AudioClip clip;
    }

    [SerializeField]
    List<SceneClipPair> clipMappings;

    AudioSource audioSource;

    void Awake()
    {
        if (mPlayer != null)
        {
            Destroy(gameObject);
        }
        else
        {
            mPlayer = this;
            DontDestroyOnLoad(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip newClip = null;

        foreach (var pair in clipMappings)
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

}
