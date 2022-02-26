using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBarC : MonoBehaviour
{
    public void CheckBgmSlider(float value)
    {
        GameManager.Sound.SetBgmVolume(value);
        PlayerPrefs.SetFloat("BgmVolume", value);
    }

    public void CheckEffectSlider(float value)
    {
        GameManager.Sound.SetEffectVolume(value);
        PlayerPrefs.SetFloat("EffectVolume", value);
    }
}
