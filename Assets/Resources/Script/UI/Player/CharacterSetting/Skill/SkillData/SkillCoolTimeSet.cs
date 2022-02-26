using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTimeSet : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonP;
    [SerializeField]
    public Image CoolTimeImg;
    [SerializeField]
    public Text CoolTimeText;
    [SerializeField]
    public Image ActiveImg;

    private void Start()
    {
        CoolTimeImg.enabled = false;
        CoolTimeText.enabled = false;
        ActiveImg.enabled = false;
    }

    private void Update()
    {
        if(buttonP.GetComponent<BlankSkillButtonSet>().Skill != null)
        {
            if (buttonP.GetComponent<BlankSkillButtonSet>().Skill.skillName != "")  // skill 데이터가 null값이 아닐때 실행(오류막아주기)
            {
                activeImg();
                CoolTimeLook();
            }
        }
        

    }

    public void CoolTimeLook()  // 쿨타임 이미지표시
    {
        if (buttonP.GetComponent<BlankSkillButtonSet>().Skill.skillName == "Lightning Enchantment")
        {
            CoolTimeImg.enabled = true;
            CoolTimeText.enabled = true;
            CoolTimeText.text = $"{string.Format("{0:N1}", CharacterBaseController.CheckEnchantTime)}";

            if (CharacterBaseController.CheckEnchantTime <= 0)
            {
                CoolTimeImg.enabled = false;
                CoolTimeText.enabled = false;

            }
        }
        if (buttonP.GetComponent<BlankSkillButtonSet>().Skill.skillName == "Slash")
        {
            CoolTimeImg.enabled = true;
            CoolTimeText.enabled = true;
            CoolTimeText.text = $"{string.Format("{0:N1}", CharacterBaseController.CheckSlachTime)}";

            if (CharacterBaseController.CheckSlachTime <= 0)
            {
                CoolTimeImg.enabled = false;
                CoolTimeText.enabled = false;
            }
        }

    }

    private void activeImg()  // 활성화 중인 스킬 이미지표시
    {
        if (buttonP.GetComponent<BlankSkillButtonSet>().Skill.skillName == "Lightning Enchantment")
        {
            if (CharacterBaseController.EnchantactiveTime <= 0)
            {
                ActiveImg.enabled = false;
            }
            if (CharacterBaseController.EnchantactiveTime > 0)
            {
                ActiveImg.enabled = true;
                ActiveImg.fillAmount = (CharacterBaseController.EnchantactiveTime / 13);
            }
        }
        if (buttonP.GetComponent<BlankSkillButtonSet>().Skill.skillName == "Slash")
        {
            if (CharacterBaseController.SlashactiveTime <= 0)
            {
                ActiveImg.enabled = false;
            }
            if (CharacterBaseController.SlashactiveTime > 0)
            {
                ActiveImg.enabled = true;
                ActiveImg.fillAmount = (CharacterBaseController.SlashactiveTime / 3);
            }
        }

    }
}
