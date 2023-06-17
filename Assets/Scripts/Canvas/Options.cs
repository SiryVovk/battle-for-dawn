using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [Header ("Volume")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    [Header ("mainMenu")]
    [SerializeField] private GameObject menu;

    private string musicHashString = "MusicVolume";
    private string soundsHashString = "SoundsVolume";

    private void Start()
    {
        if (PlayerPrefs.HasKey(soundsHashString) || PlayerPrefs.HasKey(musicHashString))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSoundsVolume();
        }

        BackButton();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat(musicHashString, volume);
        PlayerPrefs.SetFloat(musicHashString, volume);
    }

    public void SetSoundsVolume()
    {
        float volume = soundSlider.value;
        audioMixer.SetFloat(soundsHashString, volume);
        PlayerPrefs.SetFloat(soundsHashString, volume);
    }
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat(musicHashString);
        soundSlider.value = PlayerPrefs.GetFloat(soundsHashString);

        SetMusicVolume();
        SetSoundsVolume();
    }

    public void BackButton()
    {
        this.gameObject.SetActive(false);
        menu.SetActive(true);
    }
}
