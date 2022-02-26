using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlankSkillButton : MonoBehaviour
{
    public static BlankSkillButtonSet[] skillSlot;
    public static bool SkillBool = false;

    public void Awake()
    {
        skillSlot = GetComponentsInChildren<BlankSkillButtonSet>();
    }

    public void Start()
    {
        if (SkillBool == true)
        {
            GameManager.Json.SkillLoadData("SaveData");
            SkillBool = false;
        }
    }

}
