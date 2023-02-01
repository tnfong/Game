using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VolumeSave : MonoBehaviour
{
    [SerializeField] private Slider volumeSiderMusic;
    [SerializeField] private Slider volumeSiderSound;
/*
    void Start()
    {
        SaveVolumeMusic();
        LoadValueMusic();
        SaveVolumeSound();
        LoadVolumeSound();
    }*/

    void Update()
    {
        LoadValueMusic();
        SaveVolumeMusic();
        LoadVolumeSound();
        SaveVolumeSound();
        
    }

    public void SaveVolumeMusic()
    {
        float volumeValueMusic = volumeSiderMusic.value;       
        PlayerPrefs.SetFloat("VolumeValueMusic", volumeValueMusic);      
    }

    public void LoadValueMusic()
    {
        float _volumeValueMusic = PlayerPrefs.GetFloat("VolumeValueMusic");    
        volumeSiderMusic.value = _volumeValueMusic;
    }

    public void SaveVolumeSound()
    {
        float volumeValueSound = volumeSiderSound.value;
        PlayerPrefs.SetFloat("VolumeValueSound", volumeValueSound);
    }

    public void LoadVolumeSound()
    {
        float _volumeValueSound = PlayerPrefs.GetFloat("VolumeValueSound");
        volumeSiderSound.value = _volumeValueSound;
    }

}
