using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolum(_musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolum(_sfxSlider.value);
    }

}
