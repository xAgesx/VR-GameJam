using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerSettings : MonoBehaviour
{
    public static PlayerSettings Instance;

    [Header("Audio Sources")]
    public List<AudioSource> sfxSources = new List<AudioSource>();
    public AudioSource bgmSource;

    [Header("Sliders")]
    public List<Slider> sfxSliders = new List<Slider>();
    public List<Slider> bgmSliders = new List<Slider>();

    [Range(0f, 1f)]
    public float sfxVolume;
    [Range(0f, 1f)]
    public float bgmVolume;

    private void Awake()
    {
        Instance = this;

        // Load saved volumes
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
    }

    private void Start()
    {
        FindAudioSources();
        ApplyVolumes();
        UpdateSliders();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAudioSources();
        ApplyVolumes();
        UpdateSliders();
    }

    /// <summary>
    /// Called by VolumeSlider to register sliders
    /// </summary>
    public void RegisterSlider(Slider slider, VolumeSlider.SliderType type)
    {
        if (slider == null) return;

        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.wholeNumbers = false;

        slider.onValueChanged.RemoveAllListeners();

        if (type == VolumeSlider.SliderType.SFX)
        {
            if (!sfxSliders.Contains(slider))
                sfxSliders.Add(slider);

            slider.onValueChanged.AddListener((value) => SetSFXVolume(value));
            slider.value = sfxVolume;
        }
        else if (type == VolumeSlider.SliderType.BGM)
        {
            if (!bgmSliders.Contains(slider))
                bgmSliders.Add(slider);

            slider.onValueChanged.AddListener((value) => SetBGMVolume(value));
            slider.value = bgmVolume;
        }

        UpdateSliders();
    }

    /// <summary>Called when SFX slider moves</summary>
    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        ApplyVolumes();
        UpdateSliders();

        // Save to PlayerPrefs
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    /// <summary>Called when BGM slider moves</summary>
    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        ApplyVolumes();
        UpdateSliders();

        // Save to PlayerPrefs
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.Save();
    }

    private void ApplyVolumes()
    {
        foreach (var sfx in sfxSources)
            if (sfx != null)
                sfx.volume = sfxVolume;

        if (bgmSource != null)
            bgmSource.volume = bgmVolume;
    }

    private void UpdateSliders()
    {
        sfxSliders.RemoveAll(s => s == null);
        bgmSliders.RemoveAll(s => s == null);

        foreach (var slider in sfxSliders)
            slider.value = sfxVolume;

        foreach (var slider in bgmSliders)
            slider.value = bgmVolume;
    }

    private void FindAudioSources()
    {
        sfxSources.Clear();
        bgmSource = null;

        foreach (var obj in GameObject.FindGameObjectsWithTag("SFX"))
        {
            var source = obj.GetComponent<AudioSource>();
            if (source != null)
                sfxSources.Add(source);
        }

        GameObject bgmObj = GameObject.FindGameObjectWithTag("BGM");
        if (bgmObj != null)
            bgmSource = bgmObj.GetComponent<AudioSource>();
    }
}
