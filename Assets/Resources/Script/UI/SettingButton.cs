using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButton : MonoBehaviour
{
    [SerializeField]
    public GameObject setsetting;
    [SerializeField]
    public GameObject skillmanagement;
    [SerializeField]
    public GameObject statusmanagement;
    [SerializeField]
    public GameObject inventroy;

    bool isBool = false;

    private void Start()
    {
        isBool = false;
    }

    private void Update()
    {
        if(isBool == false)
        {
            inventroy.SetActive(false);  // 슬롯받기위해 넣어줌
            isBool = true;
        }

    }

    public void OnSettingButtonClick()  // 다중창
    {
        if(!setsetting.activeSelf)
        {
            setsetting.SetActive(true);
            GameManager.Sound.PlayEffectSound("Setting/SettingSound");
        }
        else if(setsetting.activeSelf)
        {
            setsetting.SetActive(false);
            statusmanagement.SetActive(false);
            inventroy.SetActive(false);
            skillmanagement.SetActive(false);
            GameManager.Sound.PlayEffectSound("Setting/SettingSound");
        }
    }

    public void OnSkillManageMentClick()  // 스킬창
    {
        if (!skillmanagement.activeSelf)
        {
            skillmanagement.SetActive(true);
            statusmanagement.SetActive(false);
            inventroy.SetActive(false);
            GameManager.Sound.PlayEffectSound("Setting/SettingSound");
        }
        else if(skillmanagement.activeSelf)
        {
            skillmanagement.SetActive(false);
            GameManager.Sound.PlayEffectSound("Setting/SettingSound");
        }
    }

    public void OnStatusManageMentClick() // 스탯창
    {
        if (!statusmanagement.activeSelf)
        {
            statusmanagement.SetActive(true);
            skillmanagement.SetActive(false);
            inventroy.SetActive(false);
            GameManager.Sound.PlayEffectSound("Setting/SettingSound");
        }
        else if(statusmanagement.activeSelf)
        {
            statusmanagement.SetActive(false);
            GameManager.Sound.PlayEffectSound("Setting/SettingSound");
        }
    }

    public void OnInventroyClick() // 인벤토리창
    {
        if (!inventroy.activeSelf)
        {
            inventroy.SetActive(true);
            skillmanagement.SetActive(false);
            statusmanagement.SetActive(false);
            GameManager.Sound.PlayEffectSound("Setting/SettingSound");
        }
        else if (inventroy.activeSelf)
        {
            inventroy.SetActive(false);
            GameManager.Sound.PlayEffectSound("Setting/SettingSound");
        }
    }
}
