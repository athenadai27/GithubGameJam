using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class MusicManager : MonoBehaviour
{
    public static MusicManager mPlayer;
    
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
    }

    void Start()
    {
        //this.gameObject.GetComponent<AudioSource>().Play();
    }

}
