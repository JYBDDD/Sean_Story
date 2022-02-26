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
            if (buttonP.GetComponent<BlankSkillButtonSet>().Skill.skillName != "")  // skill �����Ͱ� null���� �ƴҶ� ����(���������ֱ�)
            {
                activeImg();
                CoolTimeLook();
            }
        }
        

    }

    public void CoolTimeLook()  // ��Ÿ�� �̹���ǥ��
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

    private void activeImg()  // Ȱ��ȭ ���� ��ų �̹���ǥ��
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
