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
            inventroy.SetActive(false);  // ���Թޱ����� �־���
            isBool = true;
        }

    }

    public void OnSettingButtonClick()  // ����â
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

    public void OnSkillManageMentClick()  // ��ųâ
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

    public void OnStatusManageMentClick() // ����â
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

    public void OnInventroyClick() // �κ��丮â
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
