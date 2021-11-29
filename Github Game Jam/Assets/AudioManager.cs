using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //Athena Stuff
    public AudioSource aus;
    public AudioClip auclip;
    public AudioMixer audioMixer;
    public Image musicFill;
    public Image sfxFill;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        aus = GameObject.Find("MusicPlayer").GetComponent<AudioSource>();
        aus.clip = auclip;
        aus.Play();

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1f);
        }
        
        audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Max(PlayerPrefs.GetFloat("MusicVolume"), .0001f)) * 20);

        if(!PlayerPrefs.HasKey("SFXVolume")){
            PlayerPrefs.SetFloat("SFXVolume",1f);
        }

        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(PlayerPrefs.GetFloat("SFXVolume"), .0001f)) * 20);


        float musicSliderValue = PlayerPrefs.GetFloat("MusicVolume");
        musicFill.fillAmount = musicSliderValue;
        musicSlider.value = musicSliderValue;

        float sfxSliderValue = PlayerPrefs.GetFloat("SFXVolume");
        sfxFill.fillAmount = sfxSliderValue;
        sfxSlider.value = sfxSliderValue;
    }

    public void SetMusicVolume(Slider volume)
    {
        musicFill.fillAmount = volume.value;
        Debug.Log(Mathf.Log10(Mathf.Max(volume.value, .0001f)) * 20);
        audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Max(volume.value, .0001f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume.value);
    }
    public void SetSFXVolume(Slider volume)
    {
        sfxFill.fillAmount = volume.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(volume.value, .0001f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume.value);
    }

}
