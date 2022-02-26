using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestButton : MonoBehaviour
{
    [SerializeField]
    private GameObject progressQuest;

    public static bool Yesbutton = false;
    public static bool Nobutton = false;

    public void OnYesButton()
    {
        progressQuest.SetActive(true);
        Yesbutton = true;
    }

    public void OnNoButton()
    {
        Nobutton = true;
    }
}
