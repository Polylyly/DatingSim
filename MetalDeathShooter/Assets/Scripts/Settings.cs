using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject normalCanvas, settingsCanvas;

    [Header("Sliders")]
    public Slider musicVolume, sfxVolume;

    [Header("Text Boxes")]
    public TextMeshProUGUI musicVolumeText, sfxVolumeText;

    [Header("Mixers")]
    public AudioMixer sfxMixer, musicMixer;

    public void MusicVolume(float musicSliderValue)
    {
        musicMixer.SetFloat("musicVol", Mathf.Log10(musicSliderValue) * 20);
    }
    public void SFXVolume(float sfxSliderValue)
    {
        sfxMixer.SetFloat("sfxVol", Mathf.Log10(sfxSliderValue) * 20);
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("Music Volume")) PlayerPrefs.SetFloat("Music Volume", 1);
        if (!PlayerPrefs.HasKey("SFX Volume")) PlayerPrefs.SetFloat("SFX Volume", 1);

        musicVolume.value = PlayerPrefs.GetFloat("Music Volume");
        sfxVolume.value = PlayerPrefs.GetFloat("SFX Volume");
    }

    void Update()
    {
        musicVolumeText.SetText("Music: " + Mathf.RoundToInt(musicVolume.value * 100));
        sfxVolumeText.SetText("SFX: " + Mathf.RoundToInt(sfxVolume.value * 100));
    }
    public void Back()
    {
        normalCanvas.SetActive(true);
        settingsCanvas.SetActive(false);

        PlayerPrefs.SetFloat("Music Volume", musicVolume.value);
        PlayerPrefs.SetFloat("SFX Volume", sfxVolume.value);
    }
}
