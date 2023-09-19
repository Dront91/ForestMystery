using UnityEngine;
using UnityEngine.Audio;

public class UISettings : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup Mixer;

    [SerializeField] private GameObject[] _backgroundMusic;
    [SerializeField] private GameObject[] _soundsVolume;

    private void Start()
    {
        Save();
    }

    private void SwapImage(GameObject[] _swapArray, bool enabled)
    {
        if (enabled)
        {
            _swapArray[0].SetActive(true);
            _swapArray[1].SetActive(false);
        }
        else
        {
            _swapArray[0].SetActive(false);
            _swapArray[1].SetActive(true);
        }
    }

    public void Save()
    {
        if (PlayerPrefs.GetInt("BackgroundMusicVolume") == 0)
        {
            SwapImage(_backgroundMusic, false);
            Mixer.audioMixer.SetFloat("BackgroundMusicVolume", -80);

        }
        else 
        {
            SwapImage(_backgroundMusic, true);
            Mixer.audioMixer.SetFloat("BackgroundMusicVolume", 0);
        }

        if (PlayerPrefs.GetInt("UIVolume") == 0)
        {
            SwapImage(_soundsVolume, false);
            Mixer.audioMixer.SetFloat("UIVolume", -80);
            Mixer.audioMixer.SetFloat("SFXVolume", -80);
        }
        else 
        {
            SwapImage(_soundsVolume, true);
            Mixer.audioMixer.SetFloat("UIVolume", 0);
            Mixer.audioMixer.SetFloat("SFXVolume", 0);
        }

    }

    public void ChangeBackgroundMusicVolume(bool enabled)
    {
        if (enabled)
        {
            Mixer.audioMixer.SetFloat("BackgroundMusicVolume", 0);
        }
        else
        {
            Mixer.audioMixer.SetFloat("BackgroundMusicVolume", -80);
        }

        PlayerPrefs.SetInt("BackgroundMusicVolume", enabled ? 1 : 0);
        Save();
    }


    public void ChangeSounds(bool enabled)
    {
        if (enabled)
        {
            Mixer.audioMixer.SetFloat("UIVolume", 0);
            Mixer.audioMixer.SetFloat("SFXVolume", 0);
        }
        else
        {
            Mixer.audioMixer.SetFloat("UIVolume", -80);
            Mixer.audioMixer.SetFloat("SFXVolume", -80);
        }

        PlayerPrefs.SetInt("UIVolume", enabled ? 1 : 0);
        PlayerPrefs.SetInt("SFXVolume", enabled ? 1 : 0);
        Save();
    }

}
