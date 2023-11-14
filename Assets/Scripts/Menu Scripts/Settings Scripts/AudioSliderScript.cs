using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSliderScript : MonoBehaviour
{
    [SerializeField] private Slider audioSlider;
    [SerializeField] private TMP_Text currentAudioVolume;

    private void Start()
    {
        audioSlider.value = (float)SettingsData.settingVolumeScale;
        
        currentAudioVolume.SetText(Mathf.RoundToInt((float)(SettingsData.settingVolumeScale * 100)) + "%");

        audioSlider.onValueChanged.AddListener((audioLevel) =>
        {
            SettingsData.settingVolumeScale = audioLevel;
            currentAudioVolume.SetText(Mathf.RoundToInt((float)(SettingsData.settingVolumeScale * 100)) + "%");
            
            Debug.Log(SettingsData.settingVolumeScale);
        });
    }
}