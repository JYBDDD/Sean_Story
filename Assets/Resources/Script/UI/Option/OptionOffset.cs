using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionOffset : MonoBehaviour
{
    private GameObject option;
    [SerializeField]
    private GameObject optionBar;

    private void Start()
    {
        option = gameObject;
    }

    public void OptionClick()
    {
        if (!optionBar.activeSelf)
        {
            optionBar.SetActive(true);
            GameManager.Sound.PlayEffectSound("Option/OptionOnOffSound");
        }
        else if (optionBar.activeSelf)
        {
            optionBar.SetActive(false);
            GameManager.Sound.PlayEffectSound("Option/OptionOnOffSound");
        }
    }
}
