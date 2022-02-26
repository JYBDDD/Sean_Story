using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestScessSet : MonoBehaviour
{
    public static bool QuestScessBool;

    private void OnEnable()
    {
        QuestScessBool = true;
        GameManager.Sound.PlayEffectSound("Quest/QuestSusses");
    }

    public void ScessButton()
    {
        gameObject.SetActive(false);
    }
}
